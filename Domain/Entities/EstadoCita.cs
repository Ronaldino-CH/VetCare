using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class EstadoCita
    {
        public int IdEstadoCita { get; set; }
        public string? NombreEstado { get; set; }
        public string? Codigo { get; set; } 
    }
}
