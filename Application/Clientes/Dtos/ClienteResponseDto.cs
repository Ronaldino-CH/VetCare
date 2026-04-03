using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Clientes.Dtos
{
    public class ClienteResponseDto
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
    }
}
