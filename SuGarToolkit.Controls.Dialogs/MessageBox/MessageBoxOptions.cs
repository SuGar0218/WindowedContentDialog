using Microsoft.UI.Xaml.Media;

namespace SuGarToolkit.Controls.Dialogs;

public class MessageBoxOptions : MessageBoxBaseOptions
{
    public SystemBackdrop? SystemBackdrop { get; set; }

    public bool IsTitleBarVisible { get; set; } = true;

    public bool CenterInParent { get; set; } = true;

    public static MessageBoxOptions Default => new() { SystemBackdrop = new MicaBackdrop() };
}
