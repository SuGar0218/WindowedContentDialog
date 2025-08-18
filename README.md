# WindowedContentDialog

<img width="2891" height="2813" alt="MessageBox_1 1 0" src="https://github.com/user-attachments/assets/311a43c4-f55e-4d12-a82a-80f0078a80bf" />

Show ContentDialog in separate window.

## Use similarly to ContentDialog in WinUI 3

``` C#
WindowedContentDialog dialog = new()
{
    Title = "YourTitle",
    Content = "YourContent",
    PrimaryButtonText = "YourPrimaryButtonText",
    SecondaryButtonText = "YourSecondaryButtonText",
    CloseButtonText = "YourCloseButtonText",
    DefaultButton = ContentDialogButton.Primary,
    OwnerWindow = App.Current.MainWindow
};
ContentDialogResult result = await dialog.ShowAsync();
```

If you want to prevent dialog from closing after buttons clicked, please handle click event and set ```e.Cancel = true``` where ```e``` is ```ContentDialogWindowButtonClickEventArgs```.

```C#
dialog.PrimaryButtonClick += (o, e) => e.Cancel = true;
```

## Use similarly to MessageBox in WPF or WinForm

```C#
MessageBoxResult result = await MessageBox.ShowAsync(
    modal: true,
    App.Current.MainWindow,
    "YourMessage",
    "YourTitle",
    MessageBoxButtons.YesNoCancel,
    MessageBoxDefaultButton.Button3);
```

## Use WindowedContentDialog in your projects

Download nupkg from releases page or https://www.nuget.org/packages/SuGarToolkit.Controls.Dialogs.
