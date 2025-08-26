using Microsoft.UI.Xaml;
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
        if (settings.SmokeLayerKind is ContentDialogSmokeLayerKind.Custom)
        {
            SizeToXamlRoot(MessageBoxSamplesPage.CustomSmokeLayer, XamlRoot);
        }
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

                //DisableBehind = settings.DisableBehind,
                SmokeLayerKind = settings.SmokeLayerKind,
                CustomSmokeLayer = MessageBoxSamplesPage.CustomSmokeLayer,

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

    private static void SizeToXamlRoot(FrameworkElement element, XamlRoot root)
    {
        element.Width = root.Size.Width;
        element.Height = root.Size.Height;
    }
}
