using ContentDialogWindow.Views;

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
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;

using Windows.Foundation;
using Windows.Foundation.Collections;

namespace ContentDialogWindow;

public sealed partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        ExtendsContentIntoTitleBar = true;
    }

    private async void Button_Click(object sender, RoutedEventArgs e)
    {
        //await MessageBox.ShowAsync(modal: true, this, "message1", "title", MessageBoxButtons.OKCancel);
        //await MessageBox.ShowAsync(modal: false, this, "message2", "title", MessageBoxButtons.OKCancel);

        WindowedContentDialog dialog = new()
        {
            Title = Title,
            Content = new TextBox(),
            OwnerWindow = this,
        };

        ContentDialogResult result = await dialog.ShowAsync(true);
        await dialog.ShowAsync(false);
    }

    private void Frame_Loaded(object sender, RoutedEventArgs e)
    {
        Frame frame = (Frame) sender;
        frame.Navigate(typeof(ExamplePage));
    }
}
