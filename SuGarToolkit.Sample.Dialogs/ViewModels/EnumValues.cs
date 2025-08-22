using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;

using SuGarToolkit.Controls.Dialogs;

using System;
using System.Collections.Generic;

namespace SuGarToolkit.Sample.Dialogs.ViewModels;

public class EnumValues
{
    public static readonly MessageBoxButtons[] MessageBoxButtons = Enum.GetValues<MessageBoxButtons>();

    public static readonly MessageBoxDefaultButton[] MessageBoxDefaultButtons = Enum.GetValues<MessageBoxDefaultButton>();

    public static readonly MessageBoxIcon[] MessageBoxImages = Enum.GetValues<MessageBoxIcon>();

    public static readonly BuiltInSystemBackdropType[] SystemBackdropTypes = Enum.GetValues<BuiltInSystemBackdropType>();

    public static readonly ContentDialogSmokeLayerKind[] BehindOverlayTypes = Enum.GetValues<ContentDialogSmokeLayerKind>();

    public static readonly List<ElementTheme> ElementThemes = [.. Enum.GetValues<ElementTheme>()];

    public static readonly List<ContentDialogButton> ContentDialogButtons = [.. Enum.GetValues<ContentDialogButton>()];

    public static readonly List<FlyoutPlacementMode> FlyoutPlacementModes = [.. Enum.GetValues<FlyoutPlacementMode>()];
}
