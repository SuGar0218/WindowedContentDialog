# WindowedContentDialog

<img width="4400" height="2635" alt="WindowedContentDialogBanner" src="https://github.com/user-attachments/assets/0d984f70-32c2-450a-b129-923e08cc12b3" />

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
    OwnerWindow = App.Current.MainWindow,
    RequestedTheme = ElementTheme.Dark
};
ContentDialogResult result = await dialog.ShowAsync(modal: true);
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

Download nupkg in Github release page.
