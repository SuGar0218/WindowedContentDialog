using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;

namespace SuGarToolkit.Controls.Dialogs;

public class InWindowMessageBoxOptions
{
    /// <summary>
    /// Disable the content of window behind when dialog window shows.
    /// </summary>
    public bool DisableBehind { get; set; }

    public SystemBackdrop? SystemBackdrop { get; set; } = new MicaBackdrop();

    public ElementTheme RequestedTheme { get; set; } = ElementTheme.Default;

    public FlowDirection FlowDirection { get; set; }
}
