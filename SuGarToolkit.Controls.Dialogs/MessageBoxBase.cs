using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using SuGarToolkit.Controls.Dialogs.Strings;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuGarToolkit.Controls.Dialogs;

public abstract class MessageBoxBase
{
    protected MessageBoxBase(
        object? content,
        string? title,
        MessageBoxButtons buttons,
        MessageBoxIcon icon,
        MessageBoxDefaultButton? defaultButton,
        MessageBoxBaseOptions options)
    {
        _content = content;
        _title = title;
        _buttons = buttons;
        _icon = icon;
        _defaultButton = defaultButton;
        _options = options;
        _dialog = null!;
    }

    protected MessageBoxBase(
        string? message,
        string? title,
        MessageBoxButtons buttons,
        MessageBoxIcon icon,
        MessageBoxDefaultButton? defaultButton,
        MessageBoxBaseOptions options) : this(CreateSelectableTextBlock(message), title, buttons, icon, defaultButton, options) { }

    protected readonly object? _content;
    protected readonly string? _title;
    protected readonly MessageBoxButtons _buttons;
    protected readonly MessageBoxIcon _icon;
    protected readonly MessageBoxDefaultButton? _defaultButton;
    private readonly MessageBoxBaseOptions _options;
    private StandaloneContentDialogBase _dialog;

    protected async Task<MessageBoxResult> ShowAsync()
    {
        _dialog = CreateDialog();
        _dialog.Content = _content;
        _dialog.Title = new MessageBoxHeader
        {
            Icon = _icon,
            Text = _title
        };
        _dialog.DisableBehind = _options.DisableBehind;
        _dialog.SmokeLayerKind = _options.SmokeLayerKind;
        _dialog.CustomSmokeLayer = _options.CustomSmokeLayer;
        _dialog.FlowDirection = _options.FlowDirection;
        _dialog.RequestedTheme = DetermineTheme();
        DetermineDefaultButton();
        DetermineButtonText();
        return await ShowAndWaitForResultAsync();
    }

    /// <summary>
    /// Create instance and set properties not contained in StandaloneContentDialogBase. 
    /// </summary>
    /// <returns></returns>
    protected abstract StandaloneContentDialogBase CreateDialog();

    protected abstract ElementTheme DetermineTheme();

    protected void DetermineDefaultButton()
    {
        _dialog.DefaultButton = _defaultButton switch
        {
            MessageBoxDefaultButton.Button1 => ContentDialogButton.Primary,
            MessageBoxDefaultButton.Button2 => ContentDialogButton.Secondary,
            MessageBoxDefaultButton.Button3 => ContentDialogButton.Close,
            null => ContentDialogButton.None,
            _ => ContentDialogButton.None
        };
    }

    protected void DetermineButtonText()
    {
        switch (_buttons)
        {
            case MessageBoxButtons.OK:
                _dialog.CloseButtonText = MessageBoxButtonText.OK;
                _dialog.DefaultButton = ContentDialogButton.Close;
                break;

            case MessageBoxButtons.OKCancel:
                _dialog.PrimaryButtonText = MessageBoxButtonText.OK;
                _dialog.SecondaryButtonText = MessageBoxButtonText.Cancel;
                break;

            case MessageBoxButtons.YesNo:
                _dialog.PrimaryButtonText = MessageBoxButtonText.Yes;
                _dialog.SecondaryButtonText = MessageBoxButtonText.No;
                break;

            case MessageBoxButtons.YesNoCancel:
                _dialog.PrimaryButtonText = MessageBoxButtonText.Yes;
                _dialog.SecondaryButtonText = MessageBoxButtonText.No;
                _dialog.CloseButtonText = MessageBoxButtonText.Cancel;
                break;

            case MessageBoxButtons.AbortRetryIgnore:
                _dialog.PrimaryButtonText = MessageBoxButtonText.Abort;
                _dialog.SecondaryButtonText = MessageBoxButtonText.Retry;
                _dialog.CloseButtonText = MessageBoxButtonText.Ignore;
                break;

            case MessageBoxButtons.RetryCancel:
                _dialog.PrimaryButtonText = MessageBoxButtonText.Retry;
                _dialog.SecondaryButtonText = MessageBoxButtonText.Cancel;
                break;

            case MessageBoxButtons.CancelTryContinue:
                _dialog.PrimaryButtonText = MessageBoxButtonText.Continue;
                _dialog.SecondaryButtonText = MessageBoxButtonText.TryAgain;
                _dialog.CloseButtonText = MessageBoxButtonText.Cancel;
                _dialog.DefaultButton = ContentDialogButton.Close;
                break;
        }
    }

    protected async Task<MessageBoxResult> ShowAndWaitForResultAsync()
    {
        ContentDialogResult result = await _dialog.ShowAsync();
        MessageBoxResult[] results = resultGroups[(int) _buttons];
        return result switch
        {
            ContentDialogResult.None => results[^1],
            ContentDialogResult.Primary => results[0],
            ContentDialogResult.Secondary => results[1],
            _ => MessageBoxResult.None,
        };
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

    /// <summary>
    /// Create a readonly TextBox that allows user to select the message text.
    /// <br/>
    /// Why not use TextBlock with IsTextSelectionEnabled=true ?
    /// Currently (WindowsAppSDK 1.7), once user selected text,
    /// TextBlock with IsTextSelectionEnabled=true and TextWrapping=TextWrapping.Wrap
    /// cannot update text wrapping automatically until the next time user selects text.
    /// </summary>
    private static SelectableTextBlock? CreateSelectableTextBlock(string? text) => string.IsNullOrEmpty(text) ? null : new()
    {
        Text = text,
        TextWrapping = TextWrapping.Wrap,
        HorizontalAlignment = HorizontalAlignment.Stretch
    };
}
