using Microsoft.UI;
using Microsoft.UI.Composition;
using Microsoft.UI.Composition.SystemBackdrops;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using SuGarToolkit.Sample.Dialogs.Views;

using WinRT;

namespace SuGarToolkit.Sample.Dialogs;

public sealed partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        ExtendsContentIntoTitleBar = true;
        //AppWindow.TitleBar.PreferredHeightOption = Microsoft.UI.Windowing.TitleBarHeightOption.Tall;

        // 当 Windows 设置 > 个性化 中，打开了在标题栏上显示强调色时，WinUI 3 标题栏按钮颜色显示不正确。
        RootFrame.Loaded += (sender, e) =>
        {
            switch (((FrameworkElement) sender).ActualTheme)
            {
                case ElementTheme.Light:
                    AppWindow.TitleBar.ButtonForegroundColor = Colors.Black;
                    break;
                case ElementTheme.Dark:
                    AppWindow.TitleBar.ButtonForegroundColor = Colors.White;
                    break;
            }

        };
        RootFrame.ActualThemeChanged += (sender, args) =>
        {
            switch (sender.ActualTheme)
            {
                case ElementTheme.Light:
                    AppWindow.TitleBar.ButtonForegroundColor = Colors.Black;
                    break;
                case ElementTheme.Dark:
                    AppWindow.TitleBar.ButtonForegroundColor = Colors.White;
                    break;
            }
        };

        AppWindow.Closing += (appWindow, e) =>
        {
            appWindow.Hide();
        };

        micaController = new MicaController();
        systemBackdropTarget = this.As<ICompositionSupportsSystemBackdrop>();
        systemBackdropConfig = new SystemBackdropConfiguration
        {
            Theme = SystemBackdropTheme.Default
        };
        micaController.AddSystemBackdropTarget(systemBackdropTarget);
        micaController.SetSystemBackdropConfiguration(systemBackdropConfig);

        Activated += (o, e) =>
        {
            MainWindow window = (MainWindow) o;
            window.systemBackdropConfig.IsInputActive = e.WindowActivationState is not WindowActivationState.Deactivated;
        };
    }

    public ElementTheme RequestedTheme
    {
        get => systemBackdropConfig.Theme switch
        {
            SystemBackdropTheme.Default => ElementTheme.Default,
            SystemBackdropTheme.Light => ElementTheme.Light,
            SystemBackdropTheme.Dark => ElementTheme.Dark,
            _ => ElementTheme.Default,
        };
        set
        {
            switch (value)
            {
                case ElementTheme.Default:
                    systemBackdropConfig.Theme = SystemBackdropTheme.Default;
                    AppWindow.TitleBar.PreferredTheme = Microsoft.UI.Windowing.TitleBarTheme.UseDefaultAppMode;
                    (Content as FrameworkElement)?.RequestedTheme = ElementTheme.Default;
                    break;
                case ElementTheme.Light:
                    systemBackdropConfig.Theme = SystemBackdropTheme.Light;
                    AppWindow.TitleBar.PreferredTheme = Microsoft.UI.Windowing.TitleBarTheme.Light;
                    (Content as FrameworkElement)?.RequestedTheme = ElementTheme.Light;
                    break;
                case ElementTheme.Dark:
                    systemBackdropConfig.Theme = SystemBackdropTheme.Dark;
                    AppWindow.TitleBar.PreferredTheme = Microsoft.UI.Windowing.TitleBarTheme.Dark;
                    (Content as FrameworkElement)?.RequestedTheme = ElementTheme.Dark;
                    break;
            }
        }
    }

    private void Frame_Loaded(object sender, RoutedEventArgs e)
    {
        Frame frame = (Frame) sender;
        frame.Navigate(typeof(MainPage));
    }

    private void Window_Closed(object sender, WindowEventArgs args)
    {
        micaController.Dispose();
    }

    private readonly MicaController micaController;
    private readonly ICompositionSupportsSystemBackdrop systemBackdropTarget;
    private readonly SystemBackdropConfiguration systemBackdropConfig;
}
