﻿using ejemplo_api.Models.DTOs.Usuario;
using ejemplov1.Models;
using ejemplov1.Models.DTOs.Usuario;
using ejemplov1.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Data;

namespace ejemplov1.Controllers
{
    [ApiController, Route("usuario"), Tags("Usuario")]
    public class UsuarioController : Controller
    {
        // CREATE.
        [HttpPost, Route("crear_usuario"), Authorize]
        public ActionResult Create (Usuario usuario)
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(usuario.Contrasena);
            string storedProcedure = "CrearUsuario";

            SqlConnection connection = new SqlConnection(Connection.GetConnection());
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand(storedProcedure, connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@name", SqlDbType.NVarChar);
                command.Parameters["@name"].Value = usuario.Nombre;
                command.Parameters.Add("@lastName", SqlDbType.NVarChar);
                command.Parameters["@lastName"].Value = usuario.Apellidos;
                command.Parameters.Add("@email", SqlDbType.NVarChar);
                command.Parameters["@email"].Value = usuario.Correo;
                command.Parameters.Add("@phone", SqlDbType.NVarChar);
                command.Parameters["@phone"].Value = usuario.Telefono;
                command.Parameters.Add("@password", SqlDbType.NVarChar);
                command.Parameters["@password"].Value = hashedPassword;
                command.Parameters.Add("@roleId", SqlDbType.Int);
                command.Parameters["@roleId"].Value = usuario.RolId;
                command.Parameters.Add("@crear", SqlDbType.Bit);
                command.Parameters["@crear"].Value = usuario.Crear;
                command.Parameters.Add("@eliminar", SqlDbType.Bit);
                command.Parameters["@eliminar"].Value = usuario.Eliminar;
                int result = command.ExecuteNonQuery();

                if (result > 0)
                    return StatusCode(200, new { success = true, message = "Usuario creado" });
                else
                    return StatusCode(200, new { success = false, message = "No se creó al usuario." });
            }
            catch (Exception)
            {
                return StatusCode(500, new { success = false, error = true, message = "Error al crear al usuario." });
            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }
        }

        // READ.
        [HttpGet, Route("obtener_usuarios"), Authorize]
        public ActionResult Read ()
        {
            string storedProcedure = "ObtenerUsuarios";
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
                    var usuarios = JsonConvert.SerializeObject(data);
                    return StatusCode(200, new { success = true, data = JsonConvert.DeserializeObject<List<Models.Usuario>>(usuarios) });
                }
                else
                {
                    return StatusCode(200, new { success = false, message = "No se encontró a los usuarios." });
                }
            }
            catch (Exception)
            {
                return StatusCode(500, new { success = false, error = true, message = "Error al obtener los usuarios" });
            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }
        }

        // READ.
        [HttpGet, Route("obtener_usuario/{id}"), Authorize]
        public ActionResult Read (int id)
        {
            string storedProcedure = "ObtenerUsuario";

            SqlConnection connection = new SqlConnection(Connection.GetConnection());
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand(storedProcedure, connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@user", SqlDbType.Int);
                command.Parameters["@user"].Value = id;
                DataTable data = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(data);

                if (data.Rows.Count > 0)
                {
                    var usuario = JsonConvert.SerializeObject(data);
                    return StatusCode(200, new { success = true, data = JsonConvert.DeserializeObject<List<Models.Usuario>>(usuario) });
                }
                else
                {
                    return StatusCode(200, new { success = false, message = "No se encontró al usuario." });
                }
            }
            catch (Exception)
            {
                return StatusCode(500, new { success = false, error = true, message = "Error al obtener al usuario." });
            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }
        }

        // READ.
        [HttpGet, Route("obtener_foto_de_perfil/{id}"), Authorize]
        public ActionResult ReadPP(int id)
        {
            string storedProcedure = "ObtenerFotoDePerfil";

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
                    var usuario = JsonConvert.SerializeObject(data);
                    return StatusCode(200, new { success = true, data = JsonConvert.DeserializeObject<List<ObtenerFotoDePerfilDto>>(usuario) });
                }
                else
                {
                    return StatusCode(200, new { success = false, message = "No se encontró ninguna foto de perfil..." });
                }
            }
            catch (Exception)
            {
                return StatusCode(500, new { success = false, error = true, message = "Error al obtener la foto de perfil..." });
            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }
        }

        // UPDATE.
        [HttpPut, Route("actualizar_usuario/{id}"), Authorize]
        public ActionResult Update (ActualizarUsuarioDto usuario, int id)
        {
            string storedProcedure = "ActualizarUsuario";

            SqlConnection connection = new SqlConnection(Connection.GetConnection());
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand(storedProcedure, connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@user", SqlDbType.Int);
                command.Parameters["@user"].Value = id;
                command.Parameters.Add("@name", SqlDbType.NVarChar);
                command.Parameters["@name"].Value = usuario.Nombre;
                command.Parameters.Add("@lastName", SqlDbType.NVarChar);
                command.Parameters["@lastName"].Value = usuario.Apellidos;
                command.Parameters.Add("@phone", SqlDbType.NVarChar);
                command.Parameters["@phone"].Value = usuario.Telefono;
                command.Parameters.Add("@email", SqlDbType.NVarChar);
                command.Parameters["@email"].Value = usuario.Correo;
                command.Parameters.Add("@crear", SqlDbType.Bit);
                command.Parameters["@crear"].Value = usuario.Crear;
                command.Parameters.Add("@eliminar", SqlDbType.Bit);
                command.Parameters["@eliminar"].Value = usuario.Eliminar;
                int result = command.ExecuteNonQuery();

                if (result > 0)
                    return StatusCode(200, new { success = true, message = "Los datos del usuario fueron actualizados." });
                else
                    return StatusCode(200, new { success = false, message = "Los datos del usuario no fueron actualizados" });
            }
            catch (Exception)
            {
                connection.RetrieveInternalInfo();
                return StatusCode(500, new { success = false, error = true, message = "Error al actualizar los datos del usuario."});
            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }
        }

        // UPDATE.
        [HttpPut, Route("actualizar_contrasena/{id}"), Authorize]
        public ActionResult UpdatePass (ActualizarContrasenaDto usuario, int id)
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(usuario.Contrasena);
            string storedProcedure = "ActualizarContrasena";

            SqlConnection connection = new SqlConnection(Connection.GetConnection());
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand(storedProcedure, connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@user", SqlDbType.Int);
                command.Parameters["@user"].Value = id;
                command.Parameters.Add("@password", SqlDbType.NVarChar);
                command.Parameters["@password"].Value = hashedPassword;
                int result = command.ExecuteNonQuery();

                if (result > 0)
                    return StatusCode(200, new { success = true, message = "La contraseña ha sido actualizada." });
                else
                    return StatusCode(200, new { success = false, message = "La contraseña no fue actualizada" });
            }
            catch (Exception)
            {
                return StatusCode(500, new { success = false, error = true, message = "Error al actualizar la contraseña." });
            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }
        }

        // UPDATE.
        [HttpPut, Route("actualizar_foto_de_perfil/{id}"), Authorize]
        public ActionResult UpdatePP(ActualizarFotoDePerfilDto usuario, int id)
        {
            string storedProcedure = "ActualizarFotoDePerfil";

            SqlConnection connection = new SqlConnection(Connection.GetConnection());
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand(storedProcedure, connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@id", SqlDbType.Int);
                command.Parameters["@id"].Value = id;
                command.Parameters.Add("@imageUrl", SqlDbType.NVarChar);
                command.Parameters["@imageUrl"].Value = usuario.ImageUrl;
                int result = command.ExecuteNonQuery();

                if (result > 0)
                    return StatusCode(200, new { success = true, message = "La foto de perfil ha sido actualizada." });
                else
                    return StatusCode(200, new { success = false, message = "La foto de perfil no fue actualizada" });
            }
            catch (Exception)
            {
                return StatusCode(500, new { success = false, error = true, message = "Error al actualizar la foto de perfil." });
            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }
        }

        // DELETE.
        [HttpDelete, Route("eliminar_usuario/{id}"), Authorize]
        public ActionResult Delete (int id)
        {
            string storedProcedure = "EliminarUsuario";

            SqlConnection connection = new SqlConnection(Connection.GetConnection());
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand(storedProcedure, connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@user", SqlDbType.Int);
                command.Parameters["@user"].Value = id;
                int result = command.ExecuteNonQuery();

                if (result > 0)
                    return StatusCode(200, new { success = true, message = "Usuario Eliminado." });
                else
                    return StatusCode(200, new { success = false, message = "No se elimino al usuario." });
            }
            catch (Exception)
            {
                return StatusCode(500, new { success = false, error = true, message = "Error al eliminar al usuario." });
            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }
        }
    }
}
