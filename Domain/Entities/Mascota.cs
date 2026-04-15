using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Mascota
    {
        public int IdMascota { get; set; }

        public string Nombre { get; set; }
        public string Especie { get; set; }
        public string Raza { get; set; }
        public string Sexo { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public decimal? Peso { get; set; }
        public string Color { get; set; }

        public int IdCliente { get; set; }
        public bool EstadoMascota { get; set; }

        public DateTime FechaCrea { get; set; }
        public DateTime? FechaEdita { get; set; }

        // 🔗 Relación
        public Cliente Cliente { get; set; }
    }
}
