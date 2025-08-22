using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

using SuGarToolkit.Controls.Dialogs;
using SuGarToolkit.Sample.Dialogs.ViewModels;

namespace SuGarToolkit.Sample.Dialogs.Views.MessageBoxSamples;

public sealed partial class MuxcMessageBoxSamplePage : Page
{
    public MuxcMessageBoxSamplePage()
    {
        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        settings = (MessageBoxSettings) e.Parameter;
        base.OnNavigatedTo(e);
    }

    private async void ShowMuxcMessageBox()
    {
        MessageBoxResult result = await InWindowMessageBox.ShowAsync(
            this,
            settings.Content,
            settings.Title,
            settings.Buttons,
            settings.Image,
            settings.DefaultButton,
            new InWindowMessageBoxOptions
            {
                //DisableBehind = settings.DisableBehind,
                RequestedTheme = settings.RequestedTheme is ElementTheme.Default ? ActualTheme : settings.RequestedTheme,
            });
        MessageBoxResultBox.Text = result.ToString();
    }

    private MessageBoxSettings settings;
}
