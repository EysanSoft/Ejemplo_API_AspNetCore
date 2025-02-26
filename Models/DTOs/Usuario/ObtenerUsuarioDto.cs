using System.ComponentModel.DataAnnotations;

namespace ejemplo_api.Models.DTOs.Usuario
{
    public class ObtenerUsuarioDto
    {
        [Required(ErrorMessage = "Ingresa el correo")]
        public string Correo { get; set; }
    }
}
