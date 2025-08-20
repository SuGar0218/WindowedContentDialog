using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

using SuGarToolkit.Controls.Dialogs;
using SuGarToolkit.Sample.Dialogs.ViewModels;

using System;
using System.Collections.Generic;
using System.Text;

using Windows.UI;

namespace SuGarToolkit.Sample.Dialogs.Views;

public sealed partial class ExamplePage : Page
{
    public ExamplePage()
    {
        InitializeComponent();
    }

    private readonly MessageBoxLikeExampleViewModel messageBoxViewModel = new();

    private readonly ContentDialogLikeExampleViewModel contentDialogViewModel = new();

    private readonly MessageBoxButtons[] messageBoxButtons = Enum.GetValues<MessageBoxButtons>();

    private readonly MessageBoxDefaultButton[] messageBoxDefaultButtons = Enum.GetValues<MessageBoxDefaultButton>();

    private readonly MessageBoxIcon[] messageBoxImages = Enum.GetValues<MessageBoxIcon>();

    private readonly List<ContentDialogButton> contentDialogButtons =
    [
        ContentDialogButton.None,
        ContentDialogButton.Primary,
        ContentDialogButton.Secondary,
        ContentDialogButton.Close
    ];

    private async void ShowMessageBox()
    {
        if (messageBoxViewModel.SmokeLayerKind is WindowedContentDialogSmokeLayerKind.Custom && App.Current.MainWindow is not null)
        {
            SizeToWindow(LazyCustomSmokeLayer.Value, App.Current.MainWindow);
        }
        MessageBoxResult result = await MessageBox.ShowAsync(
            messageBoxViewModel.IsModal,
            messageBoxViewModel.IsChild ? App.Current.MainWindow : null,
            messageBoxViewModel.Content,
            messageBoxViewModel.Title,
            messageBoxViewModel.Buttons,
            messageBoxViewModel.Image,
            messageBoxViewModel.DefaultButton,
            new MessageBoxOptions
            {
                IsTitleBarVisible = messageBoxViewModel.IsTitleBarVisible,

                DisableBehind = messageBoxViewModel.DisableBehind,
                SmokeLayerKind = messageBoxViewModel.SmokeLayerKind,
                CustomSmokeLayer = LazyCustomSmokeLayer.Value,

                RequestedTheme = ActualTheme,
                SystemBackdrop = messageBoxViewModel.BackdropType switch
                {
                    BuiltInSystemBackdropType.Mica => new MicaBackdrop { Kind = Microsoft.UI.Composition.SystemBackdrops.MicaKind.Base },
                    BuiltInSystemBackdropType.MicaAlt => new MicaBackdrop { Kind = Microsoft.UI.Composition.SystemBackdrops.MicaKind.BaseAlt },
                    BuiltInSystemBackdropType.Arcylic => new DesktopAcrylicBackdrop(),
                    _ => null
                }
            });
        MessageBoxResultBox.Text = result.ToString();
    }

    private async void ShowInWindowMessageBox()
    {
        MessageBoxResult result = await InWindowMessageBox.ShowAsync(
            this,
            messageBoxViewModel.Content,
            messageBoxViewModel.Title,
            messageBoxViewModel.Buttons,
            messageBoxViewModel.Image,
            messageBoxViewModel.DefaultButton,
            new InWindowMessageBoxOptions
            {
                DisableBehind = messageBoxViewModel.DisableBehind,
                RequestedTheme = ActualTheme
            });
        MessageBoxResultBox.Text = result.ToString();
    }

    private async void ShowWindowedContentDialog()
    {
        if (contentDialogViewModel.SmokeLayerKind is WindowedContentDialogSmokeLayerKind.Custom && App.Current.MainWindow is not null)
        {
            SizeToWindow(LazyCustomSmokeLayer.Value, App.Current.MainWindow);
        }
        WindowedContentDialog dialog = new()
        {
            WindowTitle = contentDialogViewModel.Title,
            Title = contentDialogViewModel.Title,
            Content = !string.IsNullOrEmpty(contentDialogViewModel.Message) ? contentDialogViewModel.Message : new LoremIpsumPage(),

            PrimaryButtonText = contentDialogViewModel.PrimaryButtonText,
            SecondaryButtonText = contentDialogViewModel.SecondaryButtonText,
            CloseButtonText = contentDialogViewModel.CloseButtonText,
            DefaultButton = contentDialogViewModel.DefaultButton,

            OwnerWindow = contentDialogViewModel.IsChild ? App.Current.MainWindow : null,
            IsTitleBarVisible = contentDialogViewModel.IsTitleBarVisible,

            DisableBehind = contentDialogViewModel.DisableBehind,
            SmokeLayerKind = contentDialogViewModel.SmokeLayerKind,
            CustomSmokeLayer = LazyCustomSmokeLayer.Value,

            RequestedTheme = App.Current.MainWindow!.RequestedTheme,
            SystemBackdrop = contentDialogViewModel.BackdropType switch
            {
                BuiltInSystemBackdropType.Mica => new MicaBackdrop { Kind = Microsoft.UI.Composition.SystemBackdrops.MicaKind.Base },
                BuiltInSystemBackdropType.MicaAlt => new MicaBackdrop { Kind = Microsoft.UI.Composition.SystemBackdrops.MicaKind.BaseAlt },
                BuiltInSystemBackdropType.Arcylic => new DesktopAcrylicBackdrop(),
                _ => null
            }
        };
        if (!contentDialogViewModel.ClickPrimaryButtonToClose)
        {
            dialog.PrimaryButtonClick += (o, e) => e.Cancel = true;
        }
        if (!contentDialogViewModel.ClickSecondaryButtonToClose)
        {
            dialog.SecondaryButtonClick += (o, e) => e.Cancel = true;
        }
        ContentDialogResult result = await dialog.ShowAsync(contentDialogViewModel.IsModal);
        ContentDialogResultBox.Text = result.ToString();
    }

    private async void ShowInWindowContentDialog()
    {
        ContentDialog dialog = new()
        {
            Title = contentDialogViewModel.Title,
            Content = !string.IsNullOrEmpty(contentDialogViewModel.Message) ? contentDialogViewModel.Message : new LoremIpsumPage(),
            PrimaryButtonText = contentDialogViewModel.PrimaryButtonText,
            SecondaryButtonText = contentDialogViewModel.SecondaryButtonText,
            CloseButtonText = contentDialogViewModel.CloseButtonText,
            DefaultButton = contentDialogViewModel.DefaultButton,
            RequestedTheme = App.Current.MainWindow!.RequestedTheme,
            XamlRoot = XamlRoot,
            Style = (Style) Application.Current.Resources["DefaultContentDialogStyle"]
        };
        if (!contentDialogViewModel.ClickPrimaryButtonToClose)
        {
            dialog.PrimaryButtonClick += (o, e) => e.Cancel = true;
        }
        if (!contentDialogViewModel.ClickSecondaryButtonToClose)
        {
            dialog.SecondaryButtonClick += (o, e) => e.Cancel = true;
        }
        ContentDialogResult result = await dialog.ShowAsync();
        ContentDialogResultBox.Text = result.ToString();
    }

    private async void ShowContentDialogFlyout()
    {
        FlyoutContentDialog dialog = new()
        {
            Title = contentDialogViewModel.Title,
            Content = !string.IsNullOrEmpty(contentDialogViewModel.Message) ? contentDialogViewModel.Message : new LoremIpsumPage(),
            PrimaryButtonText = contentDialogViewModel.PrimaryButtonText,
            SecondaryButtonText = contentDialogViewModel.SecondaryButtonText,
            CloseButtonText = contentDialogViewModel.CloseButtonText,
            DefaultButton = contentDialogViewModel.DefaultButton,

            DisableBehind = contentDialogViewModel.DisableBehind,
            SmokeLayerKind = contentDialogViewModel.SmokeLayerKind,
            CustomSmokeLayer = LazyCustomSmokeLayer.Value,

            ShouldConstrainToRootBounds = false,
            PlacementTarget = ShowContentDialogButtonArea,

            RequestedTheme = App.Current.MainWindow!.RequestedTheme,
            SystemBackdrop = contentDialogViewModel.BackdropType switch
            {
                BuiltInSystemBackdropType.Mica => new MicaBackdrop { Kind = Microsoft.UI.Composition.SystemBackdrops.MicaKind.Base },
                BuiltInSystemBackdropType.MicaAlt => new MicaBackdrop { Kind = Microsoft.UI.Composition.SystemBackdrops.MicaKind.BaseAlt },
                BuiltInSystemBackdropType.Arcylic => new DesktopAcrylicBackdrop(),
                _ => null
            }
        };
        ContentDialogResult result = await dialog.ShowAsync();
        ContentDialogResultBox.Text = result.ToString();
    }

    private TextBox ContentDialogContentTextBox
    {
        get
        {
            TextBox textBox = new()
            {
                AcceptsReturn = true,
                FontFamily = new FontFamily("Consolas"),
                Text = ContentDialogTextBoxContent
            };
            ScrollViewer.SetHorizontalScrollBarVisibility(textBox, ScrollBarVisibility.Auto);
            ScrollViewer.SetVerticalScrollBarVisibility(textBox, ScrollBarVisibility.Auto);
            return textBox;
        }
    }

    internal static readonly BuiltInSystemBackdropType[] systemBackdropTypes = Enum.GetValues<BuiltInSystemBackdropType>();

    internal static readonly WindowedContentDialogSmokeLayerKind[] behindOverlayTypes =
    [
        WindowedContentDialogSmokeLayerKind.None,
        WindowedContentDialogSmokeLayerKind.Darken,
        WindowedContentDialogSmokeLayerKind.Custom
    ];

    private readonly Lazy<FrameworkElement> LazyCustomSmokeLayer = new(() => new Border
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
    });

    private string MessageBoxContent
    {
        get =>
@$"MessageBoxResult result = await MessageBox.ShowAsync(
    {messageBoxViewModel.IsModal},
    {(messageBoxViewModel.IsChild ? "App.Current.MainWindow" : "null")},
    {messageBoxViewModel.Content},
    {messageBoxViewModel.Title},
    {messageBoxViewModel.Buttons},
    {messageBoxViewModel.DefaultButton});
MessageBoxResultBox.Text = result.ToString();";
    }

    private string ContentDialogTextBoxContent
    {
        get
        {
            StringBuilder stringBuilder = new();
            stringBuilder.Append(
@$"WindowedContentDialog dialog = new()
{{
    Title = {contentDialogViewModel.Title},
    Content = ""YourContent"",
    PrimaryButtonText = {contentDialogViewModel.PrimaryButtonText},
    SecondaryButtonText = {contentDialogViewModel.SecondaryButtonText},
    CloseButtonText = {contentDialogViewModel.CloseButtonText},
    DefaultButton = {contentDialogViewModel.DefaultButton},
    OwnerWindow = {(contentDialogViewModel.IsChild ? App.Current.MainWindow : null)}
}};");
            if (!contentDialogViewModel.ClickPrimaryButtonToClose)
            {
                stringBuilder.AppendLine().AppendLine().Append(
@$"dialog.PrimaryButtonClick += (o, e) =>
{{
    e.Cancel = true;
}};");
            }
            if (!contentDialogViewModel.ClickSecondaryButtonToClose)
            {
                stringBuilder.AppendLine().AppendLine().Append(
@$"dialog.SecondaryButtonClick += (o, e) =>
{{
    e.Cancel = true;
}};");
            }
            stringBuilder.AppendLine().AppendLine().Append("ContentDialogResultBox.Text = result.ToString();");
            return stringBuilder.ToString();
        }
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

    private void Page_Loaded(object sender, RoutedEventArgs e)
    {
        TitleBarExtraButtonsArea.Height = App.Current.MainWindow!.AppWindow.TitleBar.Height / XamlRoot.RasterizationScale;
        TitleBarExtraButtonsArea.Margin = new Thickness
        {
            Right = App.Current.MainWindow!.AppWindow.TitleBar.RightInset / XamlRoot.RasterizationScale
        };
    }

    private static void SizeToWindow(FrameworkElement element, Window window)
    {
        element.Width = window.Content.XamlRoot.Size.Width;
        element.Height = window.Content.XamlRoot.Size.Height;
    }
}
