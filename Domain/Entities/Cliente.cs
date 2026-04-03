namespace Domain.Entities
{
    public class Cliente
    {
        public int IdCliente { get; set; }
        public string Nombres { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public string Documento { get; set; } = string.Empty;
        public string? Telefono { get; set; }
        public string? Correo { get; set; }
        public string? Direccion { get; set; }
        public bool EstadoCliente { get; set; }
        public DateTime FechaCrea { get; set; }
        public DateTime? FechaEdita { get; set; }
    }
}
