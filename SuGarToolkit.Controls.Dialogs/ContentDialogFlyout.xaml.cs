using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Markup;
using Microsoft.UI.Xaml.Media;

using SuGarToolkit.SourceGenerators;

using System;
using System.ComponentModel;

using Windows.Foundation;

namespace SuGarToolkit.Controls.Dialogs;

[ContentProperty(Name = nameof(DialogContent))]
public sealed partial class ContentDialogFlyout : Flyout
{
    public ContentDialogFlyout()
    {
        //InitializeComponent();  // [ContentProperty] affects not only derived classed but also current class.
        Content = new ContentDialogContent();
        ContentDialogContent.PrimaryButtonClick += OnPrimaryButtonClick;
        ContentDialogContent.SecondaryButtonClick += OnSecondaryButtonClick;
        ContentDialogContent.CloseButtonClick += OnCloseButtonClick;
        InitializeFlyout();
    }

    private ContentDialogFlyout(UIElement? content)
    {
        Content = content;
        InitializeFlyout();
    }

    internal static ContentDialogFlyout CreateWithoutComponent() => new(null);

    private ContentDialogContent ContentDialogContent => (ContentDialogContent) Content;

    internal void InitializeComponent(ContentDialogContent component)
    {
        content = component;
        content.PrimaryButtonClick += OnPrimaryButtonClick;
        content.SecondaryButtonClick += OnSecondaryButtonClick;
        content.CloseButtonClick += OnCloseButtonClick;
        Content = content;
    }

    private void InitializeFlyout()
    {
        Closed += OnClosed;
    }

    public event TypedEventHandler<ContentDialogFlyout, CancelEventArgs>? PrimaryButtonClick;
    public event TypedEventHandler<ContentDialogFlyout, CancelEventArgs>? SecondaryButtonClick;
    public event TypedEventHandler<ContentDialogFlyout, CancelEventArgs>? CloseButtonClick;

    public ContentDialogResult Result { get; private set; }

    #region ContentDialogContent properties

    /// <summary>
    /// 此 DialogTitle 表示对话框标题部分的内容，可以是文本也可以是 UI。
    /// </summary>
    [RelayDependencyProperty("ContentDialogContent.Title")]
    public partial object? DialogTitle { get; set; }

    /// <summary>
    /// 此 DialogContent 表示对话框正文部分的内容，而不是整个 Flyout 的内容。
    /// </summary>
    [RelayDependencyProperty("ContentDialogContent.Content")]
    public partial object? DialogContent { get; set; }

    [RelayDependencyProperty("ContentDialogContent.Foreground")]
    public partial Brush? Foreground { get; set; }

    [RelayDependencyProperty("ContentDialogContent.Background")]
    public partial Brush? Background { get; set; }

    [RelayDependencyProperty("ContentDialogContent.BorderBrush")]
    public partial Brush? BorderBrush { get; set; }

    [RelayDependencyProperty("ContentDialogContent.BorderThickness")]
    public partial Thickness BorderThickness { get; set; }

    [RelayDependencyProperty("ContentDialogContent.FlowDirection")]
    public partial FlowDirection FlowDirection { get; set; }

    [RelayDependencyProperty("ContentDialogContent.TitleTemplate")]
    public partial DataTemplate? TitleTemplate { get; set; }

    [RelayDependencyProperty("ContentDialogContent.ContentTemplate")]
    public partial DataTemplate? ContentTemplate { get; set; }

    [RelayDependencyProperty("ContentDialogContent.PrimaryButtonText")]
    public partial string? PrimaryButtonText { get; set; }

    [RelayDependencyProperty("ContentDialogContent.SecondaryButtonText")]
    public partial string? SecondaryButtonText { get; set; }

    [RelayDependencyProperty("ContentDialogContent.CloseButtonText")]
    public partial string? CloseButtonText { get; set; }

    [RelayDependencyProperty("ContentDialogContent.IsPrimaryButtonEnabled")]
    public partial bool IsPrimaryButtonEnabled { get; set; }

    [RelayDependencyProperty("ContentDialogContent.IsSecondaryButtonEnabled")]
    public partial bool IsSecondaryButtonEnabled { get; set; }

    [RelayDependencyProperty("ContentDialogContent.DefaultButton")]
    public partial ContentDialogButton DefaultButton { get; set; }

    [RelayDependencyProperty("ContentDialogContent.PrimaryButtonStyle")]
    public partial Style? PrimaryButtonStyle { get; set; }

    [RelayDependencyProperty("ContentDialogContent.SecondaryButtonStyle")]
    public partial Style? SecondaryButtonStyle { get; set; }

    [RelayDependencyProperty("ContentDialogContent.CloseButtonStyle")]
    public partial Style? CloseButtonStyle { get; set; }

    [RelayDependencyProperty("ContentDialogContent.RequestedTheme")]
    public partial ElementTheme RequestedTheme { get; set; }

    #endregion

    private void OnPrimaryButtonClick(ContentDialogContent sender, EventArgs e)
    {
        Result = ContentDialogResult.Primary;
        CancelEventArgs args = new();
        PrimaryButtonClick?.Invoke(this, args);
        AfterCommandBarButtonClick(args);
    }

    private void OnSecondaryButtonClick(ContentDialogContent sender, EventArgs e)
    {
        Result = ContentDialogResult.Secondary;
        CancelEventArgs args = new();
        SecondaryButtonClick?.Invoke(this, args);
        AfterCommandBarButtonClick(args);
    }

    private void OnCloseButtonClick(ContentDialogContent sender, EventArgs e)
    {
        Result = ContentDialogResult.None;
        CancelEventArgs args = new();
        CloseButtonClick?.Invoke(this, args);
        AfterCommandBarButtonClick(args);
    }

    private void AfterCommandBarButtonClick(CancelEventArgs args)
    {
        if (args.Cancel)
            return;

        Hide();
    }

    protected override Control CreatePresenter()
    {
        FlyoutPresenter presenter = new()
        {
            Content = ContentDialogContent,
            MinWidth = 0,
            MaxWidth = double.PositiveInfinity,
            MinHeight = 0,
            MaxHeight = double.PositiveInfinity,
            Padding = new Thickness(0)
        };
        ScrollViewer.SetVerticalScrollMode(presenter, ScrollMode.Disabled);
        ScrollViewer.SetVerticalScrollBarVisibility(presenter, ScrollBarVisibility.Disabled);

        // It looks odd but there seems not to be a better practice currently (WindowsAppSDK 1.7).
        // By default, FlyoutPresenter has acrylic background.
        // If ShouldConstrainToRootBounds is true,
        // the flyout will not create a new window and present content in FlyoutPresenter,
        // so we just need to set RequestedTheme for FlyoutPresenter normally.
        // If ShouldConstrainToRootBounds is false,
        // the flyout will create a new window and present content in FlyoutPresenter,
        // which default backgroud will cover on the window, resulting in SystemBackdrop cannot be seen,
        // so we should remove background.
        // After some tests, it seems that SytemBackdrop of window containing FlyoutPresenter only follows Flyout.Target
        // (comes from PlacementTarget paramter of ShowAt method, I simply call it anchor below).
        // But once we set RequestedTheme for FlyoutPresenter,
        // no matter whether the anchor's RequestedTheme is set,
        // SytemBackdrop of the window will be light when system is set to light mode.
        if (ShouldConstrainToRootBounds)
        {
            presenter.RequestedTheme = RequestedTheme;
        }
        else
        {
            originalTargetTheme = Target.RequestedTheme;
            Target.RequestedTheme = RequestedTheme;
            // If SystemBackdrop is null and FlyoutPresenter.Background is null,
            // the backgroud of flyout will be transparent.
            if (SystemBackdrop is not null)
            {
                presenter.Background = null;
            }
        }

        return presenter;
    }

    private void OnClosed(object? sender, object e)
    {
        if (!ShouldConstrainToRootBounds)
        {
            Target.RequestedTheme = originalTargetTheme;
        }
    }

    private ElementTheme originalTargetTheme;

    private static Style DefaultButtonStyle => field ??= (Style) Application.Current.Resources["DefaultButtonStyle"];
}
