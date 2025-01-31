using ejemplov1.Models;
using ejemplov1.Models.DTOs.Cliente;
using ejemplov1.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Data;

namespace ejemplov1.Controllers
{
    [ApiController, Route("cliente"), Tags("Cliente")]
    public class ClienteController : Controller
    {
        // CREATE.
        [HttpPost, Route("crear_cliente")]
        public ActionResult Create(Cliente cliente)
        {
            string storedProcedure = "CrearCliente";

            SqlConnection connection = new SqlConnection(Connection.GetConnection());
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand(storedProcedure, connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@nombre", SqlDbType.NVarChar);
                command.Parameters["@nombre"].Value = cliente.Nombre;
                command.Parameters.Add("@apellidos", SqlDbType.NVarChar);
                command.Parameters["@apellidos"].Value = cliente.Apellidos;
                command.Parameters.Add("@correo", SqlDbType.NVarChar);
                command.Parameters["@correo"].Value = cliente.Correo;
                command.Parameters.Add("@telefono", SqlDbType.NVarChar);
                command.Parameters["@telefono"].Value = cliente.Telefono;
                int result = command.ExecuteNonQuery();

                if (result > 0)
                    return StatusCode(200, new { success = true, message = "Cliente creado" });
                else
                    return StatusCode(200, new { success = false, message = "No se creó el cliente." });
            }
            catch (Exception)
            {
                return StatusCode(500, new { success = false, error = true, message = "Error al crear al cliente." });
            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }
        }

        // READ.
        [HttpGet, Route("obtener_clientes")]
        public ActionResult Read()
        {
            string storedProcedure = "ObtenerClientes";
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
                    var clientes = JsonConvert.SerializeObject(data);
                    return StatusCode(200, new { success = true, data = JsonConvert.DeserializeObject<List<Models.Cliente>>(clientes) });
                }
                else
                {
                    return StatusCode(200, new { success = false, message = "No se encontró a los clientes." });
                }
            }
            catch (Exception)
            {
                return StatusCode(500, new { success = false, error = true, message = "Error al obtener a los clientes." });
            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }
        }

        // READ.
        [HttpGet, Route("obtener_cliente/{id}")]
        public ActionResult Read(int id)
        {
            string storedProcedure = "ObtenerCliente";

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
                    var cliente = JsonConvert.SerializeObject(data);
                    return StatusCode(200, new { success = true, data = JsonConvert.DeserializeObject<List<Models.Cliente>>(cliente) });
                }
                else
                {
                    return StatusCode(200, new { success = false, message = "No se encontró al cliente." });
                }
            }
            catch (Exception)
            {
                return StatusCode(500, new { success = false, error = true, message = "Error al obtener al cliente." });
            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }
        }

        // UPDATE.
        [HttpPut, Route("actualizar_cliente/{id}")]
        public ActionResult Update(ActualizarClienteDto cliente, int id)
        {
            string storedProcedure = "ActualizarCliente";

            SqlConnection connection = new SqlConnection(Connection.GetConnection());
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand(storedProcedure, connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@id", SqlDbType.Int);
                command.Parameters["@id"].Value = id;
                command.Parameters.Add("@nombre", SqlDbType.NVarChar);
                command.Parameters["@nombre"].Value = cliente.Nombre;
                command.Parameters.Add("@apellidos", SqlDbType.NVarChar);
                command.Parameters["@apellidos"].Value = cliente.Apellidos;
                command.Parameters.Add("@correo", SqlDbType.NVarChar);
                command.Parameters["@correo"].Value = cliente.Correo;
                command.Parameters.Add("@telefono", SqlDbType.NVarChar);
                command.Parameters["@telefono"].Value = cliente.Telefono;
                int result = command.ExecuteNonQuery();

                if (result > 0)
                    return StatusCode(200, new { success = true, message = "Los datos del cliente fueron actualizados." });
                else
                    return StatusCode(200, new { success = false, message = "Los datos del cliente no fueron actualizados" });
            }
            catch (Exception)
            {
                return StatusCode(500, new { success = false, error = true, message = "Error al actualizar los datos del cliente." });
            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }
        }

        // DELETE.
        [HttpDelete, Route("eliminar_cliente/{id}")]
        public ActionResult Delete(int id)
        {
            string storedProcedure = "EliminarCliente";

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
                    return StatusCode(200, new { success = true, message = "Cliente Eliminado." });
                else
                    return StatusCode(200, new { success = false, message = "No se elimino el cliente." });
            }
            catch (Exception)
            {
                return StatusCode(500, new { success = false, error = true, message = "Error al eliminar el cliente." });
            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }
        }

    }
}
