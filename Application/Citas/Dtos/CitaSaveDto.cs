namespace Application.Citas.Dtos
{
    public class CitaSaveDto
    {
        public string Motivo { get; set; } = string.Empty;
        public string? Observaciones { get; set; }

        public int IdMascota { get; set; }
        public int IdVeterinario { get; set; }
        public int IdEstadoCita { get; set; }

        public DateTime FechaHora { get; set; }
    }
}
