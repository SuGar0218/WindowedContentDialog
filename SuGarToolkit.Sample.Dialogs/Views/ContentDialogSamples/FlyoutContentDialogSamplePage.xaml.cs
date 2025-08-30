using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;

using SuGarToolkit.Controls.Dialogs;
using SuGarToolkit.Sample.Dialogs.ViewModels;

namespace SuGarToolkit.Sample.Dialogs.Views.ContentDialogSamples;

public sealed partial class FlyoutContentDialogSamplePage : Page
{
    public FlyoutContentDialogSamplePage()
    {
        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        settings = (ContentDialogSettings) e.Parameter;
        base.OnNavigatedTo(e);
    }

    private ContentDialogSettings settings = new();

    private async void ShowFlyoutContentDialog()
    {
        FlyoutContentDialog dialog = new()
        {
            Title = settings.Title,
            Content = !string.IsNullOrEmpty(settings.Message) ? settings.Message : new LoremIpsumPage(),
            PrimaryButtonText = settings.PrimaryButtonText,
            SecondaryButtonText = settings.SecondaryButtonText,
            CloseButtonText = settings.CloseButtonText,
            DefaultButton = settings.DefaultButton,
            SmokeBehind = settings.SmokeBehind,

            ShouldConstrainToRootBounds = settings.ShouldConstrainToRootBounds,
            Placement = settings.Placement,
            PlacementTarget = ShowContentDialogButton,

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
        ContentDialogResult result = await dialog.ShowAsync();
        ContentDialogResultBox.Text = result.ToString();
    }

    private async void ShowXamlFlyoutContentDialog()
    {
        ContentDialogResult result = await XamlFlyoutContentDialog.ShowAsync();
        ContentDialogResultBox.Text = result.ToString();
    }

    private static void SizeToXamlRoot(FrameworkElement element, XamlRoot root)
    {
        element.Width = root.Size.Width;
        element.Height = root.Size.Height;
    }
}
