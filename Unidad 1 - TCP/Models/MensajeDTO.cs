using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unidad_1___TCP.Models
{
    public class MensajeDTO
    {
        public string? Mensaje { get; set; }
        public string Usuario { get; set; } = null!;
        public string? Foto { get; set; }
        public DateTime Fecha { get; set; }
    }
}
