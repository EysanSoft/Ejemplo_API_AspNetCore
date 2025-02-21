using Newtonsoft.Json;

namespace ejemplov1.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }
        public string Contrasena { get; set; }
        public int RolId { get; set; }
        public bool Crear { get; set; }
        public bool Eliminar { get; set; }
        public DateTime Creado { get; set; }
    }
}
