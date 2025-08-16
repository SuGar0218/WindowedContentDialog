using SuGarToolkit.Controls.Dialogs;

namespace SuGarToolkit.Sample.Dialogs.ViewModels;

public partial class MessageBoxLikeExampleViewModel
{
    public string Title { get; set; } = "MessageBox";
    public string Content { get; set; } = "Lorem ipsum dolor sit amet.";
    public MessageBoxButtons Buttons { get; set; } = MessageBoxButtons.OK;
    public MessageBoxDefaultButton DefaultButton { get; set; } = MessageBoxDefaultButton.Button1;
    public bool IsChild { get; set; } = true;
    public bool IsModal { get; set; } = true;
    public bool IsTitleBarVisible { get; set; } = true;
    public BuiltInSystemBackdropType BackdropType { get; set; } = BuiltInSystemBackdropType.Mica;

    /// <summary>
    /// Disable the content of window behind when dialog window shows.
    /// </summary>
    public bool DisableBehind { get; set; }

    public WindowedContentDialogSmokeLayerKind SmokeLayerKind { get; set; } = WindowedContentDialogSmokeLayerKind.Darken;

    //public UIElement? CustomSmokeLayer { get; set; }
}
