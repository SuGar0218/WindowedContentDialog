using Microsoft.UI.Xaml.Controls;

using SuGarToolkit.Controls.Dialogs;
using SuGarToolkit.Sample.Dialogs.Views;
using SuGarToolkit.Sample.Dialogs.Views.ContentDialogSamples;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuGarToolkit.Sample.Dialogs.ViewModels;

internal class MainViewModel
{
    public List<NavigationItem> SampleList =
    [
        new NavigationItem<ContentDialogSamplesPage>(nameof(ContentDialog)),
        new NavigationItem<MessageBoxSamplesPage>(nameof(MessageBox))
    ];
}
