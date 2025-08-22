using Microsoft.UI.Xaml.Controls;

using SuGarToolkit.Controls.Dialogs;
using SuGarToolkit.Sample.Dialogs.Views.ContentDialogSamples;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuGarToolkit.Sample.Dialogs.ViewModels;

internal class ContentDialogSampleViewModel
{
    public ContentDialogSampleViewModel(ContentDialogSettings settings)
    {
        _settings = settings;
    }

    public List<NavigationItem> SampleItems =
    [
        new NavigationItem<WindowedContentDialogSamplePage>(nameof(WindowedContentDialog)),
        new NavigationItem<FlyoutContentDialogSamplePage>(nameof(FlyoutContentDialog)),
        new NavigationItem<MuxcContentDialogSamplePage>(nameof(ContentDialog))
    ];

    public ContentDialogSettings Settings => _settings;

    private readonly ContentDialogSettings _settings;
}
