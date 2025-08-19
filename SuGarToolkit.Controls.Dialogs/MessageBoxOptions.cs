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

    public SystemBackdrop? SystemBackdrop { get; set; }// = new MicaBackdrop();

    public ElementTheme RequestedTheme { get; set; }// = ElementTheme.Default;

    public FlowDirection FlowDirection { get; set; }

    public bool IsTitleBarVisible { get; set; } = true;

    public bool CenterInParent { get; set; } = true;

    public static MessageBoxOptions Default => new MessageBoxOptions { SystemBackdrop = new MicaBackdrop() };
}
