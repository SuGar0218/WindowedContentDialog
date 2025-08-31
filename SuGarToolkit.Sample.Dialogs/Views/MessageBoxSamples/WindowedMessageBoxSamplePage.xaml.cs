using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;

using SuGarToolkit.Controls.Dialogs;
using SuGarToolkit.Sample.Dialogs.ViewModels;

namespace SuGarToolkit.Sample.Dialogs.Views.MessageBoxSamples;

public sealed partial class WindowedMessageBoxSamplePage : Page
{
    public WindowedMessageBoxSamplePage()
    {
        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        settings = (MessageBoxSettings) e.Parameter;
        base.OnNavigatedTo(e);
    }

    private async void ShowWindowedMessageBox()
    {
        MessageBoxResult result = await MessageBox.ShowAsync(
            settings.IsModal,
            settings.IsChild ? App.Current.MainWindow : null,
            settings.Content,
            settings.Title,
            settings.Buttons,
            settings.Image,
            settings.DefaultButton,
            new MessageBoxOptions
            {
                IsTitleBarVisible = settings.IsTitleBarVisible,
                CenterInParent = settings.CenterInParent,
                SmokeBehind = settings.SmokeBehind,

                RequestedTheme = settings.RequestedTheme,
                SystemBackdrop = settings.BackdropType switch
                {
                    BuiltInSystemBackdropType.Mica => new MicaBackdrop { Kind = Microsoft.UI.Composition.SystemBackdrops.MicaKind.Base },
                    BuiltInSystemBackdropType.MicaAlt => new MicaBackdrop { Kind = Microsoft.UI.Composition.SystemBackdrops.MicaKind.BaseAlt },
                    BuiltInSystemBackdropType.Arcylic => new DesktopAcrylicBackdrop(),
                    _ => null
                }
            });
        MessageBoxResultBox.Text = result.ToString();
    }

    private MessageBoxSettings settings;
}
