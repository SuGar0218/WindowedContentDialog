using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using System;
using System.Threading.Tasks;

namespace SuGarToolkit.Controls.Dialogs;

public class InWindowMessageBox
{
    public static async Task ShowAsync(Window ownerWindow, object? content, string? title) => await ShowAsync(ownerWindow, content, title, MessageBoxButtons.OK, MessageBoxImage.None, MessageBoxDefaultButton.Button1);
    public static async Task ShowAsync(UIElement rootElement, object? content, string? title) => await ShowAsync(rootElement, content, title, MessageBoxButtons.OK, MessageBoxImage.None, MessageBoxDefaultButton.Button1);

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
        MessageBoxImage image,
        MessageBoxDefaultButton? defaultButton)
    {
        return await ShowAsync(ownerWindow, message, title, buttons, image, defaultButton, new InWindowMessageBoxOptions());
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
        MessageBoxImage image,
        MessageBoxDefaultButton? defaultButton)
    {
        return await ShowAsync(ownerWindow, content, title, buttons, image, defaultButton, new InWindowMessageBoxOptions());
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
        MessageBoxImage image,
        MessageBoxDefaultButton? defaultButton,
        InWindowMessageBoxOptions options)
    {
        return await ShowAsync(ownerWindow.Content.XamlRoot, message, title, buttons, image, defaultButton, options);
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
        MessageBoxImage image,
        MessageBoxDefaultButton? defaultButton,
        InWindowMessageBoxOptions options)
    {
        return await ShowAsync(ownerWindow.Content.XamlRoot, content, title, buttons, image, defaultButton, options);
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
        MessageBoxImage image,
        MessageBoxDefaultButton? defaultButton)
    {
        return await ShowAsync(rootElement, message, title, buttons, image, defaultButton, new InWindowMessageBoxOptions());
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
        MessageBoxImage image,
        MessageBoxDefaultButton? defaultButton)
    {
        return await ShowAsync(rootElement, content, title, buttons, image, defaultButton, new InWindowMessageBoxOptions());
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
        MessageBoxImage image,
        MessageBoxDefaultButton? defaultButton,
        InWindowMessageBoxOptions options)
    {
        return await ShowAsync(rootElement.XamlRoot, message, title, buttons, image, defaultButton, options);
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
        MessageBoxImage image,
        MessageBoxDefaultButton? defaultButton,
        InWindowMessageBoxOptions options)
    {
        return await ShowAsync(rootElement.XamlRoot, content, title, buttons, image, defaultButton, options);
    }

    /// <summary>
    /// Show text messages in a MessageBox without icon image.
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
    /// Show text messages in a MessageBox without icon image.
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
        return await ShowAsync(xamlRoot, CreateSelectableTextBlock(message), title, buttons, MessageBoxImage.None, defaultButton);
    }

    /// <summary>
    /// Show a MessageBox without icon image.
    /// <br/>
    /// Overload #4:
    /// Invoke overload #2 with image = MessageBoxImage.None.
    /// </summary>
    public static async Task<MessageBoxResult> ShowAsync(
        XamlRoot xamlRoot,
        object? content,
        string? title,
        MessageBoxButtons buttons,
        MessageBoxDefaultButton? defaultButton)
    {
        return await ShowAsync(xamlRoot, content, title, buttons, MessageBoxImage.None, defaultButton);
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
        MessageBoxImage image,
        MessageBoxDefaultButton? defaultButton)
    {
        return await ShowAsync(xamlRoot, CreateSelectableTextBlock(message), title, buttons, image, defaultButton);
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
        MessageBoxImage image,
        MessageBoxDefaultButton? defaultButton)
    {
        return await ShowAsync(xamlRoot, content, title, buttons, image, defaultButton, new InWindowMessageBoxOptions());
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
    /// <param name="image">MessageBox icon image</param>
    /// <param name="defaultButton">Which button should be focused initially</param>
    /// <param name="options">Other style settings like SystemBackdrop.</param>
    public static async Task<MessageBoxResult> ShowAsync(
        XamlRoot xamlRoot,
        string? message,
        string? title,
        MessageBoxButtons buttons,
        MessageBoxImage image,
        MessageBoxDefaultButton? defaultButton,
        InWindowMessageBoxOptions options)
    {
        return await ShowAsync(xamlRoot, CreateSelectableTextBlock(message), title, buttons, image, defaultButton, options);
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
    /// <param name="image">MessageBox icon image</param>
    /// <param name="defaultButton">Which button should be focused initially</param>
    /// <param name="options">Other style settings like SystemBackdrop.</param>
    /// <returns>The button selected by user.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Will not happen under normal circumstances. It happens when judging which button user selected.</exception>
    public static async Task<MessageBoxResult> ShowAsync(
        XamlRoot xamlRoot,
        object? content,
        string? title,
        MessageBoxButtons buttons,
        MessageBoxImage image,
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
            Title = new MessageBoxHeader { Text = title, Image = image },
            Content = content,
            XamlRoot = xamlRoot,
            Style = defaultStyle,
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

    private static readonly Style defaultStyle = new()
    {
        TargetType = typeof(ContentDialog),
        BasedOn = Application.Current.Resources["DefaultContentDialogStyle"] as Style
    };

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

