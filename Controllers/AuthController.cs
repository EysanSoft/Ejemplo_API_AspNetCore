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
        public ActionResult Login (AuthUsuarioDto usuario)
        {
            string correo = usuario.Correo;
            // La contraseña no pasa por el procedimiento almacenado, se utiliza para comparar la contraseña encriptada como respuesta.
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
                    int rolId = Convert.ToInt32(data.Rows[0]["RolId"].ToString());
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
                            // Hay que obtener el Id desde el token.
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
                        // Regresar el rol.
                        return StatusCode(200, new
                        {
                            success = true,
                            user = id,
                            role = rolId,
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

        [HttpPost, Route("recuperar_contrasena")]
        public ActionResult RecuperarContra (ObtenerUsuarioDto usuario)
        {
            string correo = usuario.Correo;
            string storedProcedure = "ObtenerNombreUsuarioXCorreo";

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
                    string nombre = data.Rows[0]["Nombre"].ToString();
                    int id = Convert.ToInt32(data.Rows[0]["Id"].ToString());
                    var jwt = iConfig.GetSection("Jwt").Get<JWT>();
                    string date = Convert.ToString(new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds());
                    var claims = new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, jwt.Subject),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, date),
                    };
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));
                    var signing = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                        jwt.Issuer,
                        jwt.Audience,
                        claims,
                        expires: DateTime.Now.AddMinutes(10),
                        signingCredentials: signing
                    );
                    return StatusCode(200, new
                    {
                        success = true,
                        userName = nombre,
                        userId = id,
                        token = new JwtSecurityTokenHandler().WriteToken(token)
                    });                
                }
                else
                {
                    return StatusCode(200, new { success = false, message = "El correo ingresado no pertenece a ninguna cuenta con nosotros." });
                }
            }
            catch (Exception)
            {
                return StatusCode(500, new { success = false, error = true, message = "Error al buscar la cuenta." });
            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }
        }
    }
}
