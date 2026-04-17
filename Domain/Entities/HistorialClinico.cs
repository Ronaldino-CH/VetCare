namespace Domain.Entities
{
    public class HistorialClinico
    {
        public int IdHistorial { get; set; }
        public int IdMascota { get; set; }
        public string Diagnostico { get; set; } = string.Empty;
        public string Tratamiento { get; set; } = string.Empty;
        public string? Observaciones { get; set; }
        public int IdVeterinario { get; set; }
        public DateTime FechaCrea { get; set; }
        public DateTime? FechaEdita { get; set; }

        public Mascota Mascota { get; set; } = null!;
        public Usuario Veterinario { get; set; } = null!;
    }
}