# WindowedContentDialog

Show ContentDialog in separate window.

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
