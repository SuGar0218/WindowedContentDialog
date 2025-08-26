using Microsoft.UI.Xaml;

using System.Threading.Tasks;

namespace SuGarToolkit.Controls.Dialogs;

public class MessageBox : MessageBoxBase
{
    protected MessageBox(
        bool isModal,
        Window? owner,
        object? content,
        string? title,
        MessageBoxButtons buttons,
        MessageBoxIcon icon,
        MessageBoxDefaultButton? defaultButton,
        MessageBoxOptions options) : base(content, title, buttons, icon, defaultButton, options)
    {
        _isModal = isModal;
        _owner = owner;
        _options = options;
    }

    protected MessageBox(
        bool isModal,
        Window? owner,
        string? message,
        string? title,
        MessageBoxButtons buttons,
        MessageBoxIcon icon,
        MessageBoxDefaultButton? defaultButton,
        MessageBoxOptions options) : base(message, title, buttons, icon, defaultButton, options)
    {
        _isModal = isModal;
        _owner = owner;
        _options = options;
    }

    private readonly bool _isModal;
    private readonly Window? _owner;
    private readonly MessageBoxOptions _options;

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
    /// Show text messages in a MessageBox without icon.
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
        => await ShowAsync(false, null, message, title, buttons, defaultButton, isTitleBarVisible);

    /// <summary>
    /// Show a MessageBox without icon.
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
    /// Show text messages in a MessageBox without icon.
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
        return await ShowAsync(isModal, owner, message, title, buttons, MessageBoxIcon.None, defaultButton, isTitleBarVisible);
    }

    /// <summary>
    /// Show a MessageBox without icon.
    /// <br/>
    /// Overload #4:
    /// Invoke overload #2 with icon = MessageBoxIcon.None.
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
        return await ShowAsync(isModal, owner, content, title, buttons, MessageBoxIcon.None, defaultButton, isTitleBarVisible);
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
        MessageBoxIcon icon,
        MessageBoxDefaultButton? defaultButton = MessageBoxDefaultButton.Button1,
        bool isTitleBarVisible = true)
    {
        MessageBoxOptions options = MessageBoxOptions.Default;
        options.IsTitleBarVisible = isTitleBarVisible;
        return await ShowAsync(isModal, owner, message, title, buttons, icon, defaultButton, options);
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
        MessageBoxIcon icon,
        MessageBoxDefaultButton? defaultButton = MessageBoxDefaultButton.Button1,
        bool isTitleBarVisible = true)
    {
        MessageBoxOptions options = MessageBoxOptions.Default;
        options.IsTitleBarVisible = isTitleBarVisible;
        return await ShowAsync(isModal, owner, content, title, buttons, icon, defaultButton, options);
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
    /// <param name="icon">MessageBox icon</param>
    /// <param name="defaultButton">Which button should be focused initially</param>
    /// <param name="options">Other style settings like SystemBackdrop.</param>
    public static async Task<MessageBoxResult> ShowAsync(
        bool isModal,
        Window? owner,
        string? message,
        string? title,
        MessageBoxButtons buttons,
        MessageBoxIcon icon,
        MessageBoxDefaultButton? defaultButton,
        MessageBoxOptions options)
    {
        return await new MessageBox(isModal, owner, message, title, buttons, icon, defaultButton, options).ShowAsync();
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
    /// <param name="icon">MessageBox icon</param>
    /// <param name="defaultButton">Which button should be focused initially</param>
    /// <param name="options">Other style settings like SystemBackdrop.</param>
    /// <returns>The button selected by user.</returns>
    public static async Task<MessageBoxResult> ShowAsync(
        bool isModal,
        Window? owner,
        object? content,
        string? title,
        MessageBoxButtons buttons,
        MessageBoxIcon icon,
        MessageBoxDefaultButton? defaultButton,
        MessageBoxOptions options)
    {
        return await new MessageBox(isModal, owner, content, title, buttons, icon, defaultButton, options).ShowAsync();
    }

    protected override StandaloneContentDialogBase CreateDialog() => new WindowedContentDialog
    {
        WindowTitle = _title,
        IsModal = _isModal,
        OwnerWindow = _owner,
        SystemBackdrop = _options.SystemBackdrop,
        CenterInParent = _options.CenterInParent,
        IsTitleBarVisible = _options.IsTitleBarVisible
    };

    //protected override ElementTheme DetermineTheme()
    //{
    //    if (_options.RequestedTheme is not ElementTheme.Default)
    //        return _options.RequestedTheme;

    //    if (_owner?.Content is FrameworkElement root)
    //        return root.ActualTheme;

    //    return ElementTheme.Default;
    //}
}

