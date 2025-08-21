using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;

using System;
using System.Threading.Tasks;

using Windows.Foundation;

namespace SuGarToolkit.Controls.Dialogs;

public class FlyoutContentDialog : StandaloneContentDialogBase
{
    /// <summary>
    /// Doesn't work when ShouldConstrainToRootBounds = true.
    /// </summary>
    public SystemBackdrop? SystemBackdrop { get; set; }

    /// <summary>
    /// Will be passed to Flyout.ShowAt
    /// </summary>
    public required FrameworkElement PlacementTarget { get; set; }

    public bool ShouldConstrainToRootBounds { get; set; }

    public FlyoutPlacementMode Placement { get; set; }

    /// <summary>
    /// 底部第一个按钮按下时发生。若要取消点击后关闭窗口，设置 ContentDialogFlyoutButtonClickEventArgs 参数中的 ShouldCloseDialog = true.
    /// </summary>
    public event TypedEventHandler<ContentDialogFlyout, ContentDialogFlyoutButtonClickEventArgs>? PrimaryButtonClick;

    /// <summary>
    /// 底部第二个按钮按下时发生。若要取消点击后关闭窗口，设置 ContentDialogFlyoutButtonClickEventArgs 参数中的 ShouldCloseDialog = true.
    /// </summary>
    public event TypedEventHandler<ContentDialogFlyout, ContentDialogFlyoutButtonClickEventArgs>? SecondaryButtonClick;

    /// <summary>
    /// 底部关闭按钮按下时发生。若要取消点击后关闭窗口，设置 ContentDialogFlyoutButtonClickEventArgs 参数中的 ShouldCloseDialog = true.
    /// </summary>
    public event TypedEventHandler<ContentDialogFlyout, ContentDialogFlyoutButtonClickEventArgs>? CloseButtonClick;

    /// <summary>
    /// 显示对话框窗口，关闭时返回用户选择结果。
    /// <br/>
    /// 不用注意了，窗口关闭时已经脱离了父级控件。
    /// 注意：FrameworkElement 不能多处共享。
    /// 如果 DialogContent 是 FrameworkElement，那么此 FrameworkElement 不能已被其他父级持有，例如 new MainWindow().DialogContent；
    /// 下次更改 DialogContent 前，此弹窗也只能弹出一次，因为每次弹窗都创建一个新的窗口示例，使得同一个 FrameworkElement 被多处共享。
    /// </summary>
    /// <returns>用户选择结果</returns>
    public override async Task<ContentDialogResult> ShowAsync()
    {
        ContentDialogFlyout dialogFlyout = new()
        {
            DialogTitle = Title,
            DialogContent = Content,

            PrimaryButtonText = PrimaryButtonText,
            SecondaryButtonText = SecondaryButtonText,
            CloseButtonText = CloseButtonText,
            DefaultButton = DefaultButton,
            IsPrimaryButtonEnabled = IsPrimaryButtonEnabled,
            IsSecondaryButtonEnabled = IsSecondaryButtonEnabled,

            PrimaryButtonStyle = PrimaryButtonStyle,
            SecondaryButtonStyle = SecondaryButtonStyle,
            CloseButtonStyle = CloseButtonStyle,

            Placement = Placement,
            SystemBackdrop = SystemBackdrop,
            RequestedTheme = RequestedTheme
        };

        dialogFlyout.PrimaryButtonClick += PrimaryButtonClick;
        dialogFlyout.SecondaryButtonClick += SecondaryButtonClick;
        dialogFlyout.CloseButtonClick += CloseButtonClick;

        if (DisableBehind && PlacementTarget.XamlRoot.Content is Control control)
        {
            bool isOriginallyEnabled = control.IsEnabled;
            dialogFlyout.Opened += (window, e) =>
            {
                control.IsEnabled = false;
            };
            dialogFlyout.Closed += (o, e) =>
            {
                control.IsEnabled = isOriginallyEnabled;
            };
        }

        if (SmokeLayerKind is not ContentDialogSmokeLayerKind.None && PlacementTarget.XamlRoot is not null)
        {
            Popup behindOverlayPopup = new()
            {
                XamlRoot = PlacementTarget.XamlRoot,
                RequestedTheme = RequestedTheme
            };
            if (SmokeLayerKind is ContentDialogSmokeLayerKind.Darken)
            {
                Rectangle darkLayer = new()
                {
                    Opacity = 0.0,
                    OpacityTransition = new ScalarTransition { Duration = TimeSpan.FromSeconds(0.25) },
                    Fill = new SolidColorBrush(SmokeFillColor),
                };
                SizeToXamlRoot(darkLayer, PlacementTarget.XamlRoot);
                behindOverlayPopup.Child = darkLayer;

                void OnPlacementTargetSizeChanged(object sender, SizeChangedEventArgs args) => SizeToXamlRoot(darkLayer, PlacementTarget.XamlRoot);
                dialogFlyout.Opening += (o, e) =>
                {
                    behindOverlayPopup.IsOpen = true;
                    behindOverlayPopup.Child.Opacity = 1.0;
                    PlacementTarget.SizeChanged += OnPlacementTargetSizeChanged;
                };
                dialogFlyout.Closed += async (o, e) =>
                {
                    behindOverlayPopup.Child.Opacity = 0.0;
                    await Task.Delay(behindOverlayPopup.Child.OpacityTransition.Duration);
                    behindOverlayPopup.IsOpen = false;
                    PlacementTarget.SizeChanged -= OnPlacementTargetSizeChanged;
                };
            }
            else if (SmokeLayerKind is ContentDialogSmokeLayerKind.Custom && CustomSmokeLayer is not null)
            {
                behindOverlayPopup.Child = CustomSmokeLayer;

                dialogFlyout.Opening += (o, e) =>
                {
                    behindOverlayPopup.IsOpen = true;
                    behindOverlayPopup.Child.Opacity = 1.0;
                };
                dialogFlyout.Closed += async (o, e) =>
                {
                    behindOverlayPopup.Child.Opacity = 0.0;
                    await Task.Delay(behindOverlayPopup.Child.OpacityTransition?.Duration ?? new TimeSpan(0));
                    behindOverlayPopup.IsOpen = false;
                    behindOverlayPopup.Child = null;  // remove CustomSmokeLayer from visual tree
                };
            }
        }

        TaskCompletionSource<ContentDialogResult> resultCompletionSource = new();
        dialogFlyout.Closed += (o, e) => resultCompletionSource.SetResult(dialogFlyout.Result);
        dialogFlyout.ShowAt(PlacementTarget);
        return await resultCompletionSource.Task;
    }
}
