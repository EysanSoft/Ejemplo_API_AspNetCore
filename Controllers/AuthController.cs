using ejemplo_api.Models;
using ejemplo_api.Models.DTOs.Usuario;
using ejemplov1.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ejemplo_api.Controllers
{
    [ApiController, Route("autenticar_usuario"), Tags("Autenticar Usuario")]
    public class AuthController : Controller
    {
        public IConfiguration iConfig;
        public AuthController(IConfiguration configuration)
        {
            iConfig = configuration;
        }

        [HttpPost, Route("iniciar_sesion")]
        public ActionResult Login(AuthUsuarioDto usuario)
        {
            string correo = usuario.Correo;
            string contrasena = usuario.Contrasena;
            string storedProcedure = "ObtenerUsuarioXCorreo";

            SqlConnection connection = new SqlConnection(Connection.GetConnection());
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand(storedProcedure, connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@correo", SqlDbType.NVarChar);
                command.Parameters["@correo"].Value = correo;
                DataTable data = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(data);

                if (data.Rows.Count > 0)
                {
                    int id = Convert.ToInt32(data.Rows[0]["Id"].ToString());
                    string nombreUsuario = data.Rows[0]["Nombre"].ToString();
                    string contraHash = data.Rows[0]["Contrasena"].ToString();
                    bool valid = BCrypt.Net.BCrypt.Verify(contrasena, contraHash);

                    if (valid)
                    {
                        var jwt = iConfig.GetSection("Jwt").Get<JWT>();
                        string date = Convert.ToString(new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds());
                        var claims = new[]
                        {
                            new Claim(JwtRegisteredClaimNames.Sub, jwt.Subject),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                            new Claim(JwtRegisteredClaimNames.Iat, date),
                            new Claim("usuario", id.ToString())
                        };
                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));
                        var signing = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                        var token = new JwtSecurityToken(
                            jwt.Issuer,
                            jwt.Audience,
                            claims,
                            expires: DateTime.Now.AddHours(1),
                            signingCredentials: signing
                        );

                        return StatusCode(200, new
                        {
                            success = true,
                            user = id,
                            token = new JwtSecurityTokenHandler().WriteToken(token)
                        });
                    }
                    else
                    {
                        return StatusCode(200, new
                        {
                            success = false,
                            message = "Contraseña Incorrecta."
                        });
                    }
                }
                else
                {
                    return StatusCode(200, new { success = false, message = "Correo incorrecto, no se encontró al usuario." });
                }
            }
            catch (Exception)
            {
                return StatusCode(500, new { success = false, error = true, message = "Error al iniciar sesión." });
            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }
        }
    }
}
