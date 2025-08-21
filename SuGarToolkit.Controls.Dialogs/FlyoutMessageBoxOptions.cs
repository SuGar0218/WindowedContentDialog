using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Media;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuGarToolkit.Controls.Dialogs;

public class FlyoutMessageBoxOptions : MessageBoxBaseOptions
{
    /// <summary>
    /// Doesn't work when ShouldConstrainToRootBounds = true.
    /// </summary>
    public SystemBackdrop? SystemBackdrop { get; set; }

    public bool ShouldConstrainToRootBounds { get; set; }

    public FlyoutPlacementMode Placement { get; set; }
}
