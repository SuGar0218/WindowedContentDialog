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

public sealed partial class MuxcContentDialogSamplePage : Page
{
    public MuxcContentDialogSamplePage()
    {
        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        settings = (ContentDialogSettings) e.Parameter;
        base.OnNavigatedTo(e);
    }

    private async void ShowContentDialog()
    {
        ContentDialog dialog = new()
        {
            Title = settings.Title,
            Content = !string.IsNullOrEmpty(settings.Message) ? settings.Message : new LoremIpsumPage(),
            PrimaryButtonText = settings.PrimaryButtonText,
            SecondaryButtonText = settings.SecondaryButtonText,
            CloseButtonText = settings.CloseButtonText,
            DefaultButton = settings.DefaultButton,
            RequestedTheme = App.Current.MainWindow!.RequestedTheme,
            XamlRoot = XamlRoot,
            Style = (Style) Application.Current.Resources["DefaultContentDialogStyle"]
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

    private ContentDialogSettings settings;
}
