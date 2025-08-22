using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

using SuGarToolkit.Sample.Dialogs.ViewModels;

using System;

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
            RequestedTheme = settings.RequestedTheme is ElementTheme.Default ? ActualTheme : settings.RequestedTheme,
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
