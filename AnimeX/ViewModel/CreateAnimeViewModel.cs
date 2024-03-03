using ReactiveUI;
using System;
using System.Reactive;
using System.Reactive.Subjects;

namespace AnimeX.ViewModel
{
    public class CreateAnimeViewModel : ViewModelBase
    {
        private char _tipo;
        private string _titulo;
        private int _anyo;
        private string _generos;
        private double _nota;
        private string _contenido;
        private bool _recomendado;

        public char Tipo
        {
            get => _tipo;
            set => this.RaiseAndSetIfChanged(ref _tipo, value);
        }

        public string Titulo
        {
            get => _titulo;
            set => this.RaiseAndSetIfChanged(ref _titulo, value);
        }

        public int Anyo
        {
            get => _anyo;
            set => this.RaiseAndSetIfChanged(ref _anyo, value);
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
            get => _contenido;
            set => this.RaiseAndSetIfChanged(ref _contenido, value);
        }

        public bool Recomendado
        {
            get => _recomendado;
            set => this.RaiseAndSetIfChanged(ref _recomendado, value);
        }

        // Comando para guardar el anime
        public ReactiveCommand<Unit, Unit> GuardarAnimeCommand { get; }

        // Observable para notificar cuando se guarda un anime
        public IObservable<Unit> AnimeGuardado => _animeGuardado;
        private readonly Subject<Unit> _animeGuardado = new Subject<Unit>();

        public CreateAnimeViewModel()
        {
            GuardarAnimeCommand = ReactiveCommand.Create(DoGuardarAnime);
        }

        private void DoGuardarAnime()
        {
            try
            {
                // Aquí podrías llamar a un método en tu servicio o repositorio para guardar el anime en tu sistema de almacenamiento (por ejemplo, base de datos, archivo, etc.)
                // Por simplicidad, simplemente lo imprimimos en la consola en este ejemplo
                Console.WriteLine(
                    $"Anime guardado: Tipo={Tipo}, Título={Titulo}, Año={Anyo}, Géneros={Generos}, Nota={Nota}, Contenido={Contenido}, Recomendado={Recomendado}");


            }
            catch (Exception ex)
            {
                // Manejar cualquier error que pueda ocurrir durante el proceso de guardado
                Console.WriteLine($"Error al guardar el anime: {ex.Message}");
            }
        }
    }
}