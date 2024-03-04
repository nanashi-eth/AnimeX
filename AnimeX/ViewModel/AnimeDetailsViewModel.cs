using ReactiveUI;
using System;
using System.Reactive;
using AnimeX.Model;
using Avalonia.Media.Imaging;
using System.IO;
using System.Reactive.Linq;

namespace AnimeX.ViewModel
{
    public class AnimeDetailsViewModel : ViewModelBase
    {
        private Anime _currentAnime;
        private int _currentIndex;
        private int _totalAnimes;

        private string _anyoFormatted;

        public string AnyoFormatted
        {
            get => _anyoFormatted;
            set => this.RaiseAndSetIfChanged(ref _anyoFormatted, value);
        }

        public Anime CurrentAnime
        {
            get => _currentAnime;
            set => this.RaiseAndSetIfChanged(ref _currentAnime, value);
        }

        public int CurrentIndex
        {
            get => _currentIndex;
            set
            {
                this.RaiseAndSetIfChanged(ref _currentIndex, value);
                Prev = CurrentIndex < TotalAnimes - 1;
                Next = CurrentIndex > 0;
            } 
        }

        public int TotalAnimes
        {
            get => _totalAnimes;
            set
            {
                this.RaiseAndSetIfChanged(ref _totalAnimes, value);
                Prev = CurrentIndex < TotalAnimes - 1;
                Next = CurrentIndex > 0;
            } 
        }

        public ReactiveCommand<Unit, Unit> GoToPreviousAnimeCommand { get; }
        public ReactiveCommand<Unit, Unit> GoToNextAnimeCommand { get; }

        private readonly RegistroAnime _registroAnime;

        public AnimeDetailsViewModel(RegistroAnime registroAnime)
        {
            _registroAnime = registroAnime;
            GoToPreviousAnimeCommand = ReactiveCommand.Create(DoGoToPreviousAnime);
            GoToNextAnimeCommand = ReactiveCommand.Create(DoGoToNextAnime);

            CurrentIndex = 0;
            TotalAnimes = _registroAnime.Lista.Count;

            if (TotalAnimes > 0)
                CurrentAnime = _registroAnime.Lista[CurrentIndex];
            UpdateCurrentAnimeProperties();
        }

        private bool _next = true;

        public bool Next
        {
            get => _next;
            set
            {
                this.RaiseAndSetIfChanged(ref _next, value);
                Console.WriteLine(_next);
            }
        }

        private bool _prev = true;

        public bool Prev
        {
            get => _prev;
            set
            {
                this.RaiseAndSetIfChanged(ref _prev, value);
                Console.WriteLine(_prev);
            }
        }

        private void DoGoToPreviousAnime()
        {
            if (CurrentIndex > 0)
            {
                CurrentIndex--;
                CurrentAnime = _registroAnime.Lista[CurrentIndex];
                UpdateCurrentAnimeProperties(); // Actualizar propiedades después de cambiar de anime
            }
        }

        private void DoGoToNextAnime()
        {
            if (CurrentIndex < TotalAnimes - 1)
            {
                CurrentIndex++;
                CurrentAnime = _registroAnime.Lista[CurrentIndex];
                UpdateCurrentAnimeProperties(); // Actualizar propiedades después de cambiar de anime
            }
        }

        // Método para actualizar las propiedades del anime actual en la vista
        private void UpdateCurrentAnimeProperties()
        {
            CurrentAnimeInfo = $"{CurrentAnime.Titulo}'"; // Actualizar el título en la vista
            // Convertir el año de binario a DateTime y formatearlo como año
            AnyoFormatted = DateTime.FromBinary(CurrentAnime.AnyoTicks).ToString("MM/dd/yyyy");
            // Convertir el array de bytes de la imagen a un objeto Bitmap
            ImagenBitmap = ConvertByteArrayToBitmap(CurrentAnime.Imagen);
        }

        private Bitmap _imagenBitmap;

        public Bitmap ImagenBitmap
        {
            get => _imagenBitmap;
            set => this.RaiseAndSetIfChanged(ref _imagenBitmap, value);
        }

        private bool _recomendado;

        public bool Recomendado
        {
            get => _recomendado;
            set => this.RaiseAndSetIfChanged(ref _recomendado, value);
        }

        public string RecomendadoString => Recomendado ? "Sí" : "No";


        public string CurrentAnimeInfo { get; set; }

        // Método para convertir un array de bytes en un objeto Bitmap
        private Bitmap ConvertByteArrayToBitmap(byte[] byteArray)
        {
            if (byteArray == null)
                return null;
            Avalonia.Media.Imaging.Bitmap bitmap;

            using (MemoryStream ms = new MemoryStream(byteArray))
            {
                ms.Position = 0;

                bitmap = new Avalonia.Media.Imaging.Bitmap(ms);
            }

            return bitmap;
        }
    }
}