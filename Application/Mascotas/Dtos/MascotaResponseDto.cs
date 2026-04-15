using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mascotas.Dtos
{
    public class MascotaResponseDto
    {
        public int IdMascota { get; set; }

        public string Nombre { get; set; } = string.Empty;
        public string Especie { get; set; } = string.Empty;
        public string? Raza { get; set; }
        public string? Sexo { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public decimal? Peso { get; set; }
        public string? Color { get; set; }

        public int IdCliente { get; set; }
        public bool EstadoMascota { get; set; }

        public DateTime FechaCrea { get; set; }
        public DateTime? FechaEdita { get; set; }
    }
}
