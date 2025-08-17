namespace SuGarToolkit.Controls.Dialogs;

public enum MessageBoxResult
{
    None = 0,
    // 从对话框返回了 Nothing。 这表明有模式对话框继续运行。

    OK = 1,
    // 对话框的返回值是 OK（通常从标签为“确定”的按钮发送）。

    Cancel = 2,
    // 对话框的返回值是 Cancel（通常从标签为“取消”的按钮发送）。

    Abort = 3,
    // 对话框的返回值是 Abort（通常从标签为“中止”的按钮发送）。

    Retry = 4,
    // 对话框的返回值是 Retry（通常从标签为“重试”的按钮发送）。

    Ignore = 5,
    // 对话框的返回值是 Ignore（通常从标签为“忽略”的按钮发送）。

    Yes = 6,
    // 对话框的返回值是 Yes（通常从标签为“是”的按钮发送）

    No = 7,
    // 对话框的返回值是 No（通常从标签为“否”的按钮发送）。

    TryAgain = 10,
    // 对话框返回值是“重试” (通常从标记为“重试”的按钮发送) 。

    Continue = 11,
    // 对话框返回值是“继续” (通常从标记为“继续”) 的按钮发送。
}

