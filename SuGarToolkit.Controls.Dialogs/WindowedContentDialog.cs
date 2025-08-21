using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;

using System;
using System.Threading.Tasks;

using Windows.Foundation;
using Windows.UI;

namespace SuGarToolkit.Controls.Dialogs;

public class WindowedContentDialog : StandaloneContentDialogBase
{
    public string? WindowTitle { get; set; }
    public SystemBackdrop? SystemBackdrop { get; set; }
    public bool IsTitleBarVisible { get; set; } = true;
    public bool CenterInParent { get; set; } = true;

    /// <summary>
    /// 底部第一个按钮按下时发生。若要取消点击后关闭窗口，设置 ContentDialogWindowButtonClickEventArgs 参数中的 ShouldCloseDialog = false.
    /// </summary>
    public event TypedEventHandler<ContentDialogWindow, ContentDialogWindowButtonClickEventArgs>? PrimaryButtonClick;

    /// <summary>
    /// 底部第二个按钮按下时发生。若要取消点击后关闭窗口，设置 ContentDialogWindowButtonClickEventArgs 参数中的 ShouldCloseDialog = false.
    /// </summary>
    public event TypedEventHandler<ContentDialogWindow, ContentDialogWindowButtonClickEventArgs>? SecondaryButtonClick;

    /// <summary>
    /// 底部关闭按钮按下时发生。若要取消点击后关闭窗口，设置 ContentDialogWindowButtonClickEventArgs 参数中的 ShouldCloseDialog = false.
    /// </summary>
    public event TypedEventHandler<ContentDialogWindow, ContentDialogWindowButtonClickEventArgs>? CloseButtonClick;

    public bool IsModal { get; set; }

    public Window? OwnerWindow { get; set; }

    public override Task<ContentDialogResult> ShowAsync() => ShowAsync(modal: true);

    /// <summary>
    /// 显示对话框窗口，关闭时返回用户选择结果。
    /// <br/>
    /// 不用注意了，窗口关闭时已经脱离了父级控件。
    /// 注意：FrameworkElement 不能多处共享。
    /// 如果 DialogContent 是 FrameworkElement，那么此 FrameworkElement 不能已被其他父级持有，例如 new MainWindow().DialogContent；
    /// 下次更改 DialogContent 前，此弹窗也只能弹出一次，因为每次弹窗都创建一个新的窗口示例，使得同一个 FrameworkElement 被多处共享。
    /// </summary>
    /// <param name="modal">阻塞所属窗口。默认为 true，但是当 OwnerWindow is null 时不会起作用，仍然弹出普通窗口。</param>
    /// <returns>用户选择结果</returns>
    public async Task<ContentDialogResult> ShowAsync(bool modal)
    {
        IsModal = modal;

        ContentDialogWindow dialogWindow = new()
        {
            Title = WindowTitle,
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

            SystemBackdrop = SystemBackdrop,
            RequestedTheme = RequestedTheme
        };

        dialogWindow.PrimaryButtonClick += PrimaryButtonClick;
        dialogWindow.SecondaryButtonClick += SecondaryButtonClick;
        dialogWindow.CloseButtonClick += CloseButtonClick;

        dialogWindow.SetParent(OwnerWindow, IsModal, CenterInParent);

        if (!IsTitleBarVisible)
        {
            dialogWindow.AppWindow.TitleBar.PreferredHeightOption = Microsoft.UI.Windowing.TitleBarHeightOption.Collapsed;
        }

        if (DisableBehind && OwnerWindow?.Content is Control control)
        {
            bool isOriginallyEnabled = control.IsEnabled;
            dialogWindow.Opened += (window, e) =>
            {
                control.IsEnabled = false;
            };
            dialogWindow.Closed += (o, e) =>
            {
                control.IsEnabled = isOriginallyEnabled;
            };
        }

        if (SmokeLayerKind is not ContentDialogSmokeLayerKind.None && OwnerWindow?.Content?.XamlRoot is not null)
        {
            Popup behindOverlayPopup = new()
            {
                XamlRoot = OwnerWindow.Content.XamlRoot,
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
                SizeToXamlRoot(darkLayer, OwnerWindow.Content.XamlRoot);
                behindOverlayPopup.Child = darkLayer;

                void OnOwnerWindowSizeChanged(object sender, WindowSizeChangedEventArgs args) => SizeToXamlRoot(darkLayer, OwnerWindow.Content.XamlRoot);
                dialogWindow.Opened += (o, e) =>
                {
                    behindOverlayPopup.IsOpen = true;
                    behindOverlayPopup.Child.Opacity = 1.0;
                    OwnerWindow.SizeChanged += OnOwnerWindowSizeChanged;
                };
                dialogWindow.Closed += async (o, e) =>
                {
                    behindOverlayPopup.Child.Opacity = 0.0;
                    await Task.Delay(behindOverlayPopup.Child.OpacityTransition.Duration);
                    behindOverlayPopup.IsOpen = false;
                    OwnerWindow.SizeChanged -= OnOwnerWindowSizeChanged;
                };
            }
            else if (SmokeLayerKind is ContentDialogSmokeLayerKind.Custom && CustomSmokeLayer is not null)
            {
                behindOverlayPopup.Child = CustomSmokeLayer;

                dialogWindow.Opened += (o, e) =>
                {
                    behindOverlayPopup.IsOpen = true;
                    behindOverlayPopup.Child.Opacity = 1.0;
                };
                dialogWindow.Closed += async (o, e) =>
                {
                    behindOverlayPopup.Child.Opacity = 0.0;
                    await Task.Delay(behindOverlayPopup.Child.OpacityTransition?.Duration ?? new TimeSpan(0));
                    behindOverlayPopup.IsOpen = false;
                    behindOverlayPopup.Child = null;  // remove CustomSmokeLayer from visual tree
                };
            }
        }

        TaskCompletionSource<ContentDialogResult> resultCompletionSource = new();
        dialogWindow.Loaded += (window, e) => window.Open();
        dialogWindow.Closed += (o, e) => resultCompletionSource.SetResult(dialogWindow.Result);
        return await resultCompletionSource.Task;
    }
}
