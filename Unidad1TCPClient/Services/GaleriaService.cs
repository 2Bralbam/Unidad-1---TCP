using System.Net.Sockets;
using System.Net;
using System.Windows;
using System.IO;
using Microsoft.Win32;
using DevExpress.Utils.CommonDialogs.Internal;
using System.Text;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Unidad1TCPClient.Models;
using System.Text.Json;
using Newtonsoft.Json;

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
        public string Equipo { get; set; } = "";
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
                    Equipo = Dns.GetHostName();
                   
                    client.Connect(ipe);

                    //Mensaje Hello
                    var msg = new MensajeDTO
                    {
                        Fecha = DateTime.Now,
                        Foto = "**HELLO",
                        Usuario = Equipo
                    };
                    EnviarMensaje(msg);
                    // El cliente no recibe respuesta nunca asi que no es necesario implmentar un metodo para recibir respuesta
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
                //Mensaje bye
                var msg = new MensajeDTO
                {
                    Fecha = DateTime.Now,
                    Foto = "**BYE",
                    Usuario = Equipo
                };
                EnviarMensaje(msg);
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
        public bool CompartirImagen(MensajeDTO dto)
        {
            try
            {
                if (client.Connected)
                {
                    if (File.Exists(dto.Foto))
                    {
                        //serializamos la foto
                        //string json = JsonConvert.SerializeObject(dto.Foto);
                        byte[] FotoBase64 = SerializarFoto(dto.Foto);
                        // obtenemos los bytes de la imagen seleccionada
                        //byte[] imageBytes = File.ReadAllBytes(json);
                        // Convertimos los bytes de la imagen recibida a base 64
                        if(FotoBase64 != null && FotoBase64.Length > 0)
                        {
                                string? base64String = FotoBase64.ToString();
                                EnviarImagen(base64String);
                                //si todo fue bien
                                return true;
                        }
                        else
                        {
                            return false;
                        }
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
        private static byte[] SerializarFoto(string PathFoto)
        {
            byte[] imageBytes = File.ReadAllBytes(PathFoto);
            return imageBytes;
        }
        public void EliminarImagen(MensajeDTO dto)
        {
            try
            {
                if (client.Connected)
                {
                    //Serializamos el objeto
                    var json = JsonConvert.SerializeObject(dto);
                    //Obtenemos los Bytes
                    var BytesImagen = File.ReadAllBytes(json);
                    //Convertimos a base64
                    string base64String = Convert.ToBase64String(BytesImagen);
                    //Obtenemos la red
                    NetworkStream stream = client.GetStream();

                    /** Se debe identificar de alguna manera que metodo se utilizara al recibir la imagen,
                     *  al deserealizar el texto, mostrara la accion que se desea realizar (eliminar) y la imagen que
                     *  se eliminara (en este caso) 
                     *  al serializar se puede utilizar un substring para obtener la data saltando el mensaje **Eliminar
                     */
                    byte[] data = Encoding.UTF8.GetBytes("**Eliminar " + base64String);
                    //Enviamos los datos
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
        private void EnviarImagen(string? base64String)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(base64String))
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
            }
            //Manejo de errores
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void EnviarMensaje(MensajeDTO m)
        {
            if (m!=null && !string.IsNullOrWhiteSpace(m.Usuario))
            {
                var json = System.Text.Json.JsonSerializer.Serialize(m);
                byte[] buffer = Encoding.UTF8.GetBytes(json);
                var ns = client.GetStream();
                ns.Write(buffer, 0, buffer.Length);
                ns.Flush();
            }
        }
        #endregion
    }
}