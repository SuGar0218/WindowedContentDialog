using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.UI;

namespace SuGarToolkit.Controls.Dialogs;

public abstract class StandaloneContentDialogBase
{
    public abstract Task<ContentDialogResult> ShowAsync();

    public object? Title { get; set; }
    public object? Content { get; set; }
    public string? PrimaryButtonText { get; set; }
    public string? SecondaryButtonText { get; set; }
    public string? CloseButtonText { get; set; }
    public Brush? Foreground { get; set; }
    public Brush? Background { get; set; }
    public Brush? BorderBrush { get; set; }
    public Thickness BorderThickness { get; set; }
    public DataTemplate? TitleTemplate { get; set; }
    public DataTemplate? ContentTemplate { get; set; }
    public ContentDialogButton DefaultButton { get; set; }
    public bool IsPrimaryButtonEnabled { get; set; } = true;
    public bool IsSecondaryButtonEnabled { get; set; } = true;
    public Style PrimaryButtonStyle { get; set; } = DefaultButtonStyle;
    public Style SecondaryButtonStyle { get; set; } = DefaultButtonStyle;
    public Style CloseButtonStyle { get; set; } = DefaultButtonStyle;
    public ElementTheme RequestedTheme { get; set; }
    public FlowDirection FlowDirection { get; set; }

    /// <summary>
    /// Disable the content of window behind when dialog window shows.
    /// </summary>
    public bool DisableBehind { get; set; }

    public ContentDialogSmokeLayerKind SmokeLayerKind { get; set; }

    public UIElement? CustomSmokeLayer { get; set; }

    protected static void SizeToXamlRoot(FrameworkElement element, XamlRoot root)
    {
        element.Width = root.Size.Width;
        element.Height = root.Size.Height;
    }

    protected static Style DefaultButtonStyle => field ??= (Style) Application.Current.Resources["DefaultButtonStyle"];
    protected static Color SmokeFillColor => field == default ? (field = (Color) Application.Current.Resources["SmokeFillColorDefault"]) : field;

}
