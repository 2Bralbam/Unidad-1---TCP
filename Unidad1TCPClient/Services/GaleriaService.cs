using System.Net.Sockets;
using System.Net;
using System.Windows;
using System.IO;
using Microsoft.Win32;
using DevExpress.Utils.CommonDialogs.Internal;

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
        public void SubirImagen()
        {
            try
            {
                if (VerificarConexion())
                {
                    OpenFileDialog openFileDialog = new();
                    openFileDialog.Filter = "Archivos de imagen|*.jpg;*.jpeg;*.png;*.gif|Todos los archivos|*.*";

                    if (openFileDialog?.ShowDialog()!=null)
                    {
                        string selectedImagePath = openFileDialog.FileName;
                        // Aquí puedes procesar la fotografía seleccionada, como mostrarla en la interfaz o enviarla al servidor
                    }
                    string imagePath = @"C:\ruta\de\tu\imagen.jpg";
                    if (File.Exists(imagePath))
                    {
                        byte[] imageBytes = File.ReadAllBytes(imagePath);
                        string base64String = Convert.ToBase64String(imageBytes);

                        // Establecer conexión TCP con el servidor
                        TcpClient client = new("ip_servidor", 2000);
                        NetworkStream stream = client.GetStream();

                        // Enviar la imagen codificada al servidor
                        byte[] data = System.Text.Encoding.ASCII.GetBytes(base64String);
                        stream.Write(data, 0, data.Length);
                        Console.WriteLine("Imagen enviada al servidor correctamente.");
                        stream.Close();
                        client.Close();
                    }
                    else
                    {
                        Console.WriteLine("La imagen no fue encontrada en la ruta especificada.");
                    }
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
        #region Utilidades Varias
        static string ConvertirImagenBase64(string imagePath)
        {
            byte[] imageBytes = File.ReadAllBytes(imagePath);
            string base64String = Convert.ToBase64String(imageBytes);
            return base64String;
        }
        bool VerificarConexion()
        {
            return client.GetStream()!=null;
        }
        #endregion
    }
}