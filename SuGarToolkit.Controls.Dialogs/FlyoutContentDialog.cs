using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Markup;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;

using SuGarToolkit.SourceGenerators;

using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

using Windows.Foundation;
using Windows.UI;

namespace SuGarToolkit.Controls.Dialogs;

[ContentProperty(Name = nameof(Content))]
public partial class FlyoutContentDialog : Control, IStandaloneContentDialog
{
    public FlyoutContentDialog()
    {
        DefaultStyleKey = typeof(FlyoutContentDialog);
        ContentDialogContent = new ContentDialogContent();
        InitializeContentDialogFlyout();
        Loaded += OnLoaded;
    }

    private void InitializeContentDialogFlyout()
    {
        ContentDialogFlyout = ContentDialogFlyout.CreateWithoutComponent();
        ContentDialogFlyout.InitializeComponent(ContentDialogContent);
        ContentDialogFlyout.PrimaryButtonClick += (sender, args) => PrimaryButtonClick?.Invoke(this, args);
        ContentDialogFlyout.SecondaryButtonClick += (sender, args) => SecondaryButtonClick?.Invoke(this, args);
        ContentDialogFlyout.CloseButtonClick += (sender, args) => CloseButtonClick?.Invoke(this, args);
    }

    #region properties

    /// <summary>
    /// Doesn't work when ShouldConstrainToRootBounds = true.
    /// </summary>
    [RelayProperty("ContentDialogFlyout.SystemBackdrop")]
    public partial SystemBackdrop? SystemBackdrop { get; set; }

    /// <summary>
    /// Will be passed to Flyout.ShowAt. It is required.
    /// </summary>
    [DisallowNull]
    public FrameworkElement PlacementTarget { get; set; }

    [RelayProperty("ContentDialogFlyout.ShouldConstrainToRootBounds")]
    public partial bool ShouldConstrainToRootBounds { get; set; }

    [RelayProperty("ContentDialogFlyout.Placement")]
    public partial FlyoutPlacementMode Placement { get; set; }

    public bool SmokeBehind { get; set; }

    [RelayDependencyProperty("ContentDialogContent.Title")]
    public partial object? Title { get; set; }

    [RelayDependencyProperty("ContentDialogContent.Content")]
    public partial object? Content { get; set; }

    [RelayDependencyProperty("ContentDialogContent.PrimaryButtonText")]
    public partial string? PrimaryButtonText { get; set; }

    [RelayDependencyProperty("ContentDialogContent.SecondaryButtonText")]
    public partial string? SecondaryButtonText { get; set; }

    [RelayDependencyProperty("ContentDialogContent.CloseButtonText")]
    public partial string? CloseButtonText { get; set; }

    [RelayDependencyProperty("ContentDialogContent.TitleTemplate")]
    public partial DataTemplate? TitleTemplate { get; set; }

    [RelayDependencyProperty("ContentDialogContent.ContentTemplate")]
    public partial DataTemplate? ContentTemplate { get; set; }

    [RelayDependencyProperty("ContentDialogContent.ContentTemplateSelector")]
    public partial DataTemplateSelector? ContentTemplateSelector { get; set; }

    [RelayDependencyProperty("ContentDialogContent.DefaultButton")]
    public partial ContentDialogButton DefaultButton { get; set; }

    [RelayDependencyProperty("ContentDialogContent.IsPrimaryButtonEnabled")]
    public partial bool IsPrimaryButtonEnabled { get; set; }

    [RelayDependencyProperty("ContentDialogContent.IsSecondaryButtonEnabled")]
    public partial bool IsSecondaryButtonEnabled { get; set; }

    [RelayDependencyProperty("ContentDialogContent.PrimaryButtonStyle")]
    public partial Style? PrimaryButtonStyle { get; set; }

    [RelayDependencyProperty("ContentDialogContent.SecondaryButtonStyle")]
    public partial Style? SecondaryButtonStyle { get; set; }

    [RelayDependencyProperty("ContentDialogContent.CloseButtonStyle")]
    public partial Style? CloseButtonStyle { get; set; }

    #endregion

    public ContentDialogResult Result { get; private set; }

    public event TypedEventHandler<FlyoutContentDialog, CancelEventArgs>? PrimaryButtonClick;
    public event TypedEventHandler<FlyoutContentDialog, CancelEventArgs>? SecondaryButtonClick;
    public event TypedEventHandler<FlyoutContentDialog, CancelEventArgs>? CloseButtonClick;

    /// <summary>
    /// 显示对话框窗口，关闭时返回用户选择结果。
    /// <br/>
    /// 不用注意了，窗口关闭时已经脱离了父级控件。
    /// 注意：FrameworkElement 不能多处共享。
    /// 如果 DialogContent 是 FrameworkElement，那么此 FrameworkElement 不能已被其他父级持有，例如 new MainWindow().DialogContent；
    /// 下次更改 DialogContent 前，此弹窗也只能弹出一次，因为每次弹窗都创建一个新的窗口示例，使得同一个 FrameworkElement 被多处共享。
    /// </summary>
    /// <returns>用户选择结果</returns>
    public async Task<ContentDialogResult> ShowAsync()
    {
        ContentDialogFlyout.RequestedTheme = DetermineTheme();

        if (SmokeBehind && PlacementTarget.XamlRoot is not null)
        {
            Rectangle darkLayer = new()
            {
                Opacity = 0.0,
                OpacityTransition = new ScalarTransition { Duration = TimeSpan.FromSeconds(0.25) },
                Fill = new SolidColorBrush(SmokeFillColor),
            };
            SizeToXamlRoot(darkLayer, PlacementTarget.XamlRoot);
            Popup behindOverlayPopup = new()
            {
                Child = darkLayer,
                XamlRoot = PlacementTarget.XamlRoot,
                RequestedTheme = RequestedTheme
            };

            void OnPlacementTargetSizeChanged(object sender, SizeChangedEventArgs args) => SizeToXamlRoot(darkLayer, PlacementTarget.XamlRoot);
            ContentDialogFlyout.Opening += (o, e) =>
            {
                behindOverlayPopup.IsOpen = true;
                behindOverlayPopup.Child.Opacity = 1.0;
                PlacementTarget.SizeChanged += OnPlacementTargetSizeChanged;
            };
            ContentDialogFlyout.Closed += async (o, e) =>
            {
                behindOverlayPopup.Child.Opacity = 0.0;
                await Task.Delay(behindOverlayPopup.Child.OpacityTransition.Duration);
                behindOverlayPopup.IsOpen = false;
                PlacementTarget.SizeChanged -= OnPlacementTargetSizeChanged;
            };
        }

        TaskCompletionSource<ContentDialogResult> resultCompletionSource = new();
        ContentDialogFlyout.Closed += (o, e) => resultCompletionSource.SetResult(ContentDialogFlyout.Result);
        ContentDialogFlyout.ShowAt(PlacementTarget);
        Result = await resultCompletionSource.Task;
        ContentDialogFlyout.Content = null;
        InitializeContentDialogFlyout();
        return Result;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        if (PlacementTarget is null && Parent is FrameworkElement parent)
        {
            PlacementTarget = parent;
        }
    }

    /// <summary>
    /// ElementTheme.Default is treated as following owner window
    /// </summary>
    protected ElementTheme DetermineTheme()
    {
        if (RequestedTheme is not ElementTheme.Default)
            return RequestedTheme;

        return PlacementTarget.ActualTheme;
    }

    protected static void SizeToXamlRoot(FrameworkElement element, XamlRoot root)
    {
        element.Width = root.Size.Width;
        element.Height = root.Size.Height;
    }

    private ContentDialogFlyout ContentDialogFlyout { get; set; }
    private ContentDialogContent ContentDialogContent { get; set; }

    protected static Style DefaultButtonStyle => (Style) Application.Current.Resources["DefaultButtonStyle"];
    protected static Color SmokeFillColor => (Color) Application.Current.Resources["SmokeFillColorDefault"];
}
