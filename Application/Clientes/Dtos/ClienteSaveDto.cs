using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Clientes.Dtos
{
    /// <summary>
    /// DTO usado para crear o editar clientes.
    /// </summary>
    public class ClienteSaveDto
    {
        public string Nombres { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public string Documento { get; set; } = string.Empty;
        public string? Telefono { get; set; }
        public string? Correo { get; set; }
        public string? Direccion { get; set; }
    }
}
