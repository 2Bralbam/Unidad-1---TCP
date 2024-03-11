using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Unidad_1___TCP.Models;
using Unidad_1___TCP.Services;

namespace Unidad_1___TCP.ViewModels
{
    public class HomeViewModel:INotifyPropertyChanged
    {
        TCPServer server = new TCPServer();
        public ObservableCollection<string> Usuarios { get; set; } = new();
        public ObservableCollection<Publicacion> Publicaciones = new();
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
                IdPublicacion = Publicaciones.Count + 1, 
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
