using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class PaginationRequest
    {
        public int? Page { get; set; } = 1;
        public int? Take { get; set; } = 6;

        /// <summary>
        /// Formato: campo.asc o campo.desc
        /// Ejemplo: createAt.desc
        /// </summary>
        public string? Sort { get; set; } = "FechaCrea.desc";

        /// <summary>
        /// Filtros dinámicos (opcional)
        /// </summary>
        public string[]? Filters { get; set; }
    }
}
