using ejemplo_api.Models.DTOs.Mensaje;
using ejemplov1.Models;
using ejemplov1.Resources;
using Microsoft.AspNetCore.Authorization;
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
        [HttpPost, Route("crear_mensaje"), Authorize]
        public ActionResult Create(CrearMensajeDto mensaje)
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
                command.Parameters.Add("@contacto", SqlDbType.NVarChar);
                command.Parameters["@contacto"].Value = mensaje.Contacto;
                command.Parameters.Add("@tipo", SqlDbType.NVarChar);
                command.Parameters["@tipo"].Value = mensaje.Tipo;
                command.Parameters.Add("@status", SqlDbType.NVarChar);
                command.Parameters["@status"].Value = mensaje.Status;
                command.Parameters.Add("@clienteId", SqlDbType.Int);
                command.Parameters["@clienteId"].Value = mensaje.ClienteId;
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
        [HttpGet, Route("obtener_mensajes"), Authorize]
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
                    return StatusCode(200, new { success = true, data = JsonConvert.DeserializeObject<List<Mensaje>>(mensajes) });
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
        [HttpGet, Route("obtener_tipos"), Authorize]
        public ActionResult ReadTipos()
        {
            string storedProcedure = "ObtenerTipos";
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
                    var tipos_mensajes = JsonConvert.SerializeObject(data);
                    return StatusCode(200, new { success = true, data = JsonConvert.DeserializeObject<List<ObtenerTiposDto>>(tipos_mensajes) });
                }
                else
                {
                    return StatusCode(200, new { success = false, message = "No se encontró ningún registro." });
                }
            }
            catch (Exception)
            {
                return StatusCode(500, new { success = false, error = true, message = "Error al obtener los registros." });
            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }
        }

        // READ.
        [HttpGet, Route("obtener_conteo_semanal"), Authorize]
        public ActionResult ReadSemanal(RangoEntreFechas fecha)
        {
            string storedProcedure = "ObtenerConteoSemanalMensajes";
            SqlConnection connection = new SqlConnection(Connection.GetConnection());
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand(storedProcedure, connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@fechaInicial", SqlDbType.NVarChar);
                command.Parameters["@fechaInicial"].Value = fecha.Inicial;
                command.Parameters.Add("@fechaFinal", SqlDbType.NVarChar);
                command.Parameters["@fechaFinal"].Value = fecha.Final;
                DataTable data = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(data);

                if (data.Rows.Count > 0)
                {
                    var mensajes_semanales = JsonConvert.SerializeObject(data);
                    return StatusCode(200, new { success = true, data = JsonConvert.DeserializeObject<List<ObtenerTiposDto>>(mensajes_semanales) });
                }
                else
                {
                    return StatusCode(200, new { success = false, message = "No se encontró ningún registro. Espere a que los clientes registren mensajes durante el transcurso de la semana." });
                }
            }
            catch (Exception)
            {
                return StatusCode(500, new { success = false, error = true, message = "Error al obtener los registros." });
            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }
        }

        // DELETE.
        [HttpDelete, Route("eliminar_mensaje/{id}"), Authorize]
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
