using Microsoft.UI.Xaml.Controls;

namespace SuGarToolkit.Sample.Dialogs.Views;

public sealed partial class ContentDialogFlyoutSamplePage : Page
{
    public ContentDialogFlyoutSamplePage()
    {
        InitializeComponent();
    }

    internal const string XamlCode = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<Button Content=""ContentDialogFlyout"" Style=""{ThemeResource AccentButtonStyle}"">
    <Button.Flyout>
        <dialogs:ContentDialogFlyout
            CloseButtonText=""Close Button""
            DefaultButton=""Primary""
            DialogTitle=""SampleContentDialogWindow""
            PrimaryButtonText=""Primary Button""
            RequestedTheme=""{x:Bind ActualTheme}""
            SecondaryButtonText=""Secondary Button"">
            <StackPanel>
                <CheckBox Content=""Using"" />
                <CheckBox Content=""ContentDialogFlyout"" />
                <CheckBox Content=""in XAML"" />
            </StackPanel>
        </dialogs:ContentDialogFlyout>
    </Button.Flyout>
</Button>";
}
