namespace ejemplo_api.Models.DTOs.Mensaje
{
    public class CrearMensajeDTO
    {
        public int Id { get; set; }
        public string Cuerpo { get; set; }
        public string Tipo { get; set; }
        public string Status { get; set; }
        public int ClienteId { get; set; }
        public DateTime Creado { get; set; }
    }
}
