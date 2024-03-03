namespace AnimeX.ViewModel;
using ReactiveUI;
public class SplashScreenViewModel : ViewModelBase
{
    private string _startupMessage;
    private int _progressBar;
    public string StartupMessage
    {
        get => _startupMessage;
        set => this.RaiseAndSetIfChanged(ref _startupMessage, value);
    }
    public int ProgressBar
    {
        get => _progressBar;
        set => this.RaiseAndSetIfChanged(ref _progressBar, value);
    }

}