using System.IO;

namespace AnimeX.Model;

    public class Anime
    {
        public char Tipo { get; set; } // Tipo de anime: P (Película), S (Serie), D (Documental)
        public string Titulo { get; set; }
        public int Anyo { get; set; }
        public string Generos { get; set; }
        public double Nota { get; set; }
        public string Contenido { get; set; }
        public bool Recomendado { get; set; }
        public byte[] Imagen { get; set; }

        public Anime(char tipo, string titulo, int anyo, string generos, double nota, string contenido, bool recomendado, byte[] imagen)
        {
            Tipo = tipo;
            Titulo = titulo;
            Anyo = anyo;
            Generos = generos;
            Nota = nota;
            Contenido = contenido;
            Recomendado = recomendado;
            Imagen = imagen;
        }

        public static Anime CargarAnime(BinaryReader br)
        {
            char tipo = br.ReadChar();
            string titulo = br.ReadString();
            int anyo = br.ReadInt32();
            string generos = br.ReadString();
            double nota = br.ReadDouble();
            string contenido = br.ReadString();
            bool recomendado = br.ReadBoolean();
            int imagenLength = br.ReadInt32();
            byte[] imagen = br.ReadBytes(imagenLength);

            return new Anime(tipo, titulo, anyo, generos, nota, contenido, recomendado, imagen);
        }

        public void GuardarAnime(BinaryWriter bw)
        {
            bw.Write(Tipo);
            bw.Write(Titulo);
            bw.Write(Anyo);
            bw.Write(Generos);
            bw.Write(Nota);
            bw.Write(Contenido);
            bw.Write(Recomendado);
            bw.Write(Imagen.Length);
            bw.Write(Imagen);
        }
    }
