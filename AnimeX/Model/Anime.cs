using System;
using System.IO;

namespace AnimeX.Model
{
    public class Anime
    {
        public char Tipo { get; set; } // Tipo de anime: P (Película), S (Serie), D (Documental)
        public string Titulo { get; set; }
        public long AnyoTicks { get; set; } // Almacenar ticks en lugar de DateTime directamente
        public string Generos { get; set; }
        public double Nota { get; set; }
        public string Contenido { get; set; }
        public bool Recomendado { get; set; }
        public byte[] Imagen { get; set; }

        public Anime(char tipo, string titulo, DateTime anyo, string generos, double nota, string contenido, bool recomendado, byte[] imagen)
        {
            Tipo = tipo;
            Titulo = titulo;
            AnyoTicks = anyo.ToBinary(); // Convertir DateTime a ticks
            Generos = generos;
            Nota = nota;
            Contenido = contenido;
            Recomendado = recomendado;
            Imagen = imagen;
        }
        public Anime(){}

        public static Anime CargarAnime(BinaryReader br)
        {
            char tipo = br.ReadChar();
            string titulo = br.ReadString();
            long anyoTicks = br.ReadInt64(); // Leer ticks en lugar de DateTime directamente
            string generos = br.ReadString();
            double nota = br.ReadDouble();
            string contenido = br.ReadString();
            bool recomendado = br.ReadBoolean();
            int imagenLength = br.ReadInt32();
            byte[] imagen = br.ReadBytes(imagenLength);

            // Convertir ticks de vuelta a DateTime
            DateTime anyo = DateTime.FromBinary(anyoTicks);

            return new Anime(tipo, titulo, anyo, generos, nota, contenido, recomendado, imagen);
        }

        public void GuardarAnime(BinaryWriter bw)
        {
            bw.Write(Tipo);
            bw.Write(Titulo);
            bw.Write(AnyoTicks); // Escribir los ticks en lugar de DateTime directamente
            bw.Write(Generos);
            bw.Write(Nota);
            bw.Write(Contenido);
            bw.Write(Recomendado);
            bw.Write(Imagen.Length);
            bw.Write(Imagen);
        }
        
        // Método para imprimir el registro completo del anime (sin incluir los arrays de bits de las imágenes)
        public void PrintAnimeDetails()
        {
            Console.WriteLine($"Tipo: {Tipo}");
            Console.WriteLine($"Título: {Titulo}");
            Console.WriteLine($"Año: {AnyoTicks}");
            Console.WriteLine($"Géneros: {Generos}");
            Console.WriteLine($"Nota: {Nota}");
            Console.WriteLine($"Contenido: {Contenido}");
            Console.WriteLine($"Recomendado: {(Recomendado ? "Sí" : "No")}");
        }
    }
}
