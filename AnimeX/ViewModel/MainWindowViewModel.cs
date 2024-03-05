using System;
using System.Reactive;
using System.Threading.Tasks;
using ReactiveUI;
using AnimeX.Model;
using AnimeX.View;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Material.Icons;

namespace AnimeX.ViewModel
{
    public class MainWindowViewModel : ReactiveObject
    {
        private SplashScreenViewModel _splashScreenViewModel;
        private CreateAnimeViewModel _createAnimeViewModel;
        private ReactiveObject _content;
        private RegistroAnime _registroAnime;
        private AnimeDetailsViewModel _animeDetailsViewModel;
        private bool _isAppLoaded;

        public bool IsAppLoaded
        {
            get => _isAppLoaded;
            set => this.RaiseAndSetIfChanged(ref _isAppLoaded, value);
        }

        public ReactiveObject Content
        {
            get => _content;
            set => this.RaiseAndSetIfChanged(ref _content, value);
        }

        public ReactiveCommand<Unit, Unit> ChangeToNewAnimeViewCommand { get; private set; }
        public ReactiveCommand<Unit, Unit> ChangeToListAnimeViewCommand { get; private set; }

        public MainWindowViewModel(RegistroAnime registroAnime)
        {
            _registroAnime = registroAnime;
            _splashScreenViewModel = new SplashScreenViewModel();
            _createAnimeViewModel = new CreateAnimeViewModel(registroAnime: _registroAnime);
            _animeDetailsViewModel = new AnimeDetailsViewModel(ref _registroAnime);
            Content = _splashScreenViewModel;
            IsAppLoaded = false;

            ChangeToNewAnimeViewCommand = ReactiveCommand.Create(ChangeToNewAnimeView);
            ChangeToListAnimeViewCommand = ReactiveCommand.Create(ChangeToListAnimeView);
        }
        public void GuardarRegistros()
        {
            try
            {
                // Lógica para guardar los registros en un archivo, por ejemplo
                _registroAnime.GuardarRegistros(); // Suponiendo que _registroAnime es una instancia de la clase que contiene el método GuardarRegistros()
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al guardar los registros: {ex.Message}");
            }
        }


        private void ChangeToNewAnimeView()
        {
            Content = _createAnimeViewModel;
        }

        private void ChangeToListAnimeView()
        {
            _animeDetailsViewModel = new AnimeDetailsViewModel(ref _registroAnime);
            Content = _animeDetailsViewModel;
        }

        public async Task LoadContentAsync()
        {
            try
            {
                var progressValue = 20;
                _splashScreenViewModel.StartupMessage = "Buscando Furros...";
                _splashScreenViewModel.ProgressBar += progressValue;
                await Task.Delay(1000);

                _splashScreenViewModel.ProgressBar += progressValue;
                _splashScreenViewModel.StartupMessage = "Animando Personajes...";
                await Task.Delay(1000);

                _splashScreenViewModel.ProgressBar += progressValue;
                _splashScreenViewModel.StartupMessage = "Afilando Katanas...";
                await Task.Delay(1000);
                
                _splashScreenViewModel.ProgressBar += progressValue;
                _splashScreenViewModel.StartupMessage = "Cargando Covers...";
                await Task.Delay(1000);
                
                _splashScreenViewModel.ProgressBar += progressValue;
                _splashScreenViewModel.StartupMessage = "Ocultando Spoilers...";
                await Task.Delay(2000);

                Content = _animeDetailsViewModel;
                await MessageBox.Show(null, "Animes cargados correctamente", "Informacion", MaterialIconKind.Info);
                IsAppLoaded = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error durante la carga: {ex.Message}");
            }
        }
    }
}
