using ejemplov1.Models;
using ejemplov1.Models.DTOs.Mensaje;
using ejemplov1.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Data;

namespace ejemplov1.Controllers
{
    [ApiController, Route("mensaje"), Tags("Mensaje")]
    public class MensajeController : Controller
    {
        // CREATE.
        [HttpPost, Route("crear_mensaje")]
        public ActionResult Create(Mensaje mensaje)
        {
            string storedProcedure = "CrearMensaje";

            SqlConnection connection = new SqlConnection(Connection.GetConnection());
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand(storedProcedure, connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@cuerpo", SqlDbType.NVarChar);
                command.Parameters["@cuerpo"].Value = mensaje.Cuerpo;
                command.Parameters.Add("@tipo", SqlDbType.NVarChar);
                command.Parameters["@tipo"].Value = mensaje.Tipo;
                command.Parameters.Add("@status", SqlDbType.NVarChar);
                command.Parameters["@status"].Value = mensaje.Status;
                int result = command.ExecuteNonQuery();

                if (result > 0)
                    return StatusCode(200, new { success = true, message = "Mensaje Creado." });
                else
                    return StatusCode(200, new { success = false, message = "No se pudo crear el mensaje." });
            }
            catch (Exception)
            {
                return StatusCode(500, new { success = false, error = true, message = "Error al crear el mensaje." });
            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }
        }

        // READ.
        [HttpGet, Route("obtener_mensajes")]
        public ActionResult Read()
        {
            string storedProcedure = "ObtenerMensajes";
            SqlConnection connection = new SqlConnection(Connection.GetConnection());
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand(storedProcedure, connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                DataTable data = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(data);

                if (data.Rows.Count > 0)
                {
                    var mensajes = JsonConvert.SerializeObject(data);
                    return StatusCode(200, new { success = true, data = JsonConvert.DeserializeObject<List<Models.Mensaje>>(mensajes) });
                }
                else
                {
                    return StatusCode(200, new { success = false, message = "No se encontró ningún mensaje." });
                }
            }
            catch (Exception)
            {
                return StatusCode(500, new { success = false, error = true, message = "Error al obtener los mensajes." });
            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }
        }

        // READ.
        [HttpGet, Route("obtener_mensaje/{id}")]
        public ActionResult Read(int id)
        {
            string storedProcedure = "ObtenerMensaje";

            SqlConnection connection = new SqlConnection(Connection.GetConnection());
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand(storedProcedure, connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@id", SqlDbType.Int);
                command.Parameters["@id"].Value = id;
                DataTable data = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(data);

                if (data.Rows.Count > 0)
                {
                    var mensaje = JsonConvert.SerializeObject(data);
                    return StatusCode(200, new { success = true, data = JsonConvert.DeserializeObject<List<Models.Mensaje>>(mensaje) });
                }
                else
                {
                    return StatusCode(200, new { success = false, message = "No se encontró el mensaje." });
                }
            }
            catch (Exception)
            {
                return StatusCode(500, new { success = false, error = true, message = "Error al editar el mensaje." });
            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }
        }

        // UPDATE.
        [HttpPatch, Route("actualizar_mensaje/{id}")]
        public ActionResult Update(ActualizarMensajeDto mensaje, int id)
        {
            string storedProcedure = "ActualizarMensaje";

            SqlConnection connection = new SqlConnection(Connection.GetConnection());
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand(storedProcedure, connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@id", SqlDbType.Int);
                command.Parameters["@id"].Value = id;
                command.Parameters.Add("@cuerpo", SqlDbType.NVarChar);
                command.Parameters["@cuerpo"].Value = mensaje.Cuerpo;
                command.Parameters.Add("@tipo", SqlDbType.NVarChar);
                command.Parameters["@tipo"].Value = mensaje.Tipo;
                command.Parameters.Add("@status", SqlDbType.NVarChar);
                command.Parameters["@status"].Value = mensaje.Status;
                int result = command.ExecuteNonQuery();

                if (result > 0)
                    return StatusCode(200, new { success = true, message = "El mensaje fue editado." });
                else
                    return StatusCode(200, new { success = false, message = "El mensaje no pudo ser editado." });
            }
            catch (Exception)
            {
                return StatusCode(500, new { success = false, error = true, message = "Error al editar el mensaje." });
            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }
        }

        // DELETE.
        [HttpDelete, Route("eliminar_mensaje/{id}")]
        public ActionResult Delete(int id)
        {
            string storedProcedure = "EliminarMensaje";

            SqlConnection connection = new SqlConnection(Connection.GetConnection());
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand(storedProcedure, connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@id", SqlDbType.Int);
                command.Parameters["@id"].Value = id;
                int result = command.ExecuteNonQuery();

                if (result > 0)
                    return StatusCode(200, new { success = true, message = "Mensaje Eliminado." });
                else
                    return StatusCode(200, new { success = false, message = "No pudo eliminarse el mensaje." });
            }
            catch (Exception)
            {
                return StatusCode(500, new { success = false, error = true, message = "Error al eliminar el mensaje." });
            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }
        }
    }
}
