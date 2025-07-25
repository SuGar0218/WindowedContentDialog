﻿using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SuGarToolkit.Controls.Dialogs;

public class MessageBox
{
    public static async Task<MessageBoxResult> ShowAsync(
        object content,
        string? title = null)
        => await ShowAsync(false, null, content, title, MessageBoxButtons.OK);

    public static async Task<MessageBoxResult> ShowAsync(
        object content,
        string? title,
        MessageBoxButtons buttons,
        MessageBoxDefaultButton? defaultButton = MessageBoxDefaultButton.Button1)
        => await ShowAsync(false, null, content, title, buttons, defaultButton);

    public static async Task<MessageBoxResult> ShowAsync(
        bool modal,
        Window owner,
        object content,
        string? title = null) 
        => await ShowAsync(modal, owner, content, title, MessageBoxButtons.OK);

    public static async Task<MessageBoxResult> ShowAsync(
        bool modal,
        Window? owner,
        object? content, string? title,
        MessageBoxButtons buttons,
        MessageBoxDefaultButton? defaultButton = MessageBoxDefaultButton.Button1)
    {
        FrameworkElement? root = owner?.Content as FrameworkElement;
        WindowedContentDialog dialog = new()
        {
            Title = title ?? string.Empty,
            Content = content,
            OwnerWindow = owner,
            RequestedTheme = root is not null ? root.ActualTheme : owner is null ? ElementTheme.Default : owner.AppWindow.TitleBar.PreferredTheme switch
            {
                Microsoft.UI.Windowing.TitleBarTheme.UseDefaultAppMode => ElementTheme.Default,
                Microsoft.UI.Windowing.TitleBarTheme.Light => ElementTheme.Light,
                Microsoft.UI.Windowing.TitleBarTheme.Dark => ElementTheme.Dark,
                _ => ElementTheme.Default
            }
        };

        ContentDialogButton contentDialogDefaultButton = defaultButton switch
        {
            MessageBoxDefaultButton.Button1 => ContentDialogButton.Primary,
            MessageBoxDefaultButton.Button2 => ContentDialogButton.Secondary,
            MessageBoxDefaultButton.Button3 => ContentDialogButton.Close,
            null => ContentDialogButton.None,
            _ => throw new ArgumentException("MessageBoxDefaultButton defaultButton should be in {Button1=0, Button2=256, Button3=512}")
        };
        dialog.DefaultButton = contentDialogDefaultButton;

        switch (buttons)
        {
            case MessageBoxButtons.OK:
                dialog.PrimaryButtonText = "OK";
                break;

            case MessageBoxButtons.OKCancel:
                dialog.PrimaryButtonText = "OK";
                dialog.SecondaryButtonText = "Cancel";
                break;

            case MessageBoxButtons.YesNo:
                dialog.PrimaryButtonText = "Yes";
                dialog.SecondaryButtonText = "No";
                break;

            case MessageBoxButtons.YesNoCancel:
                dialog.PrimaryButtonText = "Yes";
                dialog.SecondaryButtonText = "No";
                dialog.CloseButtonText = "Cancel";
                break;

            case MessageBoxButtons.AbortRetryIgnore:
                dialog.PrimaryButtonText = "Abort";
                dialog.SecondaryButtonText = "Retry";
                dialog.CloseButtonText = "Ignore";
                break;

            case MessageBoxButtons.RetryCancel:
                dialog.PrimaryButtonText = "Retry";
                dialog.SecondaryButtonText = "Cancel";
                break;

            case MessageBoxButtons.CancelTryContinue:
                dialog.PrimaryButtonText = "Continue";
                dialog.SecondaryButtonText = "Try again";
                dialog.CloseButtonText = "Cancel";
                dialog.DefaultButton = ContentDialogButton.Close;
                break;
        }

        ContentDialogResult result = await dialog.ShowAsync(modal);
        // None    0
        // 未点击任何按钮 或 CloseButton (ESC)

        // Primary 1
        // 用户已点击主按钮

        // Secondary   2
        // 用户点击了辅助按钮
        var results = MessageBoxResultsOf(buttons);
        return results[result switch
        {
            ContentDialogResult.Primary => 0,
            ContentDialogResult.Secondary => 1,
            ContentDialogResult.None => results.Length - 1,
            _ => throw new ArgumentOutOfRangeException(nameof(result)),
        }];
    }

    private static readonly MessageBoxResult[][] resultGroups = [
        [MessageBoxResult.OK],
        [MessageBoxResult.OK, MessageBoxResult.Cancel],
        [MessageBoxResult.Abort, MessageBoxResult.Retry, MessageBoxResult.Ignore],
        [MessageBoxResult.Yes, MessageBoxResult.No, MessageBoxResult.Cancel],
        [MessageBoxResult.Yes, MessageBoxResult.No],
        [MessageBoxResult.Retry, MessageBoxResult.Cancel],
        [MessageBoxResult.Continue, MessageBoxResult.TryAgain, MessageBoxResult.Cancel]
    ];

    private static MessageBoxResult[] MessageBoxResultsOf(MessageBoxButtons buttons) => resultGroups[(int) buttons];
}

public enum MessageBoxButtons
{
    OK = 0,
    // 消息框包含“确定”按钮。

    OKCancel = 1,
    // 消息框包含“确定”和“取消”按钮。

    AbortRetryIgnore = 2,
    // 消息框包含“中止”、“重试”和“忽略”按钮。

    YesNoCancel = 3,
    // 消息框包含“是”、“否”和“取消”按钮。

    YesNo = 4,
    // 消息框包含“是”和“否”按钮。

    RetryCancel = 5,
    // 消息框包含“重试”和“取消”按钮。

    CancelTryContinue = 6,
    // 指定消息框包含“取消”、“重试”和“继续”按钮。
}

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

public enum MessageBoxDefaultButton
{
    Button1 = 0,
    // 消息框上的第一个按钮是默认按钮。

    Button2 = 256,
    // 消息框上的第二个按钮是默认按钮。

    Button3 = 512,
    // 消息框上的第三个按钮是默认按钮。
}
