using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;

namespace SuGarToolkit.Controls.Dialogs;

public class MessageBoxOptions
{
    /// <summary>
    /// Disable the content of window behind when dialog window shows.
    /// </summary>
    public bool DisableBehind { get; set; }

    public WindowedContentDialogSmokeLayerKind SmokeLayerKind { get; set; }

    public UIElement? CustomSmokeLayer { get; set; }

    public SystemBackdrop? SystemBackdrop { get; set; } = new MicaBackdrop();
    public ElementTheme RequestedTheme { get; set; } = ElementTheme.Default;

    public object? Content { get; set; }
    public string? Title { get; set; }
    public Window? OwnerWindow { get; set; }
    public FlowDirection FlowDirection { get; set; }
    public MessageBoxButtons Buttons { get; set; }
    public MessageBoxDefaultButton? DefaultButton { get; set; }

    public bool IsModal { get; set; }
    public bool IsTitleBarVisible { get; set; } = true;
    public bool CenterInParent { get; set; } = true;
}
