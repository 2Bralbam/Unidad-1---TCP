using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Unidad_1___TCP.Models;

namespace Unidad_1___TCP.Services
{
    public class TCPServer
    {
        TcpListener server = null!;
        List<TcpClient> Clientes = new();
        public event EventHandler<MensajeDTO>? MensajeRecibido;
      
        public TCPServer()
        {
            
        }
    }
}
