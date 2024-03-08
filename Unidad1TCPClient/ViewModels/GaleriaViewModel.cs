using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Unidad1TCPClient.Services;

namespace Unidad1TCPClient.ViewModels
{
    public class GaleriaViewModel : INotifyPropertyChanged
    {
        #region Variables
        public bool Conectado { get; set; } = false;
        public string IP { get; set; } = "127.0.0.1";
        public int Puerto { get; set; } = 55555;
        protected GaleriaService GaleriaService { get; set; } = new();
        public string Imagen { get; set; } = "No hay Imagen";
        public string? ImagenSeleccionada { get; set; }
        #endregion
        #region Listas
        public ObservableCollection<string> ListaImagenes { get; set; } = new();
        #endregion 
        #region Comandos
        public ICommand ConectarCommand { get; set; }
        public ICommand DesconectarCommand { get; set; }
        public ICommand SeleccionarFotoCommand { get; set; }
        public ICommand CompartirFotoCommand { get; set; }
        public ICommand EliminarFotoCommand { get; set; }
        #endregion
        public GaleriaViewModel()
        {
            ConectarCommand = new RelayCommand(ConectarServer);
            DesconectarCommand = new RelayCommand(DesconectarServer);
            SeleccionarFotoCommand = new RelayCommand(SeleccionarFoto);
            CompartirFotoCommand = new RelayCommand(CompartirFoto);
            EliminarFotoCommand = new RelayCommand<string>(EliminarFoto);
        }
        #region Fotos
        private void CompartirFoto()
        {
            if (GaleriaService.CompartirImagen(Imagen,IPAddress.Parse(IP),Puerto))
            {
                MessageBox.Show("La imagen se ah enviado al servidor");
            }
            else
            {
                MessageBox.Show("La imagen no fue enviada.");
            }
            ListaImagenes.Add(Imagen);
            OnPropertyChanged(nameof(ListaImagenes));
        }
        private void SeleccionarFoto()
        {
            OpenFileDialog openFileDialog = new()
            {
                //Permite solo imagenes
                Filter = "Archivos de imagen|*.jpg;*.jpeg;*.png;*.gif;*.bmp|Todos los archivos (*.*)|*.*"
            };
           
            if (openFileDialog.ShowDialog() == true)
            {
                // Aquí puedes manejar la imagen seleccionada, por ejemplo, obtener la ruta del archivo
                Imagen = openFileDialog.FileName;
            }
            else
            {
                Imagen = "No hay Imagen";
            }
            OnPropertyChanged(nameof(Imagen));
        }
        private void EliminarFoto(string? imagen)
        {
            if (!string.IsNullOrEmpty(ImagenSeleccionada))
            {
                // Eliminar en servidor
                GaleriaService.EliminarImagen(ImagenSeleccionada, IPAddress.Parse(IP), Puerto);
                // Eliminar localmente
                ListaImagenes.Remove(ImagenSeleccionada);
                // Limpiar la imagen seleccionada
                ImagenSeleccionada = string.Empty;
                OnPropertyChanged(nameof(ListaImagenes));
            }
        }
        #endregion
        #region Servidor
        private void DesconectarServer()
        {
            if (GaleriaService.Desconectar())
            {
                Conectado = false;
                MessageBox.Show("Se ah desconectado del servidor");
                OnPropertyChanged(nameof(Conectado));
            }
        }
        private void ConectarServer()
        {
            if (GaleriaService.Conectar(IPAddress.Parse(IP), Puerto))
            {
                MessageBox.Show("Se ah conectado al servidor");
                Conectado = true;
                OnPropertyChanged(nameof(Conectado));
            }
        }
        #endregion
        #region Actualizacion
        void OnPropertyChanged(string Propertyname = null!)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(Propertyname));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        #endregion
    }
}