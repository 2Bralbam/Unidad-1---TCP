using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Net.Http.Json;


namespace Unidad1TCPClient.Services
{
    public class GaleriaService
    {
        /** Para hacer una conexion TCP Se necesita de un cliente TCP 
         *  este nos proveera de metodos por defecto necesarios para hacer
         *  la conexion
         */
        TcpClient client = null!;
        private readonly int puerto = 55555;
        public void Conectar(IPAddress ip)
        {
            /** Si el cliente no esta conectado lo conectamos,
             *  Esta validacion es necesaria ya que el usuario puede presionar
             *  2 o más veces el boton para conectar el cliente al servidor
             */
            if (!client.Connected)
            {
                IPEndPoint ipe = new(ip, puerto);
                client = new();
                client.Connect(ipe);
            }
        }
        public void Desconectar()
        {
           client.Close();
        }
        public void CargarImagenes()
        {

        }
        public void SubirImagen()
        {

        }
        public void EliminarImagen()
        {

        }
    }
}
