using BasicMvvmSample.ViewModels;

namespace Client.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public string Greeting => "Welcome to Avalonia!";

    // Add our RactiveViewModel
    public ReactiveViewModel ReactiveViewModel { get; } = new ReactiveViewModel();
}