# WindowedContentDialog
Show ContentDialog in a separate window in WinUI 3.

<img width="4400" height="2635" alt="WindowedContentDialogBanner" src="https://github.com/user-attachments/assets/8a4d29f2-59c3-4a7e-b5ed-7cd9c86c64f6" />

To use windowed ContentDialog, just copy these files:
- Themes\Generics.xaml
- ContentDialogContent.cs
- ContentDialogWindow.cs
- MessageBox.cs
- WindowedContentDialog.cs

Generics.xaml, ContentDialogContent.cs, ContentDialogWindow.cs are implements.

WindowedContentDialog.cs is a wrapper that allows you to use like ContentDialog in WinUI 3.

``` C#
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
```

MessageBox.cs is a wrapper that allows you to use like MessageBox in WPF.

``` C#
MessageBoxResult result = await MessageBox.ShowAsync(
    messageBoxViewModel.IsModal,
    messageBoxViewModel.IsChild ? App.Current.MainWindow : null,
    messageBoxViewModel.Content,
    messageBoxViewModel.Title,
    messageBoxViewModel.Buttons,
    messageBoxViewModel.DefaultButton);
MessageBoxResultBox.Text = result.ToString();
```
