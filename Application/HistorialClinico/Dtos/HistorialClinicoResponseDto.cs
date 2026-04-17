namespace Application.HistorialClinico.Dtos
{
    public class HistorialClinicoResponseDto
    {
        public int IdHistorial { get; set; }
        public int IdMascota { get; set; }
        public string Diagnostico { get; set; } = string.Empty;
        public string Tratamiento { get; set; } = string.Empty;
        public string? Observaciones { get; set; }
        public int IdVeterinario { get; set; }
        public DateTime FechaCrea { get; set; }
        public DateTime? FechaEdita { get; set; }
    }
}