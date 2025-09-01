using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using SuGarToolkit.Controls.Dialogs.Strings;

using System;
using System.Threading.Tasks;

namespace SuGarToolkit.Controls.Dialogs;

public class InWindowMessageBox
{
    public static async Task ShowAsync(Window ownerWindow, object? content, string? title) => await ShowAsync(ownerWindow, content, title, MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1);
    public static async Task ShowAsync(UIElement rootElement, object? content, string? title) => await ShowAsync(rootElement, content, title, MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1);

    /// <summary>
    /// Show text messages in a MessageBox with xaml root is ownerWindow.Content.XamlRoot.
    /// <br/>
    /// Overload #15:
    /// Invoke overload #13 with default options.
    /// </summary>
    public static async Task<MessageBoxResult> ShowAsync(
        Window ownerWindow,
        string? message,
        string? title,
        MessageBoxButtons buttons,
        MessageBoxIcon icon,
        MessageBoxDefaultButton? defaultButton)
    {
        return await ShowAsync(ownerWindow, message, title, buttons, icon, defaultButton, new InWindowMessageBoxOptions());
    }

    /// <summary>
    /// Show a MessageBox with xaml root is ownerWindow.Content.XamlRoot.
    /// <br/>
    /// Overload #14:
    /// Invoke overload #12 with default options.
    /// </summary>
    public static async Task<MessageBoxResult> ShowAsync(
        Window ownerWindow,
        object? content,
        string? title,
        MessageBoxButtons buttons,
        MessageBoxIcon icon,
        MessageBoxDefaultButton? defaultButton)
    {
        return await ShowAsync(ownerWindow, content, title, buttons, icon, defaultButton, new InWindowMessageBoxOptions());
    }

    /// <summary>
    /// Show a MessageBox with xaml root is ownerWindow.Content.XamlRoot.
    /// <br/>
    /// Overload #13:
    /// Invoke overload #1 with xaml root is ownerWindow.Content.XamlRoot.
    /// </summary>
    public static async Task<MessageBoxResult> ShowAsync(
        Window ownerWindow,
        string? message,
        string? title,
        MessageBoxButtons buttons,
        MessageBoxIcon icon,
        MessageBoxDefaultButton? defaultButton,
        InWindowMessageBoxOptions options)
    {
        return await ShowAsync(ownerWindow.Content.XamlRoot, message, title, buttons, icon, defaultButton, options);
    }

    /// <summary>
    /// Show a MessageBox with xaml root is ownerWindow.Content.XamlRoot.
    /// <br/>
    /// Overload #12:
    /// Invoke overload #0 with xaml root is ownerWindow.Content.XamlRoot.
    /// </summary>
    public static async Task<MessageBoxResult> ShowAsync(
        Window ownerWindow,
        object? content,
        string? title,
        MessageBoxButtons buttons,
        MessageBoxIcon icon,
        MessageBoxDefaultButton? defaultButton,
        InWindowMessageBoxOptions options)
    {
        return await ShowAsync(ownerWindow.Content.XamlRoot, content, title, buttons, icon, defaultButton, options);
    }

    /// <summary>
    /// Show text messages in a MessageBox with xaml root of UIElement.
    /// <br/>
    /// Overload #11:
    /// Invoke overload #9 with default options.
    /// </summary>
    public static async Task<MessageBoxResult> ShowAsync(
        UIElement rootElement,
        string? message,
        string? title,
        MessageBoxButtons buttons,
        MessageBoxIcon icon,
        MessageBoxDefaultButton? defaultButton)
    {
        return await ShowAsync(rootElement, message, title, buttons, icon, defaultButton, new InWindowMessageBoxOptions());
    }

    /// <summary>
    /// Show a MessageBox with xaml root of UIElement.
    /// <br/>
    /// Overload #10:
    /// Invoke overload #8 with default options.
    /// </summary>
    public static async Task<MessageBoxResult> ShowAsync(
        UIElement rootElement,
        object? content,
        string? title,
        MessageBoxButtons buttons,
        MessageBoxIcon icon,
        MessageBoxDefaultButton? defaultButton)
    {
        return await ShowAsync(rootElement, content, title, buttons, icon, defaultButton, new InWindowMessageBoxOptions());
    }

    /// <summary>
    /// Show text messages in a MessageBox with xaml root of UIElement.
    /// <br/>
    /// Overload #9:
    /// Invoke overload #1 with xaml root of UIElement.
    /// </summary>
    public static async Task<MessageBoxResult> ShowAsync(
        UIElement rootElement,
        string? message,
        string? title,
        MessageBoxButtons buttons,
        MessageBoxIcon icon,
        MessageBoxDefaultButton? defaultButton,
        InWindowMessageBoxOptions options)
    {
        return await ShowAsync(rootElement.XamlRoot, message, title, buttons, icon, defaultButton, options);
    }

    /// <summary>
    /// Show a MessageBox with xaml root of UIElement.
    /// <br/>
    /// Overload #8:
    /// Invoke overload #0 with xaml root of UIElement.
    /// </summary>
    public static async Task<MessageBoxResult> ShowAsync(
        UIElement rootElement,
        object? content,
        string? title,
        MessageBoxButtons buttons,
        MessageBoxIcon icon,
        MessageBoxDefaultButton? defaultButton,
        InWindowMessageBoxOptions options)
    {
        return await ShowAsync(rootElement.XamlRoot, content, title, buttons, icon, defaultButton, options);
    }

    /// <summary>
    /// Show text messages in a MessageBox without icon icon.
    /// <br/>
    /// Overload #7:
    /// Invoke overload #6 with content is a SelectableTextBox.
    /// </summary>
    public static async Task<MessageBoxResult> ShowAsync(
        XamlRoot xamlRoot,
        string? message,
        string? title)
    {
        return await ShowAsync(xamlRoot, CreateSelectableTextBlock(message), title, MessageBoxButtons.OK, MessageBoxDefaultButton.Button1);
    }

    /// <summary>
    /// Show a MessageBox with only OK button.
    /// <br/>
    /// Overload #6:
    /// Invoke overload #4.
    /// </summary>
    public static async Task<MessageBoxResult> ShowAsync(
        XamlRoot xamlRoot,
        object? content,
        string? title)
    {
        return await ShowAsync(xamlRoot, content, title, MessageBoxButtons.OK, MessageBoxDefaultButton.Button1);
    }

    /// <summary>
    /// Show text messages in a MessageBox without icon icon.
    /// <br/>
    /// Overload #5:
    /// Invoke overload #4 with content is a SelectableTextBox.
    /// </summary>
    public static async Task<MessageBoxResult> ShowAsync(
        XamlRoot xamlRoot,
        string? message,
        string? title,
        MessageBoxButtons buttons,
        MessageBoxDefaultButton? defaultButton)
    {
        return await ShowAsync(xamlRoot, CreateSelectableTextBlock(message), title, buttons, MessageBoxIcon.None, defaultButton);
    }

    /// <summary>
    /// Show a MessageBox without icon icon.
    /// <br/>
    /// Overload #4:
    /// Invoke overload #2 with icon = MessageBoxIcon.None.
    /// </summary>
    public static async Task<MessageBoxResult> ShowAsync(
        XamlRoot xamlRoot,
        object? content,
        string? title,
        MessageBoxButtons buttons,
        MessageBoxDefaultButton? defaultButton)
    {
        return await ShowAsync(xamlRoot, content, title, buttons, MessageBoxIcon.None, defaultButton);
    }

    /// <summary>
    /// Show text messages in a MessageBox.
    /// <br/>
    /// Overload #3:
    /// Invoke overload #2 with content is a SelectableTextBox.
    /// </summary>
    public static async Task<MessageBoxResult> ShowAsync(
        XamlRoot xamlRoot,
        string? message,
        string? title,
        MessageBoxButtons buttons,
        MessageBoxIcon icon,
        MessageBoxDefaultButton? defaultButton)
    {
        return await ShowAsync(xamlRoot, CreateSelectableTextBlock(message), title, buttons, icon, defaultButton);
    }

    /// <summary>
    /// Show a MessageBox.
    /// <br/>
    /// Overload #2:
    /// Invoke overload #0 with default options.
    /// </summary>
    public static async Task<MessageBoxResult> ShowAsync(
        XamlRoot xamlRoot,
        object? content,
        string? title,
        MessageBoxButtons buttons,
        MessageBoxIcon icon,
        MessageBoxDefaultButton? defaultButton)
    {
        return await ShowAsync(xamlRoot, content, title, buttons, icon, defaultButton, new InWindowMessageBoxOptions());
    }

    /// <summary>
    /// Show text messages in a MessageBox with a similar appearance to WinUI 3 ContentDialog.
    /// <br/>
    /// Overload #1:
    /// Invoke overload #0 with content is a SelectableTextBox.
    /// </summary>
    /// <param name="xamlRoot">XamlRoot of Popup, cannot be null, otherwise, it will cause exception.</param>
    /// <param name="message">Text message displayed in body area</param>
    /// <param name="title">Text text displayed in header area</param>
    /// <param name="buttons">The button combination displayed at the bottom of MessageBox</param>
    /// <param name="icon">MessageBox icon icon</param>
    /// <param name="defaultButton">Which button should be focused initially</param>
    /// <param name="options">Other style settings like SystemBackdrop.</param>
    public static async Task<MessageBoxResult> ShowAsync(
        XamlRoot xamlRoot,
        string? message,
        string? title,
        MessageBoxButtons buttons,
        MessageBoxIcon icon,
        MessageBoxDefaultButton? defaultButton,
        InWindowMessageBoxOptions options)
    {
        return await ShowAsync(xamlRoot, CreateSelectableTextBlock(message), title, buttons, icon, defaultButton, options);
    }

    /// <summary>
    /// Show a MessageBox with a similar appearance to WinUI 3 ContentDialog.
    /// <br/>
    /// Overload #0:
    /// The main overload of ShowAsync with full parameters.
    /// </summary>
    /// <param name="xamlRoot">XamlRoot of Popup, cannot be null, otherwise, it will cause exception.</param>
    /// <param name="content">DialogContent displayed in body area, which can be string or UIElement</param>
    /// <param name="title">Text text displayed in header area</param>
    /// <param name="buttons">The button combination displayed at the bottom of MessageBox</param>
    /// <param name="icon">MessageBox icon icon</param>
    /// <param name="defaultButton">Which button should be focused initially</param>
    /// <param name="options">Other style settings like SystemBackdrop.</param>
    /// <returns>The button selected by user.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Will not happen under normal circumstances. It happens when judging which button user selected.</exception>
    public static async Task<MessageBoxResult> ShowAsync(
        XamlRoot xamlRoot,
        object? content,
        string? title,
        MessageBoxButtons buttons,
        MessageBoxIcon icon,
        MessageBoxDefaultButton? defaultButton,
        InWindowMessageBoxOptions options)
    {
        ElementTheme theme;
        if (options.RequestedTheme is not ElementTheme.Default)
        {
            theme = options.RequestedTheme;
        }
        else if (xamlRoot.Content is FrameworkElement root)
        {
            theme = root.ActualTheme;
        }
        else
        {
            theme = ElementTheme.Default;
        }

        ContentDialog dialog = new()
        {
            Title = new MessageBoxHeader { Text = title, Icon = icon },
            Content = content,
            XamlRoot = xamlRoot,
            Style = (Style) Application.Current.Resources["DefaultContentDialogStyle"],
            RequestedTheme = theme,
            FlowDirection = options.FlowDirection
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
                dialog.CloseButtonText = MessageBoxButtonText.OK;
                dialog.DefaultButton = ContentDialogButton.Close;
                break;

            case MessageBoxButtons.OKCancel:
                dialog.PrimaryButtonText = MessageBoxButtonText.OK;
                dialog.SecondaryButtonText = MessageBoxButtonText.Cancel;
                break;

            case MessageBoxButtons.YesNo:
                dialog.PrimaryButtonText = MessageBoxButtonText.Yes;
                dialog.SecondaryButtonText = MessageBoxButtonText.No;
                break;

            case MessageBoxButtons.YesNoCancel:
                dialog.PrimaryButtonText = MessageBoxButtonText.Yes;
                dialog.SecondaryButtonText = MessageBoxButtonText.No;
                dialog.CloseButtonText = MessageBoxButtonText.Cancel;
                break;

            case MessageBoxButtons.AbortRetryIgnore:
                dialog.PrimaryButtonText = MessageBoxButtonText.Abort;
                dialog.SecondaryButtonText = MessageBoxButtonText.Retry;
                dialog.CloseButtonText = MessageBoxButtonText.Ignore;
                break;

            case MessageBoxButtons.RetryCancel:
                dialog.PrimaryButtonText = MessageBoxButtonText.Retry;
                dialog.SecondaryButtonText = MessageBoxButtonText.Cancel;
                break;

            case MessageBoxButtons.CancelTryContinue:
                dialog.PrimaryButtonText = MessageBoxButtonText.Continue;
                dialog.SecondaryButtonText = MessageBoxButtonText.TryAgain;
                dialog.CloseButtonText = MessageBoxButtonText.Cancel;
                dialog.DefaultButton = ContentDialogButton.Close;
                break;
        }

        ContentDialogResult result;
        if (options.DisableBehind && xamlRoot.Content is Control control)
        {
            bool isOriginallyEnabled = control.IsEnabled;
            control.IsEnabled = false;
            result = await dialog.ShowAsync();
            control.IsEnabled = isOriginallyEnabled;
        }
        else
        {
            result = await dialog.ShowAsync();
        }

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

