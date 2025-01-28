namespace ejemplov1.Models
{
    public class Mensaje
    {
        public int Id { get; set; }
        public string Cuerpo { get; set; }
        public string Tipo { get; set; }
        public string Status { get; set; }
        public DateTime Creado { get; set; }
    }
}
