# WindowedContentDialog and MessageBox

**WindowedContentDialog**: Show ContentDialog in separate window.

**MessageBox**: Show MessageBox in WinUI 3 Style.

## Using in {code-behind}

### Using similarly to ContentDialog in WinUI 3

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

### Using similarly to MessageBox in WPF or WinForm

```C#
MessageBoxResult result = await MessageBox.ShowAsync(
    modal: true,
    App.Current.MainWindow,
    "YourMessage",
    "YourTitle",
    MessageBoxButtons.YesNoCancel,
    MessageBoxDefaultButton.Button3);
```


## Using in < XAML />

``` xml
xmlns:dialogs="using:SuGarToolkit.Controls.Dialogs"
```

### Using ```WindowedContentDialog``` in ```<Page.Resources>```

``` xml
<Page.Resources>
    <dialogs:WindowedContentDialog
        x:Key="XamlWindowedContentDialog"
        x:Name="XamlWindowedContentDialog"
        CloseButtonText="{x:Bind settings.CloseButtonText, Mode=OneWay}"
        DefaultButton="{x:Bind settings.DefaultButton, Mode=OneWay}"
        IsModal="{x:Bind settings.IsModal, Mode=OneWay}"
        IsPrimaryButtonEnabled="{x:Bind settings.IsPrimaryButtonEnabled, Mode=OneWay}"
        IsSecondaryButtonEnabled="{x:Bind settings.IsSecondaryButtonEnabled, Mode=OneWay}"
        OwnerWindow="{x:Bind app:App.Current.MainWindow}"
        PrimaryButtonText="{x:Bind settings.PrimaryButtonText, Mode=OneWay}"
        SecondaryButtonText="{x:Bind settings.SecondaryButtonText, Mode=OneWay}"
        SmokeBehind="{x:Bind settings.SmokeBehind, Mode=OneWay}">

        <dialogs:WindowedContentDialog.SystemBackdrop>
            <MicaBackdrop />
        </dialogs:WindowedContentDialog.SystemBackdrop>

        <dialogs:WindowedContentDialog.Title>
            <dialogs:MessageBoxHeader Icon="Information" Text="{x:Bind settings.Title, Mode=OneWay}" />
        </dialogs:WindowedContentDialog.Title>

        <StackPanel>
            <CheckBox Content="Lorem" IsThreeState="True" />
            <CheckBox Content="Ipsum" IsThreeState="True" />
            <CheckBox Content="Dolor" IsThreeState="True" />
            <CheckBox Content="Sit" IsThreeState="True" />
            <CheckBox Content="Amet" IsThreeState="True" />
        </StackPanel>
    </dialogs:WindowedContentDialog>
</Page.Resources>
```

### Using ```FlyoutContentDialog``` in ```<Page.Resources>```

``` xml
<Page.Resources>
    <dialogs:FlyoutContentDialog
        x:Key="XamlFlyoutContentDialog"
        x:Name="XamlFlyoutContentDialog"
        CloseButtonText="{x:Bind settings.CloseButtonText, Mode=OneWay}"
        IsPrimaryButtonEnabled="{x:Bind settings.IsPrimaryButtonEnabled, Mode=OneWay}"
        IsSecondaryButtonEnabled="{x:Bind settings.IsSecondaryButtonEnabled, Mode=OneWay}"
        PlacementTarget="{x:Bind ShowContentDialogButton}"
        PrimaryButtonText="{x:Bind settings.PrimaryButtonText, Mode=OneWay}"
        SecondaryButtonText="{x:Bind settings.SecondaryButtonText, Mode=OneWay}"
        SmokeBehind="{x:Bind settings.SmokeBehind, Mode=OneWay}">

        <dialogs:FlyoutContentDialog.Title>
            <dialogs:MessageBoxHeader Icon="Information" Text="{x:Bind settings.Title, Mode=OneWay}" />
        </dialogs:FlyoutContentDialog.Title>

        <StackPanel>
            <CheckBox Content="Lorem" IsThreeState="True" />
            <CheckBox Content="Ipsum" IsThreeState="True" />
            <CheckBox Content="Dolor" IsThreeState="True" />
            <CheckBox Content="Sit" IsThreeState="True" />
            <CheckBox Content="Amet" IsThreeState="True" />
        </StackPanel>
    </dialogs:FlyoutContentDialog>
</Page.Resources>
```

### Using ```ContentDialogWindow``` in XAML

``` xml
<dialogs:ContentDialogWindow
    x:Class="SuGarToolkit.Sample.Dialogs.Views.SampleContentDialogWindow"
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
    xmlns:dialogs="using:SuGarToolkit.Controls.Dialogs"
    xmlns:local="using:SuGarToolkit.Sample.Dialogs.Views"
    xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
    Title="Sample ContentDialogWindow"
    CloseButtonText="Close Button"
    DefaultButton="Primary"
    DialogTitle="{x:Bind Title}"
    PrimaryButtonText="Primary Button"
    SecondaryButtonText="Secondary Button"
    mc:Ignorable="d">

    <dialogs:ContentDialogWindow.SystemBackdrop>
        <MicaBackdrop />
    </dialogs:ContentDialogWindow.SystemBackdrop>

    <StackPanel>
        <CheckBox Content="Using" IsThreeState="True" />
        <CheckBox Content="ContentDialogWindow" IsChecked="True" />
        <CheckBox Content="in XAML" IsThreeState="True" />
        <TextBox
            AcceptsReturn="True"
            FontFamily="Consolas"
            IsReadOnly="True"
            ScrollViewer.HorizontalScrollBarVisibility="Auto"
            ScrollViewer.VerticalScrollBarVisibility="Auto"
            Text="{x:Bind local:SampleContentDialogWindow.XamlCode}" />
    </StackPanel>

</dialogs:ContentDialogWindow>
```

### Using ```ContentDialogFlyout``` as Button.Flyout

``` xml
<Button Content="ContentDialogFlyout">
    <Button.Flyout>
        <dialogs:ContentDialogFlyout
            CloseButtonText="Close Button"
            DefaultButton="Primary"
            DialogTitle="Sample ContentDialogFlyout"
            PrimaryButtonText="Primary Button"
            RequestedTheme="{x:Bind ActualTheme}"
            SecondaryButtonText="Secondary Button">
            <StackPanel>
                <CheckBox Content="Using" IsThreeState="True" />
                <CheckBox Content="ContentDialogFlyout" IsChecked="True" />
                <CheckBox Content="in XAML" IsThreeState="True" />
                <TextBox
                    AcceptsReturn="True"
                    FontFamily="Consolas"
                    IsReadOnly="True"
                    ScrollViewer.HorizontalScrollBarVisibility="Auto"
                    ScrollViewer.VerticalScrollBarVisibility="Auto"
                    Text="{x:Bind local:ContentDialogFlyoutSamplePage.XamlCode}" />
            </StackPanel>
        </dialogs:ContentDialogFlyout>
    </Button.Flyout>
</Button>
```
