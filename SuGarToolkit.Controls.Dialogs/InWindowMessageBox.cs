using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using System;
using System.Threading.Tasks;

namespace SuGarToolkit.Controls.Dialogs;

public class InWindowMessageBox
{
    public static ElementTheme RequestedTheme { get; set; } = ElementTheme.Default;

    public static async Task ShowAsync(Window parent, object content, string? title = null) => await ShowAsync(parent.Content.XamlRoot, content, title, MessageBoxButtons.OK);
    public static async Task ShowAsync(UIElement parent, object content, string? title = null) => await ShowAsync(parent.XamlRoot, content, title, MessageBoxButtons.OK);

    public static async Task<MessageBoxResult> ShowAsync(
        Window parent,
        object content, string? title,
        MessageBoxButtons buttons,
        MessageBoxDefaultButton? defaultButton = MessageBoxDefaultButton.Button1)
        => await ShowAsync(parent.Content.XamlRoot, content, title, buttons, defaultButton);

    public static async Task<MessageBoxResult> ShowAsync(
        UIElement parent,
        object content, string? title,
        MessageBoxButtons buttons,
        MessageBoxDefaultButton? defaultButton = MessageBoxDefaultButton.Button1)
        => await ShowAsync(parent.XamlRoot, content, title, buttons, defaultButton);

    public static async Task<MessageBoxResult> ShowAsync(
        XamlRoot xamlRoot,
        object content, string? title,
        MessageBoxButtons buttons,
        MessageBoxDefaultButton? defaultButton = MessageBoxDefaultButton.Button1)
    {

        ContentDialog dialog = new ContentDialog
        {
            Title = title,
            Content = content,
            XamlRoot = xamlRoot,
            Style = defaultStyle,
            RequestedTheme = RequestedTheme
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
                //dialog.PrimaryButtonStyle = new() {
                //    TargetType = typeof(Button),
                //    Setters = {
                //        new Setter { Property = Button.AccessKeyProperty, Value = "Y" }
                //    }
                //};
                //dialog.SecondaryButtonStyle = new() {
                //    TargetType = typeof(Button),
                //    Setters = {
                //        new Setter { Property = Button.AccessKeyProperty, Value = "N" }
                //    }
                //};
                break;

            case MessageBoxButtons.YesNoCancel:
                dialog.PrimaryButtonText = "Yes";
                dialog.SecondaryButtonText = "No";
                dialog.CloseButtonText = "Cancel";
                //dialog.PrimaryButtonStyle = new() {
                //    TargetType = typeof(Button),
                //    Setters = {
                //        new Setter { Property = Button.AccessKeyProperty, Value = "Y" }
                //    }
                //};
                //dialog.SecondaryButtonStyle = new() {
                //    TargetType = typeof(Button),
                //    Setters = {
                //        new Setter { Property = Button.AccessKeyProperty, Value = "N" }
                //    }
                //};
                break;

            case MessageBoxButtons.AbortRetryIgnore:
                dialog.PrimaryButtonText = "Abort";
                dialog.SecondaryButtonText = "Retry";
                dialog.CloseButtonText = "Ignore";
                //dialog.PrimaryButtonStyle = new() {
                //    TargetType = typeof(Button),
                //    Setters = {
                //        new Setter { Property = Button.AccessKeyProperty, Value = "A" }
                //    }
                //};
                //dialog.SecondaryButtonStyle = new() {
                //    TargetType = typeof(Button),
                //    Setters = {
                //        new Setter { Property = Button.AccessKeyProperty, Value = "R" }
                //    }
                //};
                //dialog.CloseButtonStyle = new() {
                //    TargetType = typeof(Button),
                //    Setters = {
                //        new Setter { Property = Button.AccessKeyProperty, Value = "I" }
                //    }
                //};
                break;

            case MessageBoxButtons.RetryCancel:
                dialog.PrimaryButtonText = "Retry";
                dialog.SecondaryButtonText = "Cancel";
                //dialog.PrimaryButtonStyle = new() {
                //    TargetType = typeof(Button),
                //    Setters = {
                //        new Setter { Property = Button.AccessKeyProperty, Value = "R" }
                //    }
                //};
                break;

            case MessageBoxButtons.CancelTryContinue:
                dialog.PrimaryButtonText = "Continue";
                dialog.SecondaryButtonText = "Try again";
                dialog.CloseButtonText = "Cancel";
                dialog.DefaultButton = ContentDialogButton.Close;
                //dialog.PrimaryButtonStyle = new() {
                //    TargetType = typeof(Button),
                //    Setters = {
                //        new Setter { Property = Button.AccessKeyProperty, Value = "C" }
                //    }
                //};
                //dialog.SecondaryButtonStyle = new() {
                //    TargetType = typeof(Button),
                //    Setters = {
                //        new Setter { Property = Button.AccessKeyProperty, Value = "T" }
                //    }
                //};
                break;

        }

        ContentDialogResult result = await dialog.ShowAsync();
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
            _ => throw new ArgumentException()
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

    private static readonly Style defaultStyle = new()
    {
        TargetType = typeof(ContentDialog),
        BasedOn = Application.Current.Resources["DefaultContentDialogStyle"] as Style
    };
}

