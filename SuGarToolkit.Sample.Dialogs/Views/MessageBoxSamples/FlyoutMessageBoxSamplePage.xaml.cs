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

namespace SuGarToolkit.Sample.Dialogs.Views.MessageBoxSamples;

public sealed partial class FlyoutMessageBoxSamplePage : Page
{
    public FlyoutMessageBoxSamplePage()
    {
        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        settings = (MessageBoxSettings) e.Parameter;
        base.OnNavigatedTo(e);
    }

    private async void ShowFlyoutMessageBox()
    {
        if (settings.SmokeLayerKind is ContentDialogSmokeLayerKind.Custom && App.Current.MainWindow is not null)
        {
            SizeToXamlRoot(MessageBoxSamplesPage.CustomSmokeLayer, XamlRoot);
        }
        MessageBoxResult result = await FlyoutMessageBox.ShowAsync(
            ShowMessageBoxButton,
            settings.Content,
            settings.Title,
            settings.Buttons,
            settings.Image,
            settings.DefaultButton,
            new FlyoutMessageBoxOptions
            {
                ShouldConstrainToRootBounds = settings.ShouldConstrainToRootBounds,

                //DisableBehind = settings.DisableBehind,
                SmokeLayerKind = settings.SmokeLayerKind,
                CustomSmokeLayer = MessageBoxSamplesPage.CustomSmokeLayer,

                RequestedTheme = settings.RequestedTheme is ElementTheme.Default ? ActualTheme : settings.RequestedTheme,
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
