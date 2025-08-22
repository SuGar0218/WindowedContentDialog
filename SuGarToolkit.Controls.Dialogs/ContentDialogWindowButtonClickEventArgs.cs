namespace SuGarToolkit.Controls.Dialogs;

/// <summary>
/// 点击窗口化的 ContentDialog 自带的底部按钮事件的信息，若要防止点击后关闭窗口，请处理此参数，设置 ShouldCloseDialog 为 true
/// </summary>
public sealed class ContentDialogWindowButtonClickEventArgs
{
    /// <summary>
    /// Whether to cancel closing dialog after button clicked.
    /// </summary>
    public bool Cancel { get; set; }
}
