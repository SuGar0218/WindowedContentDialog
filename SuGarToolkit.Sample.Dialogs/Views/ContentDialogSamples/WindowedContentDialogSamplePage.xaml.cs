using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;

using SuGarToolkit.Controls.Dialogs;
using SuGarToolkit.Sample.Dialogs.ViewModels;

namespace SuGarToolkit.Sample.Dialogs.Views.ContentDialogSamples;

public sealed partial class WindowedContentDialogSamplePage : Page
{
    public WindowedContentDialogSamplePage()
    {
        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        settings = (ContentDialogSettings) e.Parameter;
        base.OnNavigatedTo(e);
    }

    private ContentDialogSettings settings = new();

    private async void ShowWindowedContentDialog()
    {
        WindowedContentDialog dialog = new()
        {
            WindowTitle = settings.Title,
            Title = settings.Title,
            Content = !string.IsNullOrEmpty(settings.Message) ? settings.Message : new LoremIpsumPage(),

            PrimaryButtonText = settings.PrimaryButtonText,
            SecondaryButtonText = settings.SecondaryButtonText,
            CloseButtonText = settings.CloseButtonText,
            DefaultButton = settings.DefaultButton,

            OwnerWindow = settings.IsChild ? App.Current.MainWindow : null,
            IsTitleBarVisible = settings.IsTitleBarVisible,
            CenterInParent = settings.CenterInParent,

            //DisableBehind = settings.DisableBehind,
            SmokeLayerKind = settings.SmokeLayerKind,
            CustomSmokeLayer = ContentDialogSamplesPage.CustomSmokeLayer,

            RequestedTheme = settings.RequestedTheme,
            SystemBackdrop = settings.BackdropType switch
            {
                BuiltInSystemBackdropType.Mica => new MicaBackdrop { Kind = Microsoft.UI.Composition.SystemBackdrops.MicaKind.Base },
                BuiltInSystemBackdropType.MicaAlt => new MicaBackdrop { Kind = Microsoft.UI.Composition.SystemBackdrops.MicaKind.BaseAlt },
                BuiltInSystemBackdropType.Arcylic => new DesktopAcrylicBackdrop(),
                _ => null
            }
        };
        if (settings.PrimaryButtonNotClose)
        {
            dialog.PrimaryButtonClick += (o, e) => e.Cancel = true;
        }
        if (settings.SecondaryButtonNotClose)
        {
            dialog.SecondaryButtonClick += (o, e) => e.Cancel = true;
        }
        ContentDialogResult result = await dialog.ShowAsync(settings.IsModal);
        ContentDialogResultBox.Text = result.ToString();
    }
}
