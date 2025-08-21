using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuGarToolkit.Controls.Dialogs;

public class MessageBoxBaseOptions
{
    /// <summary>
    /// Disable the content of window behind when _dialog window shows.
    /// </summary>
    public bool DisableBehind { get; set; }

    public ContentDialogSmokeLayerKind SmokeLayerKind { get; set; }

    public UIElement? CustomSmokeLayer { get; set; }

    public ElementTheme RequestedTheme { get; set; }

    public FlowDirection FlowDirection { get; set; }
}
