using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Unidad1TCPClient.Services;

namespace Unidad1TCPClient.ViewModels
{
    public class GaleriaViewModel : INotifyPropertyChanged
    {
        public bool Conectado { get; set; } = false;
        public string IP { get; set; } = "127.0.0.1";
        public int Port { get; set; } = 55555;
        public string vista = "";
        public ICommand ConectarCommand { get; set; }
        public ICommand DesconectarCommand { get; set; }
        protected GaleriaService GaleriaService { get; set; } = new();

        public GaleriaViewModel()
        {
            ConectarCommand = new RelayCommand(ConectarServer);
            DesconectarCommand = new RelayCommand(DesconectarServer);
        }
        private void DesconectarServer()
        {
            GaleriaService.Desconectar();
            Conectado = false;
            OnPropertyChanged(nameof(Conectado));
        }
        void ConectarServer()
        {
            GaleriaService.Conectar(IPAddress.Parse(IP));
            Conectado = true;
            MessageBox.Show("Se ah conectado al servidor correctamente");
            OnPropertyChanged(nameof(Conectado));
        }

        void OnPropertyChanged(string Propertyname = null!)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(Propertyname));
        }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}