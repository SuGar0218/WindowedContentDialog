using CommunityToolkit.Mvvm.ComponentModel;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;

using SuGarToolkit.Controls.Dialogs;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuGarToolkit.Sample.Dialogs.ViewModels;

public partial class ContentDialogSettings : ObservableObject
{
    public string Title { get; set; } = "Lorem ipsum dolor sit amet";

    public string Message { get; set; } = "";

    public string PrimaryButtonText { get; set; } = "Primary Button";

    public string SecondaryButtonText { get; set; } = "Secondary Button";

    public string CloseButtonText { get; set; } = "Close";

    public ContentDialogButton DefaultButton { get; set; } = ContentDialogButton.Primary;

    [ObservableProperty]
    public partial bool IsChild { get; set; } = true;

    public bool IsModal { get; set; } = true;

    public bool IsTitleBarVisible { get; set; } = true;

    public bool PrimaryButtonNotClose { get; set; }

    public bool SecondaryButtonNotClose { get; set; }

    public BuiltInSystemBackdropType BackdropType { get; set; } = BuiltInSystemBackdropType.Mica;

    public bool DisableBehind { get; set; }

    public ContentDialogSmokeLayerKind SmokeLayerKind { get; set; } = ContentDialogSmokeLayerKind.Darken;

    public bool CenterInParent { get; set; } = true;

    public ElementTheme RequestedTheme { get; set; }

    [ObservableProperty]
    public bool ShouldConstrainToRootBounds { get; set; } = true;

    public FlyoutPlacementMode Placement { get; set; }
}
