using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Input;
using Unidad1TCPClient.Services;

namespace Unidad1TCPClient.ViewModels
{
    public class GaleriaViewModel : INotifyPropertyChanged
    {
        #region Variables
        public bool Conectado { get; set; } = true;
        public string IP { get; set; } = "127.0.0.1";
        public int Puerto { get; set; } = 55555;
        protected GaleriaService GaleriaService { get; set; } = new();
        public string Imagen { get; set; } = "No hay Imagen";
        #endregion
        #region Listas
        public ObservableCollection<string> ListaImagenes { get; set; } = new()
        {
            "C:\\Users\\LuisR\\OneDrive\\Imágenes\\Bluestack\\Screenshot_2024.02.21_22.27.23.201.png",
            "C:\\Users\\LuisR\\OneDrive\\Imágenes\\Bluestack\\Screenshot_2024.03.05_01.41.33.361.png"
        };
        public string? ImagenSeleccionada { get; set; }

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
            EliminarFotoCommand = new RelayCommand(EliminarFoto);
            CargarLista();
        }

        #region Metodos Listas
        private void CargarLista()
        {
            try
            {
                // Verifica si el archivo existe antes de intentar cargarlo
                string filePath = "ListaImagenes.txt";
                if (File.Exists(filePath))
                {
                    // Lee todas las líneas del archivo
                    string[] lines = File.ReadAllLines(filePath);

                    // Limpia la lista actual
                    ListaImagenes.Clear();

                    // Agrega las imágenes a la lista
                    foreach (string line in lines)
                    {
                        ListaImagenes.Add(line);
                    }
                    //Actualizar lista
                    OnPropertyChanged(nameof(ListaImagenes));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar la lista: {ex.Message}");
            }
        }
        private void GuardarLista()
        {
            try
            {
                string filePath = "ListaImagenes.txt";
                //Crea el archivo si no existe
                if (!File.Exists(filePath))
                {
                    File.Create(filePath);
                }
                // Escribe las imágenes en el archivo
                File.WriteAllLines(filePath, ListaImagenes);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar la lista: {ex.Message}");
            }
        }
        #endregion
        #region Fotos
        private void CompartirFoto()
        {
            //Agregar la imagen al servidor
            if (GaleriaService.CompartirImagen(Imagen,IPAddress.Parse(IP),Puerto))
            {
                MessageBox.Show("La imagen se ah compartido");
                //Agrega la imagen a la lista local
                ListaImagenes.Add(Imagen);
                GuardarLista();
                OnPropertyChanged();
            }
            else
            {
                MessageBox.Show("La imagen no se ah compartido");
            }
        }
        private void SeleccionarFoto()
        {
            //Mostrar ventana para seleccionar la foto/imagen/mapa de bits/etc...
            OpenFileDialog openFileDialog = new()
            {
                //Permite solo imagenes
                Title = "Seleccionar imagen",
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
        private void EliminarFoto()
        {
            if (!string.IsNullOrEmpty(ImagenSeleccionada))
            {
                GaleriaService.EliminarImagen(ImagenSeleccionada, IPAddress.Parse(IP), Puerto);
                // Eliminar localmente
                ListaImagenes.Remove(ImagenSeleccionada);
                // Eliminar en servidor
                // Limpiar la imagen seleccionada
                ImagenSeleccionada = string.Empty;

                OnPropertyChanged();
            }
        }
        #endregion
        #region Servidor
        private void DesconectarServer()
        {
            if (GaleriaService.Desconectar())
            {
                /** Al desconectar el cliente del servidor mostrara un mensaje y cambiara de vista
                 */
                Conectado = false;
                MessageBox.Show("Se ah desconectado del servidor");
                OnPropertyChanged(nameof(Conectado));
            }
        }
        private void ConectarServer()
        {
            
            if (GaleriaService.Conectar(IPAddress.Parse(IP), Puerto))
            {
                /** Si la conexion fue exitosa mostrara un mensaje y cambiara la vista
                 */
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