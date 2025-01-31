/*
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
*/

namespace ejemplov1.Models.DTOs.Usuario
{
    public class ActualizarUsuarioDto
    {
        // [Required(ErrorMessage = "Ingresa el nombre."), MinLength(3, ErrorMessage = "Mínimo 3 caracteres"), MaxLength(20, ErrorMessage = "Maximo 20 caracteres")]
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }
    }
}
