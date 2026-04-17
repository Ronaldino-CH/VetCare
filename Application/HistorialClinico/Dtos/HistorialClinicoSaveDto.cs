namespace Application.HistorialClinico.Dtos
{
    public class HistorialClinicoSaveDto
    {
        public int IdMascota { get; set; }
        public string Diagnostico { get; set; } = string.Empty;
        public string Tratamiento { get; set; } = string.Empty;
        public string? Observaciones { get; set; }
        public int IdVeterinario { get; set; }
    }
}