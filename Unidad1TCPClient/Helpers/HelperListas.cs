using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Unidad1TCPClient.Helpers
{
    public class HelperListas
    {
        #region Metodos Listas
        public static ObservableCollection<string> CargarLista(ObservableCollection<string> lista)
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
                    lista.Clear();

                    // Agrega las imágenes a la lista
                    foreach (string line in lines)
                    {
                        lista.Add(line);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar la lista: {ex.Message}");
            }
            return lista;
        }
        public static void GuardarLista(ObservableCollection<string> lista)
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
                File.WriteAllLines(filePath, lista);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar la lista: {ex.Message}");
            }
        }
        #endregion
    }
}
