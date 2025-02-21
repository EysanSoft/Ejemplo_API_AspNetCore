namespace ejemplo_api.Models
{
    public class Permiso
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public bool Crear { get; set; }
        public bool Eliminar { get; set; }
    }
}
