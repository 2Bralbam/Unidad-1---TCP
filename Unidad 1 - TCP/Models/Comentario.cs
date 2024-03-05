using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unidad_1___TCP.Models
{
    public class Comentario
    {
        public string Usuario { get; set; } = null!;
        public string Texto { get; set; } = null!;
        public DateTime Fecha { get; set; }
    }
}
