using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Citas.Dtos
{
    public class CitaResponseDto
    {
        public int IdCita { get; set; }

        public string Motivo { get; set; } = string.Empty;
        public string? Observaciones { get; set; }

        public int IdMascota { get; set; }
        public int IdVeterinario { get; set; }
        public int IdEstadoCita { get; set; }

        public DateTime FechaHora { get; set; }

        public DateTime FechaCrea { get; set; }
        public DateTime? FechaEdita { get; set; }
    }
}
