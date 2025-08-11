using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

using System.Threading.Tasks;

using Windows.Foundation;

namespace SuGarToolkit.Controls.Dialogs;

public class WindowedContentDialog
{
    public string? Title { get; set; }
    public object? Content { get; set; }

    public ElementTheme RequestedTheme { get; set; } = ElementTheme.Default;
    public SystemBackdrop? SystemBackdrop { get; set; } = new MicaBackdrop();
    public Brush Foreground { get; set; } = (Brush) Application.Current.Resources["ApplicationForegroundThemeBrush"];
    public Brush? Background { get; set; }
    public Brush? BorderBrush { get; set; }
    public Thickness BorderThickness { get; set; }
    public CornerRadius CornerRadius { get; set; }
    public FlowDirection FlowDirection { get; set; }
    public DataTemplate? TitleTemplate { get; set; }
    public DataTemplate? ContentTemplate { get; set; }
    public string PrimaryButtonText { get; set; } = string.Empty;
    public string SecondaryButtonText { get; set; } = string.Empty;
    public string CloseButtonText { get; set; } = string.Empty;
    public bool IsPrimaryButtonEnabled { get; set; } = true;
    public bool IsSecondaryButtonEnabled { get; set; } = true;
    public ContentDialogButton DefaultButton { get; set; } = ContentDialogButton.Close;
    public bool IsTitleBarVisible { get; set; } = true;
    public bool CenterInParent { get; set; } = true;

    public Style PrimaryButtonStyle { get; set; } = DefaultButtonStyle;
    public Style SecondaryButtonStyle { get; set; } = DefaultButtonStyle;
    public Style CloseButtonStyle { get; set; } = DefaultButtonStyle;

    /// <summary>
    /// 底部第一个按钮按下时发生。若要取消点击后关闭窗口，设置 ContentDialogWindowButtonClickEventArgs 参数中的 Cancel = true.
    /// </summary>
    public event TypedEventHandler<ContentDialogWindow, ContentDialogWindowButtonClickEventArgs>? PrimaryButtonClick;

    /// <summary>
    /// 底部第二个按钮按下时发生。若要取消点击后关闭窗口，设置 ContentDialogWindowButtonClickEventArgs 参数中的 Cancel = true.
    /// </summary>
    public event TypedEventHandler<ContentDialogWindow, ContentDialogWindowButtonClickEventArgs>? SecondaryButtonClick;

    /// <summary>
    /// 底部关闭按钮按下时发生。若要取消点击后关闭窗口，设置 ContentDialogWindowButtonClickEventArgs 参数中的 Cancel = true.
    /// </summary>
    public event TypedEventHandler<ContentDialogWindow, ContentDialogWindowButtonClickEventArgs>? CloseButtonClick;

    public Window? OwnerWindow { get; set; }

    /// <summary>
    /// 显示对话框窗口，关闭时返回用户选择结果。
    /// <br/>
    /// 不用注意了，窗口关闭时已经脱离了父级控件。
    /// 注意：FrameworkElement 不能多处共享。
    /// 如果 Content 是 FrameworkElement，那么此 FrameworkElement 不能已被其他父级持有，例如 new MainWindow().Content；
    /// 下次更改 Content 前，此弹窗也只能弹出一次，因为每次弹窗都创建一个新的窗口示例，使得同一个 FrameworkElement 被多处共享。
    /// </summary>
    /// <param name="modal">阻塞所属窗口。默认为 true，但是当 OwnerWindow is null 时不会起作用，仍然弹出普通窗口。</param>
    /// <returns>用户选择结果</returns>
    public async Task<ContentDialogResult> ShowAsync(bool modal = true)
    {
        ContentDialogWindow window = new()
        {
            Title = Title ?? string.Empty,
            Content = Content,

            PrimaryButtonText = PrimaryButtonText,
            SecondaryButtonText = SecondaryButtonText,
            CloseButtonText = CloseButtonText,
            DefaultButton = DefaultButton,
            IsPrimaryButtonEnabled = IsPrimaryButtonEnabled,
            IsSecondaryButtonEnabled = IsSecondaryButtonEnabled,

            PrimaryButtonStyle = PrimaryButtonStyle,
            SecondaryButtonStyle = SecondaryButtonStyle,
            CloseButtonStyle = CloseButtonStyle,

            IsTitleBarVisible = IsTitleBarVisible,

            SystemBackdrop = SystemBackdrop,
            RequestedTheme = RequestedTheme
        };

        window.PrimaryButtonClick += PrimaryButtonClick;
        window.SecondaryButtonClick += SecondaryButtonClick;
        window.CloseButtonClick += CloseButtonClick;

        window.SetParent(OwnerWindow, modal, CenterInParent);

        // 如果是模态窗口，则禁用父窗口内所有控件，强化视觉效果。
        window.Loaded += (window, e) =>
        {
            window.AppWindow.Show();
            if (OwnerWindow?.Content is Control control)
            {
                control.IsEnabled = !((OverlappedPresenter) window.AppWindow.Presenter).IsModal;
            }
        };

        TaskCompletionSource<ContentDialogResult> resultCompletionSource = new();
        window.Closed += (o, e) =>
        {
            resultCompletionSource.SetResult(window.Result);
            if (OwnerWindow?.Content is Control control)
            {
                control.IsEnabled = true;
            }
        };
        return await resultCompletionSource.Task;
    }

    private static Style DefaultButtonStyle => (Style) Application.Current.Resources["DefaultButtonStyle"];
}
