using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;

using SuGarToolkit.Controls.Dialogs;
using SuGarToolkit.Sample.Dialogs.ViewModels;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

using Windows.Foundation;
using Windows.Foundation.Collections;

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
        if (settings.SmokeLayerKind is ContentDialogSmokeLayerKind.Custom)
        {
            SizeToXamlRoot(ContentDialogSamplesPage.CustomSmokeLayer, XamlRoot);
        }
        FlyoutContentDialog dialog = new()
        {
            Title = settings.Title,
            Content = !string.IsNullOrEmpty(settings.Message) ? settings.Message : new LoremIpsumPage(),
            PrimaryButtonText = settings.PrimaryButtonText,
            SecondaryButtonText = settings.SecondaryButtonText,
            CloseButtonText = settings.CloseButtonText,
            DefaultButton = settings.DefaultButton,

            //DisableBehind = settings.DisableBehind,
            SmokeLayerKind = settings.SmokeLayerKind,
            CustomSmokeLayer = ContentDialogSamplesPage.CustomSmokeLayer,

            ShouldConstrainToRootBounds = settings.ShouldConstrainToRootBounds,
            PlacementTarget = ShowContentDialogButton,

            RequestedTheme = settings.RequestedTheme is ElementTheme.Default ? ActualTheme : settings.RequestedTheme,
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

    private static void SizeToXamlRoot(FrameworkElement element, XamlRoot root)
    {
        element.Width = root.Size.Width;
        element.Height = root.Size.Height;
    }
}
