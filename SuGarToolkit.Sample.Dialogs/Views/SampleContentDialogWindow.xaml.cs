using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;

using SuGarToolkit.Controls.Dialogs;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

using Windows.Foundation;
using Windows.Foundation.Collections;

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
