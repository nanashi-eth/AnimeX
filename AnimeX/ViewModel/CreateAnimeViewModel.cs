using ReactiveUI;
using System;
using System.IO;
using System.Reactive;
using System.Threading.Tasks;
using System.Windows.Input;
using AnimeX.Model;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace AnimeX.ViewModel
{
    public class CreateAnimeViewModel : ViewModelBase
    {
        private string _titulo;
        private string _generos;
        private double _nota;
        private string _sinopsis;
        private bool _recomendado;
        private Bitmap? _imagen;
        private DateTime _anyo;
        private int _tipoIndex;

        public int TipoIndex
        {
            get => _tipoIndex;
            set => this.RaiseAndSetIfChanged(ref _tipoIndex, value);
        }

        public DateTime Anyo
        {
            get { return _anyo; }
            set
            {
                if (_anyo != value)
                {
                    _anyo = value;
                }
            }
        }

        public Bitmap? Imagen
        {
            get => _imagen;
            set => this.RaiseAndSetIfChanged(ref _imagen, value);
        }
        
        public ReactiveCommand<Unit, Unit> AgregarImagenCommand { get; }

        public string Titulo
        {
            get => _titulo;
            set => this.RaiseAndSetIfChanged(ref _titulo, value);
        }

        public string Generos
        {
            get => _generos;
            set => this.RaiseAndSetIfChanged(ref _generos, value);
        }

        public double Nota
        {
            get => _nota;
            set => this.RaiseAndSetIfChanged(ref _nota, value);
        }

        public string Contenido
        {
            get => _sinopsis;
            set => this.RaiseAndSetIfChanged(ref _sinopsis, value);
        }

        public bool Recomendado
        {
            get => _recomendado;
            set => this.RaiseAndSetIfChanged(ref _recomendado, value);
        }

        // Comando para guardar el anime
        public ICommand GuardarAnimeCommand { get; }

        private readonly RegistroAnime _registroAnime;

        // Constructor que recibe una instancia de RegistroAnime
        public CreateAnimeViewModel(RegistroAnime registroAnime)
        {
            _registroAnime = registroAnime;
            GuardarAnimeCommand = ReactiveCommand.CreateFromTask(DoGuardarAnime);
            AgregarImagenCommand = ReactiveCommand.CreateFromTask(DoAgregarImagen);
            Imagen = new Bitmap(AssetLoader.Open(new Uri("avares://AnimeX/Assets/loading.png")));
            Anyo = new DateTime(1999, 10, 20);
        }
        
        private async Task DoAgregarImagen()
        {
            try
            {
                var dlg = new OpenFileDialog();
                dlg.Filters!.Add(new FileDialogFilter() { Name = "Imágenes", Extensions = { "jpg", "png", "jpeg", "gif" } });
                dlg.AllowMultiple = false;

                var result = await dlg.ShowAsync(((IClassicDesktopStyleApplicationLifetime)Application.Current.ApplicationLifetime).MainWindow);
                if (result != null && result.Length > 0)
                {
                    // Leer la imagen desde el archivo seleccionado
                    using (var stream = File.OpenRead(result[0]))
                    {
                        var bitmap = new Bitmap(stream);
                        // Asignar la imagen al ViewModel
                        Imagen = bitmap;
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejar cualquier error que pueda ocurrir durante la selección de la imagen
                Console.WriteLine($"Error al agregar la imagen: {ex.Message}");
            }
        }

        private async Task DoGuardarAnime()
        {
            try
            {
                // Convertir el Bitmap a un arreglo de bytes
                if (Imagen == null)
                {
                    // Si la imagen es nula, maneja el caso según sea necesario
                    Console.WriteLine("La imagen es nula.");
                    return;
                }

                // Convertir el Bitmap a un array de bytes
                byte[] imagenBytes = ConvertirBitmapABytes(Imagen);
                // Crear un nuevo objeto Anime con los datos del ViewModel
                Anime nuevoAnime = new Anime
                {
                    Tipo = 'A',
                    Titulo = Titulo,
                    AnyoTicks = Anyo.ToBinary(),
                    Generos = Generos,
                    Nota = Nota,
                    Contenido = Contenido,
                    Recomendado = Recomendado,
                    Imagen = imagenBytes
                };

                // Agregar el nuevo anime a la lista de registros
                _registroAnime.AgregarAnime(nuevoAnime);

                // Resetear los campos después de agregar el anime
                LimpiarCampos();

                // Asegúrate de que la actualización de la interfaz de usuario se realice en el hilo principal
                Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() =>
                {
                    _registroAnime.PrintAllAnime();
                });
            }
            catch (Exception ex)
            {
                // Manejar cualquier error que pueda ocurrir durante el proceso de guardado
                Console.WriteLine($"Error al guardar el anime: {ex.Message}");
            }
        }

        private void LimpiarCampos()
        {
            // Restablecer los valores de los campos a valores predeterminados
            Titulo = "";
            Generos = "";
            Nota = 0;
            Contenido = "";
            Recomendado = false;
            Imagen = new Bitmap(AssetLoader.Open(new Uri("avares://AnimeX/Assets/loading.png")));
            Anyo = new DateTime(1999, 10, 20);
        }
        

        private byte[] ConvertirBitmapABytes(Bitmap bitmap)
        {
            using (var ms = new MemoryStream())
            {
                bitmap.Save(ms);
                return ms.ToArray();
            }
        }
    }
}
