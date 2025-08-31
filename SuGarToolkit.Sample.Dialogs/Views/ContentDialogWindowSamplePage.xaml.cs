using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

namespace SuGarToolkit.Sample.Dialogs.Views;

public sealed partial class ContentDialogWindowSamplePage : Page
{
    public ContentDialogWindowSamplePage()
    {
        InitializeComponent();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        new SampleContentDialogWindow
        {
            RequestedTheme = ActualTheme,
            SystemBackdrop = new MicaBackdrop()
        }
        .OpenAfterLoaded();
    }
}
