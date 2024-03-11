using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace Unidad_1___TCP.Models
{
    public class Publicacion
    {
        public List<Comentario>? Comentarios { get; set; } = new();
        public int IdPublicacion { get; set; }
        public MensajeDTO Mensaje { get; set; } = null!;
        public ImageSource FotoSrc { get; set; }
    }
}
