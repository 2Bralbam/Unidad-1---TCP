using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unidad1TCPClient.Models
{
    public class MensajeDTO
    {
        public string Usuario { get; set; } = null!;
        public string Foto { get; set; } = "nada";
        public DateTime Fecha { get; set; }
    }
}
