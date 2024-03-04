using System;
using System.Collections.ObjectModel;
using System.IO;

namespace AnimeX.Model
{
    public class RegistroAnime 
    {
        private const string NOMBRE_FICHERO = "databank.data";
        private ObservableCollection<Anime> lista;

        public RegistroAnime()
        {
            lista = new ObservableCollection<Anime>();
            CargarRegistros();
        }
        
        public void PrintAllAnime()
        {
            foreach (var anime in Lista)
            {
                anime.PrintAnimeDetails();
                Console.WriteLine(); // Añadir una línea en blanco entre cada anime para mayor legibilidad
            }
        }

        public ObservableCollection<Anime> Lista => lista;

        public void AgregarAnime(Anime anime)
        {
            lista.Add(anime);
            GuardarRegistros(); // Guardar el registro inmediatamente después de agregar uno nuevo.
        }

        public void EliminarAnime(int indice)
        {
            if (indice >= 0 && indice < lista.Count)
            {
                lista.RemoveAt(indice);
                GuardarRegistros(); // Guardar el registro después de eliminar uno.
            }
            else
            {
                throw new IndexOutOfRangeException("Índice de reseña fuera de rango.");
            }
        }

        private void GuardarRegistros()
        {
            try
            {
                using (BinaryWriter bw = new BinaryWriter(File.OpenWrite(NOMBRE_FICHERO)))
                {
                    foreach (Anime anime in lista)
                    {
                        anime.GuardarAnime(bw);
                    }
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Error al guardar registros: {ex.Message}");
            }
        }

        private void CargarRegistros()
        {
            if (File.Exists(NOMBRE_FICHERO))
            {
                try
                {
                    using (BinaryReader br = new BinaryReader(File.OpenRead(NOMBRE_FICHERO)))
                    {
                        while (br.PeekChar() != -1)
                        {
                            lista.Add(Anime.CargarAnime(br));
                        }
                    }
                }
                catch (IOException ex)
                {
                    Console.WriteLine($"Error al cargar registros: {ex.Message}");
                }
            }
        }
    }
}
