using ContentDialogWindow.ViewModels;

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
using System.Threading.Tasks;

using Windows.Foundation;
using Windows.Foundation.Collections;

namespace ContentDialogWindow.Views;

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
            modal: messageBoxViewModel.IsModal,
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
            Content = LazyContentDialogContentTextBox.Value,
            PrimaryButtonText = contentDialogViewModel.PrimaryButtonText,
            SecondaryButtonText = contentDialogViewModel.SecondaryButtonText,
            CloseButtonText = contentDialogViewModel.CloseButtonText,
            DefaultButton = contentDialogViewModel.DefaultButton,
            OwnerWindow = contentDialogViewModel.IsChild ? App.Current.MainWindow : null
        };
        ContentDialogResult result = await dialog.ShowAsync(contentDialogViewModel.IsModal);
        ContentDialogResultBox.Text = result.ToString();
    }

    private static readonly Lazy<TextBox> LazyContentDialogContentTextBox = new(() =>
    {
        TextBox textBox = new TextBox
        {
            AcceptsReturn = true,
            FontFamily = new FontFamily("Consolas"),
            Text = """
            MessageBoxResult result = await MessageBox.ShowAsync(
                modal: messageBoxViewModel.IsModal,
                messageBoxViewModel.IsChild ? App.Current.MainWindow : null,
                messageBoxViewModel.Content,
                messageBoxViewModel.Title,
                messageBoxViewModel.Buttons,
                messageBoxViewModel.DefaultButton);
            MessageBoxResultBox.Text = result.ToString();
            """
        };
        ScrollViewer.SetHorizontalScrollBarVisibility(textBox, ScrollBarVisibility.Auto);
        ScrollViewer.SetVerticalScrollBarVisibility(textBox, ScrollBarVisibility.Auto);
        return textBox;
    });
}
