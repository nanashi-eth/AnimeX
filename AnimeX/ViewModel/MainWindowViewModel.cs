﻿using System;
using System.ComponentModel;
using ReactiveUI;
using System.Threading.Tasks;

namespace AnimeX.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        private SplashScreenViewModel _splashScreenViewModel;
        private ReactiveObject _content;
        public ReactiveObject Content
        {
            get => _content;
            set => this.RaiseAndSetIfChanged(ref _content, value);
        }

        public MainWindowViewModel()
        {
            _splashScreenViewModel = new SplashScreenViewModel();
            Content = _splashScreenViewModel;
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
                // Después de que se haya cargado el contenido inicial, cambia al CreateAnimeViewModel
                Content = new CreateAnimeViewModel();
            }
            catch (Exception ex)
            {
                // Maneja cualquier excepción que pueda ocurrir durante la carga.
                // Aquí podrías establecer el contenido en un ViewModel de error o manejarlo de otra manera según tu aplicación.
                Console.WriteLine($"Error durante la carga: {ex.Message}");
            }
        }
    }
}