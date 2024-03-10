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
        /* 
           Para hacer una conexion TCP Se necesita de un cliente TCP 
           este nos proveera de metodos por defecto necesarios para hacer
           la conexion 
        */
        private TcpClient client = new();

        // Ya que quiero saber desde el viewmodel si el cliente se conecto correctamente utilizo un booleano
        public bool Conectar(IPAddress ip,int puerto)
        {
            try
            {
                /** Esta validacion es necesaria ya que el usuario puede presionar
                 *  2 o más veces el boton para conectar el cliente al servidor
                 */
                
                CrearClienteTCP();
                if (!client.Connected)
                {
                    IPEndPoint ipe = new(ip, puerto);
                    client.Connect(ipe);
                }
            }
            //Manejo de errores
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return client.Connected;
        }
        public bool Desconectar()
        {
            //Intentara desconectar el servidor
            try
            {
                client.Close();
            }
            //Manejo de errores
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            //Si el cliente no esta conectado regresara true, en caso contrario regresara false
            return !client.Connected;
        }
        public bool CompartirImagen(string rutaImagen,IPAddress ip,int puerto)
        {
            try
            {
                if (client.Connected)
                {
                    if (File.Exists(rutaImagen))
                    {
                        // obtenemos los bytes de la imagen seleccionada
                        byte[] imageBytes = File.ReadAllBytes(rutaImagen);
                        // Convertimos los bytes de la imagen recibida a base 64
                        string base64String = Convert.ToBase64String(imageBytes);
                        EnviarImagen(base64String);
                        //si todo fue bien
                        return true;
                    }
                }
            }
            //Manejo de errores
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            //si no se logro compartir la imagen regresara un false
            return false;
        }
        public void EliminarImagen(string Imagen, IPAddress ip, int puerto)
        {
            try
            {
                if (client.Connected)
                {
                    var BytesImagen = File.ReadAllBytes(Imagen);
                    string base64String = Convert.ToBase64String(BytesImagen);
                    NetworkStream stream = client.GetStream();
                    /** Se debe identificar de alguna manera que metodo se utilizara al recibir la imagen,
                     *  al deserealizar el texto, mostrara la accion que se desea realizar (eliminar) y la imagen que
                     *  se eliminara (en este caso)
                     */
                    byte[] data = Encoding.UTF8.GetBytes("eliminar " + base64String);
                    stream.Write(data, 0, data.Length);
                }
            }
            //Manejo de errores
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #region Metodos Necesarios
        public void CrearClienteTCP()
        {
            client ??= new();
        }

        private void EnviarImagen(string base64String)
        {
            try
            {
                NetworkStream stream = client.GetStream();
                // Enviar la imagen codificada al servidor
                byte[] data = Encoding.UTF8.GetBytes(base64String);
                stream.Write(data, 0, data.Length);
                /** El stream se queda abierto en caso de que se quiera mandar mas informacion,
                 *  en este caso las imagenes ya que se esta utilizando una conexion por el 
                 *  protocolo TCP no es necesario cerrar la conexion
                 */
            }
            //Manejo de errores
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion
    }
}