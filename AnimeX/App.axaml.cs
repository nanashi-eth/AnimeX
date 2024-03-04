using AnimeX.Model;
using AnimeX.ViewModel;
using AnimeX.View;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

namespace AnimeX;

public partial class App : Application
{
    public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override async void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                // Creamos una instancia de RegistroAnime
                var registroAnime = new RegistroAnime();
                // Creamos una instancia de MainWindowViewModel
                var mainWindowViewModel = new MainWindowViewModel(registroAnime);

                // Creamos una instancia de MainWindow y configuramos su DataContext con la instancia de MainWindowViewModel
                var mainWindow = new MainWindowView()
                {
                    DataContext = mainWindowViewModel
                };

                // Configuramos la MainWindow como la ventana principal de la aplicación
                desktop.MainWindow = mainWindow;

                // Mostramos la MainWindow
                mainWindow.Show();

                // Llamamos al método LoadContentAsync() de MainWindowViewModel, que realizará los cambios de contenido
                await mainWindowViewModel.LoadContentAsync();
            }

            base.OnFrameworkInitializationCompleted();
        }
    }