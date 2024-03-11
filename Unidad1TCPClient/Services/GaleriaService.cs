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
using DevExpress.Data.Helpers;

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
                        Mensaje = "**HELLO",
                        Fecha = DateTime.Now,
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
                    Mensaje = "**BYE",
                    Fecha = DateTime.Now,
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
                        dto.Foto = ConvertirABase64(dto.Foto);
                        EnviarMensaje(dto);
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
        private static string ConvertirABase64(string imagen)
        {
            return Convert.ToBase64String(File.ReadAllBytes(imagen));
        }
        public bool EliminarImagen(MensajeDTO dto)
        {
            try
            {
                if (client.Connected)
                {
                    dto.Foto = ConvertirABase64(dto.Foto);
                    EnviarMensaje(dto);
                    return true;
                }
            }
            //Manejo de errores
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return false;
        }
        #region Metodos Necesarios
        public void CrearClienteTCP()
        {
            client ??= new();
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