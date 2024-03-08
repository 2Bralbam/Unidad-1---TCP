using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
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
        ObservableCollection<Publicacion> Publicaciones = new();
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
                server.IniciarServer();
            });
            DetenerServer = new RelayCommand(() =>
            {
                server.DetenerServer();
                Usuarios.Clear();
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
            P.IdPublicacion = Publicaciones.Max(x=>x.IdPublicacion) +1;
            bool IdRepetido = true;
            while (IdRepetido)
            {
                IdRepetido = Publicaciones.Any(x => x.IdPublicacion == P.IdPublicacion);
                if (IdRepetido)
                {
                    P.IdPublicacion = Publicaciones.Max(x => x.IdPublicacion) + 1;
                    Publicaciones.Add(P);
                }
                else
                {
                    
                    IdRepetido = false;
                    Publicaciones.Add(P);
                }
            }
            
            if(e.Mensaje == "**-")
            {
                Usuarios.Remove(e.Usuario);
            }
            else
            {
                Usuarios.Add(e.Usuario);
            }

        }
    }
}
