using Microsoft.UI.Xaml;

using System.Threading.Tasks;

namespace SuGarToolkit.Controls.Dialogs;

public class FlyoutMessageBox : MessageBoxBase
{
    public static async Task<MessageBoxResult> ShowAsync(
        FrameworkElement anchor,
        string? message,
        string? title,
        MessageBoxButtons buttons,
        MessageBoxIcon icon,
        MessageBoxDefaultButton? defaultButton,
        FlyoutMessageBoxOptions options)
    {
        return await new FlyoutMessageBox(anchor, message, title, buttons, icon, defaultButton, options).ShowAsync();
    }

    public static async Task<MessageBoxResult> ShowAsync(
        FrameworkElement anchor,
        object? content,
        string? title,
        MessageBoxButtons buttons,
        MessageBoxIcon icon,
        MessageBoxDefaultButton? defaultButton,
        FlyoutMessageBoxOptions options)
    {
        return await new FlyoutMessageBox(anchor, content, title, buttons, icon, defaultButton, options).ShowAsync();
    }

    protected FlyoutMessageBox(
        FrameworkElement anchor,
        string? message,
        string? title,
        MessageBoxButtons buttons,
        MessageBoxIcon icon,
        MessageBoxDefaultButton? defaultButton,
        FlyoutMessageBoxOptions options) : base(message, title, buttons, icon, defaultButton, options)
    {
        _anchor = anchor;
        _options = options;
    }

    protected FlyoutMessageBox(
        FrameworkElement anchor,
        object? content,
        string? title,
        MessageBoxButtons buttons,
        MessageBoxIcon icon,
        MessageBoxDefaultButton? defaultButton,
        FlyoutMessageBoxOptions options) : base(content, title, buttons, icon, defaultButton, options)
    {
        _anchor = anchor;
        _options = options;
    }

    /// <summary>
    /// Will be passed to Flyout.ShowAt
    /// </summary>
    private readonly FrameworkElement _anchor;

    private readonly FlyoutMessageBoxOptions _options;

    protected override StandaloneContentDialogBase CreateDialog() => new FlyoutContentDialog
    {
        PlacementTarget = _anchor,
        Placement = _options.Placement,
        SystemBackdrop = _options.SystemBackdrop,
        ShouldConstrainToRootBounds = _options.ShouldConstrainToRootBounds
    };

    //protected override ElementTheme DetermineTheme()
    //{
    //    return _options.RequestedTheme is ElementTheme.Default ? _anchor.ActualTheme : _options.RequestedTheme;
    //}
}
