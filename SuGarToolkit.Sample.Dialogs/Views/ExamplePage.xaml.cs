using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

using SuGarToolkit.Controls.Dialogs;
using SuGarToolkit.Sample.Dialogs.ViewModels;

using System;
using System.Collections.Generic;
using System.Text;

namespace SuGarToolkit.Sample.Dialogs.Views;

public sealed partial class ExamplePage : Page
{
    public ExamplePage()
    {
        InitializeComponent();
    }

    private readonly MessageBoxLikeExampleViewModel messageBoxViewModel = new();
    private readonly ContentDialogLikeExampleViewModel contentDialogViewModel = new();

    private readonly List<MessageBoxButtons> messageBoxButtons =
    [
        MessageBoxButtons.OK,
        MessageBoxButtons.OKCancel,
        MessageBoxButtons.AbortRetryIgnore,
        MessageBoxButtons.YesNoCancel,
        MessageBoxButtons.YesNo,
        MessageBoxButtons.RetryCancel
    ];

    private readonly List<MessageBoxDefaultButton> messageBoxDefaultButtons =
    [
        MessageBoxDefaultButton.Button1,
        MessageBoxDefaultButton.Button2,
        MessageBoxDefaultButton.Button3
    ];

    private readonly List<ContentDialogButton> contentDialogButtons =
    [
        ContentDialogButton.None,
        ContentDialogButton.Primary,
        ContentDialogButton.Secondary,
        ContentDialogButton.Close
    ];

    private async void ShowMessageBoxButton_Click(object sender, RoutedEventArgs e)
    {
        MessageBox.SystemBackdrop = messageBoxViewModel.BackdropType switch
        {
            BuiltInSystemBackdropType.Mica => new MicaBackdrop { Kind = Microsoft.UI.Composition.SystemBackdrops.MicaKind.Base },
            BuiltInSystemBackdropType.MicaAlt => new MicaBackdrop { Kind = Microsoft.UI.Composition.SystemBackdrops.MicaKind.BaseAlt },
            BuiltInSystemBackdropType.Arcylic => new DesktopAcrylicBackdrop(),
            _ => null
        };
        MessageBoxResult result = await MessageBox.ShowAsync(
            messageBoxViewModel.IsModal,
            messageBoxViewModel.IsChild ? App.Current.MainWindow : null,
            messageBoxViewModel.Content,
            messageBoxViewModel.Title,
            messageBoxViewModel.Buttons,
            messageBoxViewModel.DefaultButton,
            messageBoxViewModel.IsTitleBarVisible);
        MessageBoxResultBox.Text = result.ToString();
    }

    private async void ShowContentDialogButton_Click(object sender, RoutedEventArgs e)
    {
        WindowedContentDialog dialog = new()
        {
            Title = contentDialogViewModel.Title,
            Content = ContentDialogContentTextBox,
            PrimaryButtonText = contentDialogViewModel.PrimaryButtonText,
            SecondaryButtonText = contentDialogViewModel.SecondaryButtonText,
            CloseButtonText = contentDialogViewModel.CloseButtonText,
            DefaultButton = contentDialogViewModel.DefaultButton,
            OwnerWindow = contentDialogViewModel.IsChild ? App.Current.MainWindow : null,
            RequestedTheme = App.Current.MainWindow!.RequestedTheme,
            IsTitleBarVisible = contentDialogViewModel.IsTitleBarVisible,
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

    internal static readonly BuiltInSystemBackdropType[] backdropTypes = Enum.GetValues<BuiltInSystemBackdropType>();

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

        //await MessageBox.ShowAsync(modal: true, App.Current.MainWindow, "嗨，别来无恙啊！", "与君初相识，犹如故人归");
    }
}
