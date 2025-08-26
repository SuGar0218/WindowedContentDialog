using Microsoft.UI.Xaml;

namespace SuGarToolkit.Controls.Dialogs;

public class MessageBoxBaseOptions
{
    /// <summary>
    /// Disable the content of window behind when _dialog window shows.
    /// </summary>
    public bool DisableBehind { get; set; }

    public ContentDialogSmokeLayerKind SmokeLayerKind { get; set; }

    public UIElement? CustomSmokeLayer { get; set; }

    /// <summary>
    /// ElementTheme.Default is treated as following owner window
    /// </summary>
    public ElementTheme RequestedTheme { get; set; }

    public FlowDirection FlowDirection { get; set; }
}
