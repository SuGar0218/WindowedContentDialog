using CommunityToolkit.Mvvm.ComponentModel;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls.Primitives;

using SuGarToolkit.Controls.Dialogs;

namespace SuGarToolkit.Sample.Dialogs.ViewModels;

public partial class MessageBoxSettings : ObservableObject
{
    public string Title { get; set; } = "Message Box";

    public string Content { get; set; } = "Lorem ipsum dolor sit amet.";

    public MessageBoxButtons Buttons { get; set; } = MessageBoxButtons.OK;

    public MessageBoxDefaultButton DefaultButton { get; set; } = MessageBoxDefaultButton.Button1;

    public MessageBoxIcon Image { get; set; } = MessageBoxIcon.Information;

    [ObservableProperty]
    public partial bool IsChild { get; set; } = true;

    public bool IsModal { get; set; } = true;

    public bool IsTitleBarVisible { get; set; } = true;

    public BuiltInSystemBackdropType BackdropType { get; set; } = BuiltInSystemBackdropType.Mica;

    public ContentDialogSmokeLayerKind SmokeLayerKind { get; set; } = ContentDialogSmokeLayerKind.Darken;

    public FlyoutPlacementMode Placement { get; set; }

    public bool ShouldConstrainToRootBounds { get; set; } = true;

    public bool CenterInParent { get; set; } = true;

    public ElementTheme RequestedTheme { get; set; }
}
