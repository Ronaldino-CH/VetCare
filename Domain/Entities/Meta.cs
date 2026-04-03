using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Meta
    {
        public int? Page { get; set; }
        public int? TotalCount { get; set; }
        public int? TotalPages { get; set; }
        public int CurrentPage { get; set; }
    }
}
