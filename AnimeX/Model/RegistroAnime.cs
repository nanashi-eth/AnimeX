﻿using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.InteropServices;
using AnimeX.View;
using Material.Icons;

namespace AnimeX.Model
{
    public class RegistroAnime 
    {
        private const string NOMBRE_FICHERO = "databank.data";
        private ObservableCollection<Anime> lista;

        public ObservableCollection<Anime> Lista
        {
            get => lista;
            private set
            {
                lista = value;
                // Notificar a los suscriptores que la propiedad ha cambiado
                ListaChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public event EventHandler ListaChanged;

        public RegistroAnime()
        {
            Lista = new ObservableCollection<Anime>();
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

        public void AgregarAnime(Anime anime)
        {
            Lista.Add(anime);// Guardar el registro inmediatamente después de agregar uno nuevo.
            GuardarRegistros();
        }

        public void EliminarAnime(int indice)
        {
            if (indice >= 0 && indice < Lista.Count)
            {
                Lista.RemoveAt(indice);
            }
            else
            {
                throw new IndexOutOfRangeException("Índice de reseña fuera de rango.");
            }
            foreach (Anime anime in Lista)
            {
                Console.WriteLine(anime.Titulo);
            }
            GuardarRegistros();
        }

        public void GuardarRegistros()
        {
            try
            {
                if (Lista.Count == 0)
                {
                    if (File.Exists(NOMBRE_FICHERO))
                    {
                        File.Delete(NOMBRE_FICHERO);
                    }
                }
                using (BinaryWriter bw = new BinaryWriter(File.OpenWrite(NOMBRE_FICHERO)))
                {
                    foreach (Anime anime in Lista)
                    {
                        anime.GuardarAnime(bw);
                    }
                }
            }
            catch (IOException ex)
            {
                MessageBox.Show(null, "Error al guardar registros", "Error", MaterialIconKind.Error);
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
                            Lista.Add(Anime.CargarAnime(br));
                        }
                    }
                }
                catch (IOException ex)
                {
                    MessageBox.Show(null, "Error al cargar registros", "Error", MaterialIconKind.Error);
                }
            }
        }
    }
}
