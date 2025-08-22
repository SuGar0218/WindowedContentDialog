using SuGarToolkit.Controls.Dialogs;
using SuGarToolkit.Sample.Dialogs.Views.MessageBoxSamples;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuGarToolkit.Sample.Dialogs.ViewModels;

internal class MessageBoxSampleViewModel
{
    public MessageBoxSampleViewModel(MessageBoxSettings settings)
    {
        _settings = settings;
    }

    public List<NavigationItem> SampleItems =
    [
        new NavigationItem<WindowedMessageBoxSamplePage>(nameof(MessageBox)),
        new NavigationItem<FlyoutMessageBoxSamplePage>(nameof(FlyoutMessageBox)),
        new NavigationItem<MuxcMessageBoxSamplePage>(nameof(InWindowMessageBox))
    ];

    public MessageBoxSettings Settings => _settings;

    private readonly MessageBoxSettings _settings = new();
}
