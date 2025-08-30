using Microsoft.UI.Xaml;

namespace SuGarToolkit.Controls.Dialogs;

public class MessageBoxBaseOptions
{
    /// <summary>
    /// ElementTheme.Default is treated as following owner window
    /// </summary>
    public ElementTheme RequestedTheme { get; set; }

    public bool SmokeBehind { get; set; }

    public FlowDirection FlowDirection { get; set; }
}
