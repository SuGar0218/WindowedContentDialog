using Microsoft.UI.Xaml;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SuGarToolkit.Sample.Dialogs;

/// <summary>
/// Provides application-specific behavior to supplement the default Application class.
/// </summary>
public partial class App : Application
{
    public MainWindow? MainWindow { get; private set; }

    /// <summary>
    /// Initializes the singleton application object.
    /// This is the first line of authored code executed,
    /// and as such is the logical equivalent of main() or WinMain().
    /// </summary>
    public App()
    {
        InitializeComponent();
    }

    public static new App Current => (App) Application.Current;

    /// <summary>
    /// Invoked when the application is launched.
    /// </summary>
    /// <param name="args">Details about the launch request and process.</param>
    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        MainWindow = new MainWindow();
        //MainWindow.AppWindow.Resize(new Windows.Graphics.SizeInt32((int) (MainWindow.AppWindow.Size.Height * 1.618), MainWindow.AppWindow.Size.Height));
        MainWindow.Activate();
    }
}
