using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

using Windows.Foundation;

namespace SuGarToolkit.Controls.Dialogs;

public sealed partial class ContentDialogFlyout : Flyout
{
    public ContentDialogFlyout()
    {
        InitializeComponent();
        Closed += OnClosed;
    }

    public event TypedEventHandler<ContentDialogFlyout, ContentDialogFlyoutButtonClickEventArgs>? PrimaryButtonClick;
    public event TypedEventHandler<ContentDialogFlyout, ContentDialogFlyoutButtonClickEventArgs>? SecondaryButtonClick;
    public event TypedEventHandler<ContentDialogFlyout, ContentDialogFlyoutButtonClickEventArgs>? CloseButtonClick;

    public ContentDialogResult Result { get; private set; }

    /// <summary>
    /// 此 DialogContent 表示对话框正文部分的内容，而不是整个 Flyout 的内容。
    /// </summary>
    public object? DialogContent
    {
        get => content.Content;
        set => content.Content = value;
    }

    /// <summary>
    /// 此 DialogTitle 表示对话框标题部分的内容，可以是文本也可以是 UI。
    /// </summary>
    public object? DialogTitle
    {
        get => content.Title;
        set => content.Title = value;
    }

    #region ContentDialogContent properties

    public Brush? Foreground
    {
        get => content.Foreground;
        set => content.Foreground = value;
    }

    public Brush? Background
    {
        get => content.Background;
        set => content.Background = value;
    }

    public Brush? BorderBrush
    {
        get => content.BorderBrush;
        set => content.BorderBrush = value;
    }

    public Thickness BorderThickness
    {
        get => content.BorderThickness;
        set => content.BorderThickness = value;
    }

    public FlowDirection FlowDirection
    {
        get => content.FlowDirection;
        set => content.FlowDirection = value;
    }

    public DataTemplate? TitleTemplate
    {
        get => content.TitleTemplate;
        set => content.TitleTemplate = value;
    }

    public DataTemplate? ContentTemplate
    {
        get => content.ContentTemplate;
        set => content.ContentTemplate = value;
    }

    public string? PrimaryButtonText
    {
        get => content.PrimaryButtonText;
        set => content.PrimaryButtonText = value;
    }

    public string? SecondaryButtonText
    {
        get => content.SecondaryButtonText;
        set => content.SecondaryButtonText = value;
    }

    public string? CloseButtonText
    {
        get => content.CloseButtonText;
        set => content.CloseButtonText = value;
    }

    public bool IsPrimaryButtonEnabled
    {
        get => content.IsPrimaryButtonEnabled;
        set => content.IsPrimaryButtonEnabled = value;
    }

    public bool IsSecondaryButtonEnabled
    {
        get => content.IsSecondaryButtonEnabled;
        set => content.IsSecondaryButtonEnabled = value;
    }

    public ContentDialogButton DefaultButton
    {
        get => content.DefaultButton;
        set => content.DefaultButton = value;
    }

    public Style? PrimaryButtonStyle
    {
        get => content.PrimaryButtonStyle;
        set => content.PrimaryButtonStyle = value;
    }

    public Style? SecondaryButtonStyle
    {
        get => content.SecondaryButtonStyle;
        set => content.SecondaryButtonStyle = value;
    }

    public Style? CloseButtonStyle
    {
        get => content.CloseButtonStyle;
        set => content.CloseButtonStyle = value;
    }

    public ElementTheme RequestedTheme
    {
        get;// => content.RequestedTheme;
        set;// => content.RequestedTheme = value;
    }

    #endregion

    private void OnPrimaryButtonClick(object sender, RoutedEventArgs e)
    {
        Result = ContentDialogResult.Primary;
        ContentDialogFlyoutButtonClickEventArgs args = new();
        PrimaryButtonClick?.Invoke(this, args);
        AfterCommandBarButtonClick(args);
    }

    private void OnSecondaryButtonClick(object sender, RoutedEventArgs e)
    {
        Result = ContentDialogResult.Secondary;
        ContentDialogFlyoutButtonClickEventArgs args = new();
        SecondaryButtonClick?.Invoke(this, args);
        AfterCommandBarButtonClick(args);
    }

    private void OnCloseButtonClick(object sender, RoutedEventArgs e)
    {
        Result = ContentDialogResult.None;
        ContentDialogFlyoutButtonClickEventArgs args = new();
        CloseButtonClick?.Invoke(this, args);
        AfterCommandBarButtonClick(args);
    }

    private void AfterCommandBarButtonClick(ContentDialogFlyoutButtonClickEventArgs args)
    {
        if (args.Cancel)
            return;

        DispatcherQueue.TryEnqueue(Hide);
    }

    protected override Control CreatePresenter()
    {
        FlyoutPresenter presenter = new()
        {
            Content = Content,
            MinWidth = 0,
            MaxWidth = double.PositiveInfinity,
            MinHeight = 0,
            MaxHeight = double.PositiveInfinity,
            Padding = new Thickness(0),
        };

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
