using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using SuGarToolkit.Controls.Dialogs.Strings;

using System;
using System.Threading.Tasks;

namespace SuGarToolkit.Controls.Dialogs;

public class MessageBox
{
    /// <summary>
    /// Show text messages in a MessageBox with only OK button.
    /// <br/>
    /// Overload #11:
    /// Invoke overload #9 with without owner/parent window.
    /// </summary>
    public static async Task<MessageBoxResult> ShowAsync(string? message, string? title = null) => await ShowAsync(false, null, message, title);

    /// <summary>
    /// Show text messages in a MessageBox with only OK button.
    /// <br/>
    /// Overload #10:
    /// Invoke overload #8 without owner/parent window.
    /// </summary>
    public static async Task<MessageBoxResult> ShowAsync(object? content, string? title = null) => await ShowAsync(false, null, content, title);

    /// <summary>
    /// Show text messages in a MessageBox with only OK button.
    /// <br/>
    /// Overload #9:
    /// Invoke overload #5 with buttons is MessageBoxButtons.OK.
    /// </summary>
    public static async Task<MessageBoxResult> ShowAsync(
        bool isModal,
        Window? owner,
        string? message,
        string? title = null)
        => await ShowAsync(isModal, owner, message, title, MessageBoxButtons.OK, MessageBoxDefaultButton.Button1, false);

    /// <summary>
    /// Show text messages in a MessageBox with only OK button.
    /// <br/>
    /// Overload #8:
    /// Invoke overload #4 with buttons is MessageBoxButtons.OK.
    /// </summary>
    public static async Task<MessageBoxResult> ShowAsync(
        bool isModal,
        Window? owner,
        object? content,
        string? title = null)
        => await ShowAsync(isModal, owner, content, title, MessageBoxButtons.OK, MessageBoxDefaultButton.Button1, false);

    /// <summary>
    /// Show text messages in a MessageBox without icon image.
    /// <br/>
    /// Overload #7:
    /// Invoke overload #6 with content is a SelectableTextBox.
    /// </summary>
    public static async Task<MessageBoxResult> ShowAsync(
        string? message,
        string? title,
        MessageBoxButtons buttons,
        MessageBoxDefaultButton? defaultButton = MessageBoxDefaultButton.Button1,
        bool isTitleBarVisible = true)
        => await ShowAsync(false, null, CreateSelectableTextBlock(message), title, buttons, defaultButton, isTitleBarVisible);

    /// <summary>
    /// Show a MessageBox without icon image.
    /// <br/>
    /// Overload #6:
    /// Invoke overload #4 without owner/parent window.
    /// </summary>
    public static async Task<MessageBoxResult> ShowAsync(
        object? content,
        string? title,
        MessageBoxButtons buttons,
        MessageBoxDefaultButton? defaultButton = MessageBoxDefaultButton.Button1,
        bool isTitleBarVisible = true)
        => await ShowAsync(false, null, content, title, buttons, defaultButton, isTitleBarVisible);

    /// <summary>
    /// Show text messages in a MessageBox without icon image.
    /// <br/>
    /// Overload #5:
    /// Invoke overload #4 with content is a SelectableTextBox.
    /// </summary>
    public static async Task<MessageBoxResult> ShowAsync(
        bool isModal,
        Window? owner,
        string? message,
        string? title,
        MessageBoxButtons buttons,
        MessageBoxDefaultButton? defaultButton = MessageBoxDefaultButton.Button1,
        bool isTitleBarVisible = true)
    {
        return await ShowAsync(isModal, owner, CreateSelectableTextBlock(message), title, buttons, MessageBoxImage.None, defaultButton, isTitleBarVisible);
    }

    /// <summary>
    /// Show a MessageBox without icon image.
    /// <br/>
    /// Overload #4:
    /// Invoke overload #2 with image = MessageBoxImage.None.
    /// </summary>
    public static async Task<MessageBoxResult> ShowAsync(
        bool isModal,
        Window? owner,
        object? content,
        string? title,
        MessageBoxButtons buttons,
        MessageBoxDefaultButton? defaultButton = MessageBoxDefaultButton.Button1,
        bool isTitleBarVisible = true)
    {
        return await ShowAsync(isModal, owner, content, title, buttons, MessageBoxImage.None, defaultButton, isTitleBarVisible);
    }

    /// <summary>
    /// Determine whether to show title bar (mainly the close button) and show text messages in a MessageBox.
    /// <br/>
    /// Overload #3:
    /// Invoke overload #2 with content is a SelectableTextBox.
    /// </summary>
    public static async Task<MessageBoxResult> ShowAsync(
        bool isModal,
        Window? owner,
        string? message,
        string? title,
        MessageBoxButtons buttons,
        MessageBoxImage image,
        MessageBoxDefaultButton? defaultButton = MessageBoxDefaultButton.Button1,
        bool isTitleBarVisible = true)
    {
        MessageBoxOptions options = MessageBoxOptions.Default;
        options.IsTitleBarVisible = isTitleBarVisible;
        return await ShowAsync(isModal, owner, CreateSelectableTextBlock(message), title, buttons, image, defaultButton, options);
    }

    /// <summary>
    /// Determine whether to show title bar (mainly the close button) and show a MessageBox.
    /// <br/>
    /// Overload #2:
    /// Invoke overload #0 and only set IsTitleBarVisible in options.
    /// </summary>
    public static async Task<MessageBoxResult> ShowAsync(
        bool isModal,
        Window? owner,
        object? content,
        string? title,
        MessageBoxButtons buttons,
        MessageBoxImage image,
        MessageBoxDefaultButton? defaultButton = MessageBoxDefaultButton.Button1,
        bool isTitleBarVisible = true)
    {
        MessageBoxOptions options = MessageBoxOptions.Default;
        options.IsTitleBarVisible = isTitleBarVisible;
        return await ShowAsync(isModal, owner, content, title, buttons, image, defaultButton, options);
    }

    /// <summary>
    /// Show text messages in a MessageBox with a similar appearance to WinUI 3 ContentDialog.
    /// Overload #1:
    /// Invoke overload #0 with content is a SelectableTextBox.
    /// </summary>
    /// <param name="isModal">Whether the MessageBox is a modal window.</param>
    /// <param name="owner">Owner/Parent window of the MessageBox</param>
    /// <param name="message">Text message displayed in body area</param>
    /// <param name="title">Text text displayed in header area</param>
    /// <param name="buttons">The button combination displayed at the bottom of MessageBox</param>
    /// <param name="image">MessageBox icon image</param>
    /// <param name="defaultButton">Which button should be focused initially</param>
    /// <param name="options">Other style settings like SystemBackdrop.</param>
    public static async Task<MessageBoxResult> ShowAsync(
        bool isModal,
        Window? owner,
        string? message,
        string? title,
        MessageBoxButtons buttons,
        MessageBoxImage image,
        MessageBoxDefaultButton? defaultButton,
        MessageBoxOptions options)
    {
        return await ShowAsync(isModal, owner, CreateSelectableTextBlock(message), title, buttons, image, defaultButton, options);  // Overload #0
    }

    /// <summary>
    /// Show a MessageBox with a similar appearance to WinUI 3 ContentDialog.
    /// <br/>
    /// Overload #0:
    /// The main overload of ShowAsync with full parameters.
    /// </summary>
    /// <param name="isModal">Whether the MessageBox is a modal window.</param>
    /// <param name="owner">Owner/Parent window of the MessageBox</param>
    /// <param name="content">DialogContent displayed in body area, which can be string or UIElement</param>
    /// <param name="title">Text text displayed in header area</param>
    /// <param name="buttons">The button combination displayed at the bottom of MessageBox</param>
    /// <param name="image">MessageBox icon image</param>
    /// <param name="defaultButton">Which button should be focused initially</param>
    /// <param name="options">Other style settings like SystemBackdrop.</param>
    /// <returns>The button selected by user.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Will not happen under normal circumstances. It happens when judging which button user selected.</exception>
    public static async Task<MessageBoxResult> ShowAsync(
        bool isModal,
        Window? owner,
        object? content,
        string? title,
        MessageBoxButtons buttons,
        MessageBoxImage image,
        MessageBoxDefaultButton? defaultButton,
        MessageBoxOptions options)
    {
        ElementTheme theme;
        if (options.RequestedTheme is not ElementTheme.Default)
        {
            theme = options.RequestedTheme;
        }
        else if (owner is not null)
        {
            if (owner.Content is FrameworkElement root)
            {
                theme = root.ActualTheme;
            }
            else
            {
                theme = owner.AppWindow.TitleBar.PreferredTheme switch
                {
                    Microsoft.UI.Windowing.TitleBarTheme.UseDefaultAppMode => ElementTheme.Default,
                    Microsoft.UI.Windowing.TitleBarTheme.Light => ElementTheme.Light,
                    Microsoft.UI.Windowing.TitleBarTheme.Dark => ElementTheme.Dark,
                    _ => ElementTheme.Default
                };
            }
        }
        else
        {
            theme = ElementTheme.Default;
        }

        WindowedContentDialog dialog = new()
        {
            WindowTitle = title,
            Title = new MessageBoxHeader { Text = title, Image = image },
            Content = content,
            OwnerWindow = owner,
            SystemBackdrop = options.SystemBackdrop,
            RequestedTheme = theme,
            IsTitleBarVisible = options.IsTitleBarVisible,
            FlowDirection = options.FlowDirection,
            CenterInParent = options.CenterInParent,
            SmokeLayerKind = options.SmokeLayerKind,
            CustomSmokeLayer = options.CustomSmokeLayer,
            DisableBehind = options.DisableBehind,
        };

        ContentDialogButton contentDialogDefaultButton = defaultButton switch
        {
            MessageBoxDefaultButton.Button1 => ContentDialogButton.Primary,
            MessageBoxDefaultButton.Button2 => ContentDialogButton.Secondary,
            MessageBoxDefaultButton.Button3 => ContentDialogButton.Close,
            null => ContentDialogButton.None,
            _ => ContentDialogButton.None
        };
        dialog.DefaultButton = contentDialogDefaultButton;

        switch (buttons)
        {
            case MessageBoxButtons.OK:
                dialog.CloseButtonText = "OK";
                dialog.DefaultButton = ContentDialogButton.Close;
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

        ContentDialogResult result = await dialog.ShowAsync(isModal);
        MessageBoxResult[] results = MessageBoxResultsOf(buttons);
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

    /// <summary>
    /// Create a readonly TextBox that allows user to select the message text.
    /// <br/>
    /// Why not use TextBlock with IsTextSelectionEnabled=true ?
    /// Currently (WindowsAppSDK 1.7), once user selected text,
    /// TextBlock with IsTextSelectionEnabled=true and TextWrapping=TextWrapping.Wrap
    /// cannot update text wrapping automatically until the next time user selects text.
    /// </summary>
    private static SelectableTextBlock CreateSelectableTextBlock(string? text) => new()
    {
        Text = text,
        TextWrapping = TextWrapping.Wrap,
        HorizontalAlignment = HorizontalAlignment.Stretch
    };
}

