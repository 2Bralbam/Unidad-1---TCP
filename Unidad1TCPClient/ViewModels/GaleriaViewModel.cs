using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unidad1TCPClient.ViewModels
{
    public class GaleriaViewModel : INotifyPropertyChanged
    {
        public bool Conectado { get; set; } = false;
        public string IP { get; set; } = "127.0.0.1";
        public int Port { get; set; } = 55555;

        void OnPropertyChanged(string Propertyname = null!)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(Propertyname));
        }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
