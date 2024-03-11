using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Unidad_1___TCP.Models;
using Unidad_1___TCP.Services;

namespace Unidad_1___TCP.ViewModels
{
    public class HomeViewModel:INotifyPropertyChanged
    {
        TCPServer server = new TCPServer();
        public ObservableCollection<string> Usuarios { get; set; } = new();
        public ObservableCollection<Publicacion> Publicaciones { get; set; } = new();
        public string EventoTipo { get; set; } = "Evento";
        public bool ServidorCorriendo { 
            get
            {
                return _serverActivo;
            }
            set
            {
                _serverActivo = value;
                OnPropertyChanged();
            }
        }
        private bool _serverActivo { get; set; }
        public ICommand IniciarServer { get; set; }
        public ICommand DetenerServer { get; set; }
        public static string IP
        {
            get
            {
                var Direcciones = Dns.GetHostAddresses(Dns.GetHostName());
                if (Direcciones != null)
                {
                    return string.Join("/\n", Direcciones.Where(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        .Select(x => x.ToString()));
                }
                else
                {
                    return "No hay direcciones";
                }
            } 
        }
        public HomeViewModel()
        {
            server.MensajeRecibido += RecibiendoMensaje;
            IniciarServer = new RelayCommand(() =>
            {
                if (!server.ServerActivo)
                {
                    server.IniciarServer();
                    CheckServerStatus();
                }
                
            });
            DetenerServer = new RelayCommand(() =>
            {
                server.DetenerServer();
                CheckServerStatus();
            });
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void RecibiendoMensaje(object? sender, MensajeDTO e)
        {
            Publicacion P = new Publicacion() { 
                IdPublicacion = Publicaciones.LastOrDefault() != null ? Publicaciones.LastOrDefault().IdPublicacion+1:1, 
                Mensaje = e,
                Comentarios = new() 
            };
            if(e.Mensaje == "**HELLO")
            {
                Usuarios.Add(e.Usuario);
            }
            else if(e.Mensaje == "**BYE")
            {
                Usuarios.Remove(e.Usuario);
            }
            else if(e.Mensaje == "**DELETE")
            {
                try
                {
                    
                    Publicacion? p = Publicaciones.Where(x => x.Mensaje.Foto == e.Foto).FirstOrDefault();
                    Publicaciones.Remove(p);
                    OnPropertyChanged();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                try
                {
                    byte[] binaryData = Convert.FromBase64String(e.Foto);
                    BitmapImage bi = new BitmapImage();
                    bi.BeginInit();
                    bi.StreamSource = new MemoryStream(binaryData);
                    bi.EndInit();
                    P.FotoSrc = bi;
                    Publicaciones.Insert(0,P);
                    OnPropertyChanged();

                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

        }
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        void CheckServerStatus()
        {
            if(server.ServerActivo)
            {
                ServidorCorriendo = true;
                
            }
            else
            {
               
                ServidorCorriendo = false;
                Usuarios.Clear();
            }
        }
    }
}
