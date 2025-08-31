using SuGarToolkit.Controls.Dialogs;

namespace SuGarToolkit.Sample.Dialogs.Views;

public sealed partial class SampleContentDialogWindow : ContentDialogWindow
{
    public SampleContentDialogWindow()
    {
        InitializeComponent();
    }

    internal const string XamlCode = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<dialogs:ContentDialogWindow
    xmlns:dialogs=""using:SuGarToolkit.Controls.Dialogs""
    Title=""SampleContentDialogWindow""
    CloseButtonText=""Close Button""
    DefaultButton=""Primary""
    PrimaryButtonText=""Primary Button""
    SecondaryButtonText=""Secondary Button""
    ...>

    <dialogs:ContentDialogWindow.SystemBackdrop>
        <MicaBackdrop />
    </dialogs:ContentDialogWindow.SystemBackdrop>

    <StackPanel>
        <CheckBox Content=""Using"" />
        <CheckBox Content=""ContentDialogWindow"" />
        <CheckBox Content=""in XAML"" />
    </StackPanel>
    
</dialogs:ContentDialogWindow>";
}
