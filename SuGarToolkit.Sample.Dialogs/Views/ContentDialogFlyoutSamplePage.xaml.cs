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

using Windows.Foundation;
using Windows.Foundation.Collections;

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
