using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

using SuGarToolkit.Sample.Dialogs.ViewModels;

using Windows.UI;

namespace SuGarToolkit.Sample.Dialogs.Views;

public sealed partial class ContentDialogSamplesPage : Page
{
    public ContentDialogSamplesPage()
    {
        InitializeComponent();
    }

    private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
    {
        if (args.SelectedItem is null)
            return;

        NavigationItem item = (NavigationItem) args.SelectedItem;
        ContentFrame.Navigate(item.PageType, viewModel.Settings, args.RecommendedNavigationTransitionInfo);
    }

    private readonly ContentDialogSampleViewModel viewModel = new(new ContentDialogSettings());

    internal static FrameworkElement CustomSmokeLayer => field ??= new Border
    {
        Background = new SolidColorBrush((Color) Application.Current.Resources["SystemAccentColorDark2"]) { Opacity = 0.618 },
        Child = new TextBlock
        {
            Text = "Dialog is opened",
            TextWrapping = TextWrapping.Wrap,
            Foreground = new SolidColorBrush(Colors.White),
            VerticalAlignment = VerticalAlignment.Bottom,
            HorizontalAlignment = HorizontalAlignment.Center,
            Style = (Style) Application.Current.Resources["TitleTextBlockStyle"]
        }
    };
}
