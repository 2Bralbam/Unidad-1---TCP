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
        ObservableCollection<MensajeDTO> Publicaciones = new();
        public ICommand IniciarServer { get; set; }
        public ICommand DetenerServer { get; set; }
        public string IP { get
            {
                var Direcciones = Dns.GetHostAddresses(Dns.GetHostName());
                if (Direcciones != null)
                {
                    return string.Join("/", Direcciones.Where(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        .Select(x => x.ToString()));
                }
                else
                {
                    return "No hay direcciones";
                }
            } }
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
            e.IdPublicacion = Publicaciones.Count + 1;
            if(e.Mensaje == "**-")
            {
                Usuarios.Remove(e.Usuario);
            }
            else if(e.Mensaje == "**+")
            {
                Usuarios.Add(e.Usuario);
            }
            else
            {
                Publicaciones.Add(e);
            }

        }
    }
}
