using ejemplo_api.Models;
using ejemplov1.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Data;

namespace ejemplo_api.Controllers
{
    [ApiController, Route("permiso"), Tags("Permiso")]
    public class PermisoController : Controller
    {
        // READ.
        [HttpGet, Route("obtener_permisos/{id}"), Authorize]
        public ActionResult Read(int id)
        {
            string storedProcedure = "ObtenerPermisos";

            SqlConnection connection = new SqlConnection(Connection.GetConnection());
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand(storedProcedure, connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@userId", SqlDbType.Int);
                command.Parameters["@userId"].Value = id;
                DataTable data = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(data);

                if (data.Rows.Count > 0)
                {
                    var permisos = JsonConvert.SerializeObject(data);
                    return StatusCode(200, new { success = true, data = JsonConvert.DeserializeObject<List<Permiso>>(permisos) });
                }
                else
                {
                    return StatusCode(200, new { success = false, message = "No se encontró ningún permiso" });
                }
            }
            catch (Exception)
            {
                return StatusCode(500, new { success = false, error = true, message = "Error al obtener los permisos." });
            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }
        }
    }
}
