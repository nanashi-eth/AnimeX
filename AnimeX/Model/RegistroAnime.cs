using System;
using System.Collections.Generic;
using System.IO;

namespace AnimeX.Model;

public class RegistroAnime
{
    private const string NOMBRE_FICHERO = "databank.data";
    private List<Anime> lista;

    public RegistroAnime()
    {
        lista = new List<Anime>();
        CargarRegistros();
    }

    public IReadOnlyList<Anime> Lista => lista;

    public void AgregarResenya(Anime anime)
    {
        lista.Add(anime);
    }

    public void EliminarResenya(int indice)
    {
        if (indice >= 0 && indice < lista.Count)
        {
            lista.RemoveAt(indice);
        }
        else
        {
            throw new IndexOutOfRangeException("Índice de resaña fuera de rango.");
        }
    }

    public void GuardarRegistros()
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