using System.Net.Sockets;
using System.Net;
using System.Windows;
using System.IO;
using Microsoft.Win32;
using DevExpress.Utils.CommonDialogs.Internal;
using System.Text;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Unidad1TCPClient.Services
{
    public class GaleriaService
    {
        /** Para hacer una conexion TCP Se necesita de un cliente TCP 
         *  este nos proveera de metodos por defecto necesarios para hacer
         *  la conexion
         */
        TcpClient client = new();
        public bool Conectar(IPAddress ip,int puerto)
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
                    if (client.Connected)
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return false;
        }
        public bool Desconectar()
        {
            try
            {
                client.Close();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return false;
        }

        public bool CompartirImagen(string rutaImagen,IPAddress ip,int puerto)
        {
            try
            {
                if (VerificarConexion())
                {
                    if (File.Exists(rutaImagen))
                    {
                        byte[] imageBytes = File.ReadAllBytes(rutaImagen);
                        string base64String = Convert.ToBase64String(imageBytes);

                        // Establecer conexión TCP con el servidor
                        Conectar(ip,puerto);
                        EnviarImagen(base64String);
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return false;
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
        #region Metodos Necesarios
        private void EnviarImagen(string base64String)
        {
            try
            {
                NetworkStream stream = client.GetStream();
                // Enviar la imagen codificada al servidor
                byte[] data = Encoding.UTF8.GetBytes(base64String);
                stream.Write(data, 0, data.Length);
                stream.Close();
                client.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        bool VerificarConexion()
        {
            return client.GetStream()!=null;
        }
        #endregion
    }
}