using Microsoft.UI.Xaml.Controls;

using SuGarToolkit.Controls.Dialogs;
using SuGarToolkit.Sample.Dialogs.Views;

using System.Collections.Generic;

namespace SuGarToolkit.Sample.Dialogs.ViewModels;

internal class MainViewModel
{
    public List<NavigationItem> SampleList =
    [
        new NavigationItem<ContentDialogSamplesPage>(nameof(ContentDialog)),
        new NavigationItem<MessageBoxSamplesPage>(nameof(MessageBox))
    ];
}
