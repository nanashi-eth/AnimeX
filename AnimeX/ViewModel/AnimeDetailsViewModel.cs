using ReactiveUI;
using System;
using System.Reactive;
using AnimeX.Model;
using Avalonia.Media.Imaging;
using System.IO;

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
        public ReactiveCommand<Unit, Unit> DeleteCurrentAnimeCommand { get; }

        private RegistroAnime _registroAnime;

        public AnimeDetailsViewModel(ref RegistroAnime registroAnime)
        {
            _registroAnime = registroAnime;
            GoToPreviousAnimeCommand = ReactiveCommand.Create(DoGoToPreviousAnime);
            GoToNextAnimeCommand = ReactiveCommand.Create(DoGoToNextAnime);
            DeleteCurrentAnimeCommand = ReactiveCommand.Create(DoDeleteCurrentAnime);

            // Verificar si hay animes en la lista
            if (_registroAnime.Lista.Count > 0)
            {
                CurrentIndex = 0;
                TotalAnimes = _registroAnime.Lista.Count;
                CurrentAnime = _registroAnime.Lista[CurrentIndex];
            }
            else
            {
                // Cargar valores predeterminados en caso de que no haya animes
                CurrentIndex = -1;
                TotalAnimes = 0;
                CurrentAnime = new Anime(); // O cualquier otro valor predeterminado que desees
            }
    
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
            if (CurrentAnime != null)
            {
                CurrentAnimeInfo = $"{CurrentAnime.Titulo}'"; // Actualizar el título en la vista
                // Convertir el año de binario a DateTime y formatearlo como año
                AnyoFormatted = DateTime.FromBinary(CurrentAnime.AnyoTicks).ToString("MM/dd/yyyy");
                // Convertir el array de bytes de la imagen a un objeto Bitmap
                ImagenBitmap = ConvertByteArrayToBitmap(CurrentAnime.Imagen);
            }
            else
            {
                CurrentAnimeInfo = " "; 
                AnyoFormatted = " ";
                ImagenBitmap = new Bitmap("Assets/loading.png");
            }
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

                bitmap = new Bitmap(ms);
            }

            return bitmap;
        }
        private void DoDeleteCurrentAnime()
        {
            try
            {
                // Eliminar el anime actual de la lista
                _registroAnime.EliminarAnime(CurrentIndex);
                
                // Actualizar el índice y total de animes
                TotalAnimes = _registroAnime.Lista.Count;
                CurrentIndex = Math.Min(CurrentIndex, TotalAnimes - 1);
                
                // Actualizar las propiedades del anime actual
                CurrentAnime = TotalAnimes > 0 ? _registroAnime.Lista[CurrentIndex] : null;
                UpdateCurrentAnimeProperties();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar el anime: {ex.Message}");
            }
        }
    }
}