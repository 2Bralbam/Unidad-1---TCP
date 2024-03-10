using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using Unidad_1___TCP.Models;

namespace Unidad_1___TCP.Services
{
    public class TCPServer
    {
        TcpListener server = null!;
        List<TcpClient> Clientes = new();
        public bool ServerActivo { get; set; }
        public event EventHandler<MensajeDTO>? MensajeRecibido;
        public void IniciarServer()
        {
            server = new(new IPEndPoint(IPAddress.Any, 9000));
            server.Start();
            ServerActivo = true;
            new Thread(Escuchar) { IsBackground = true }.Start();
        }
        void Escuchar()
        {
            while (ServerActivo)
            {
                try
                {
                    TcpClient cliente = server.AcceptTcpClient();
                    Clientes.Add(cliente);
                    new Thread(() => IniciarCanal(cliente)) { IsBackground = true }.Start();
                }
                catch
                {

                }
            }
            DetenerServer();
        }
        void IniciarCanal(TcpClient Cliente)
        {
            while (Cliente.Connected)
            {
                try
                {
                    NetworkStream stream = Cliente.GetStream();
                    while (Cliente.Available == 0)
                    {
                        Thread.Sleep(500);
                    }
                    byte[] buffer = new byte[Cliente.Available];
                    stream.Read(buffer, 0, buffer.Length);
                    string mensaje = Encoding.UTF8.GetString(buffer);
                    MensajeDTO? mensajeDTO = JsonSerializer.Deserialize<MensajeDTO>(mensaje);
                    if (mensajeDTO != null)
                    {
                        RebotarMensaje(buffer, Cliente);
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            MensajeRecibido?.Invoke(this, mensajeDTO);
                        }); 
                    }
                    
                }
                catch
                {
                    //Control de errores

                }
            }
        }
        public void DetenerServer()
        {
            ServerActivo = false;
            server.Stop();
            Parallel.ForEach(Clientes, c =>
            {
                c.Close();
            });
            Clientes.Clear();
        }
        void RebotarMensaje(byte[] Mensaje, TcpClient Client)
        {
            foreach (TcpClient c in Clientes)
            {
                Parallel.ForEach(Clientes, c =>
                {
                    if (c != Client && c.Connected)
                    {
                        NetworkStream ns = c.GetStream();
                        ns.Write(Mensaje, 0, Mensaje.Length);
                        ns.Flush();
                    }
                    else if(!c.Connected)
                    {
                        try
                        {
                            c.Close();
                            Clientes.Remove(c);
                        }
                        catch
                        { 
                            //Control de errores
                        }  
                    }
                });
            }
        }
    }
}
