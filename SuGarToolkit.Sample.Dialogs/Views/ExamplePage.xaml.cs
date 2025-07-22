using SuGarToolkit.Sample.Dialogs.ViewModels;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

using Windows.Foundation;
using Windows.Foundation.Collections;
using SuGarToolkit.Controls.Dialogs;

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
        MessageBoxResult result = await MessageBox.ShowAsync(
            messageBoxViewModel.IsModal,
            messageBoxViewModel.IsChild ? App.Current.MainWindow : null,
            messageBoxViewModel.Content,
            messageBoxViewModel.Title,
            messageBoxViewModel.Buttons,
            messageBoxViewModel.DefaultButton);
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
            RequestedTheme = App.Current.MainWindow!.RequestedTheme
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
}
