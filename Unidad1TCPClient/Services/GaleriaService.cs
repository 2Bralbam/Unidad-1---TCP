using System.Net.Sockets;
using System.Net;
using System.Windows;
using System.Data;
using Xamarin.Essentials;
using System.IO;
using System.Xaml;

namespace Unidad1TCPClient.Services
{
    public class GaleriaService
    {
        /** Para hacer una conexion TCP Se necesita de un cliente TCP 
         *  este nos proveera de metodos por defecto necesarios para hacer
         *  la conexion
         */
        TcpClient client = new();
        private readonly int puerto = 55555;
        public void Conectar(IPAddress ip)
        {
            try
            {
                /** Esta validacion es necesaria ya que el usuario puede presionar
                 *  2 o más veces el boton para conectar el cliente al servidor
                 */
                if (!client.Connected)
                {
                    IPEndPoint ipe = new(ip, puerto);
                    client = new();
                    client.Connect(ipe);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Desconectar()
        {
           client.Close();
        }
        public void SubirImagen()
        {
            try
            {
                if (VerificarConexion())
                {
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void CargarImagenes()
        {
            try
            {
                if (VerificarConexion())
                {

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void EliminarImagen()
        {
            try
            {
                if (VerificarConexion())
                {

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        string ConvertirImagenBase64(string imagePath)
        {
            byte[] imageBytes = File.ReadAllBytes(imagePath);
            string base64String = Convert.ToBase64String(imageBytes);

            return base64String;
        }
        bool VerificarConexion()
        {
            return client.GetStream()!=null;
        }
    }
}