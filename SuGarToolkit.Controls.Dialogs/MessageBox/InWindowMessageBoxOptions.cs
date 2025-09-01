using Microsoft.UI.Xaml;

namespace SuGarToolkit.Controls.Dialogs;

public class InWindowMessageBoxOptions
{
    /// <summary>
    /// Disable the content of window behind when _dialog window shows.
    /// </summary>
    public bool DisableBehind { get; set; }

    public ElementTheme RequestedTheme { get; set; }

    public FlowDirection FlowDirection { get; set; }
}
