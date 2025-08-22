using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using SuGarToolkit.Sample.Dialogs.ViewModels;

namespace SuGarToolkit.Sample.Dialogs.Views;

public sealed partial class MainPage : Page
{
    public MainPage()
    {
        InitializeComponent();
    }

    private void ToggleNavigationPane()
    {
        RootNavigationView.IsPaneOpen = !RootNavigationView.IsPaneOpen;
    }

    private void Page_Loaded(object sender, RoutedEventArgs e)
    {
        App.Current.MainWindow!.SetTitleBar(TitleBarArea);
        TitleBarRightInset.Width = App.Current.MainWindow!.AppWindow.TitleBar.RightInset / XamlRoot.RasterizationScale;
    }

    private void RootNavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
    {
        if (args.SelectedItem is null)
            return;

        NavigationItem item = (NavigationItem) args.SelectedItem;
        ContentFrame.Navigate(item.PageType, null, args.RecommendedNavigationTransitionInfo);
    }

    private void ThemeToggleButton_Click(object sender, RoutedEventArgs e)
    {
        if (ActualTheme is ElementTheme.Dark)
        {
            App.Current.MainWindow!.RequestedTheme = ElementTheme.Light;
        }
        else
        {
            App.Current.MainWindow!.RequestedTheme = ElementTheme.Dark;
        }
    }

    private readonly MainViewModel viewModel = new();
}
