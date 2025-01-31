using System.ComponentModel.DataAnnotations;

namespace ejemplo_api.Models.DTOs.Usuario
{
    public class AuthUsuarioDto
    {
        [Required(ErrorMessage = "Ingresa el correo")]
        public string Correo { get; set; }
        [Required(ErrorMessage = "Ingresa la contraseña")]
        public string Contrasena { get; set; }
    }
}
