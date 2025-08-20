namespace SuGarToolkit.Controls.Dialogs;

/// <summary>
/// 点击窗口化的 ContentDialog 自带的底部按钮事件的信息，若要防止点击后关闭窗口，请处理此参数，设置 Cancel 为 true
/// </summary>
public sealed class ContentDialogFlyoutButtonClickEventArgs
{
    /// <summary>
    /// 该值可取消对话框的关闭。如果为 true，则取消默认行为。
    /// </summary>
    public bool Cancel { get; set; }
}
