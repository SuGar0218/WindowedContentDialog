using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

using Windows.Foundation;

namespace SuGarToolkit.Controls.Dialogs;

/// <summary>
/// 容纳 ContentDialog 的窗口，但是通常不直接用这个，而是用 WindowedContentDialog.
/// <br/>
/// 请不要在此调用 Activate 或其他直接显示窗口的操作，因为只有在 ShowAsync 中才会初始化显示内容。
/// <br/>
/// 关于初始化：
/// <br/>
/// 初始化时按照最大宽高限制尽可能小地显示（这与ContentDialog行为相同），窗口缩小至紧紧包裹内容。
/// 窗口弹出后用户可以调整窗口大小，并且内容总是填满窗口。
/// 这需要在首次加载时，调整窗口大小，然后把内容对齐方式改为拉伸，因此内容和文字只能在初始化时设置一次，否则后续不好判断完成设置的时机。
/// </summary>
public sealed partial class ContentDialogWindow : Window
{
    public ContentDialogWindow() : base()
    {
        ExtendsContentIntoTitleBar = true;
        Activated += OnActivated;
        presenter = (OverlappedPresenter) AppWindow.Presenter;
        presenter.IsMinimizable = false;
        presenter.IsMaximizable = false;

        Closed += (o, e) =>
        {
            content?.Content = null;  // 窗口关闭后，要让对话框与其子元素脱离，避免再次弹出窗口时因 FrameworkElement 不能被多处共享而崩溃。
        };
    }

    public event TypedEventHandler<ContentDialogWindow, ContentDialogWindowButtonClickEventArgs> PrimaryButtonClick;
    public event TypedEventHandler<ContentDialogWindow, ContentDialogWindowButtonClickEventArgs> SecondaryButtonClick;
    public event TypedEventHandler<ContentDialogWindow, ContentDialogWindowButtonClickEventArgs> CloseButtonClick;

    private void OnActivated(object sender, WindowActivatedEventArgs args)
    {
        if (args.WindowActivationState is WindowActivationState.Deactivated)
        {
            content.AfterLostFocus();
        }
        else
        {
            content.AfterGotFocus();
        }
    }

    /// <summary>
    /// 显示对话框窗口，关闭时返回用户选择结果。
    /// <br/>
    /// 
    /// </summary>
    /// <param name="modal">阻塞所属窗口。默认为 true，但是当 OwnerWindow is null 时不会起作用，仍然弹出普通窗口。</param>
    /// <returns>用户选择结果</returns>
    public async Task<ContentDialogResult> ShowAsync()
    {
        AppWindow.TitleBar.PreferredTheme = RequestedTheme switch
        {
            ElementTheme.Default => TitleBarTheme.UseDefaultAppMode,
            ElementTheme.Light => TitleBarTheme.Light,
            ElementTheme.Dark => TitleBarTheme.Dark,
            _ => TitleBarTheme.UseDefaultAppMode,
        };

        // 必须延迟到准备弹出窗口时初始化，因为只在第一次初始化时不拉伸，初始化后需要改为拉伸以跟随窗口大小。
        content = new ContentDialogContent()
        {
            Title = Title,
            Content = Content,

            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,

            PrimaryButtonText = PrimaryButtonText,
            SecondaryButtonText = SecondaryButtonText,
            CloseButtonText = CloseButtonText,
            DefaultButton = DefaultButton,
            IsPrimaryButtonEnabled = IsPrimaryButtonEnabled,
            IsSecondaryButtonEnabled = IsSecondaryButtonEnabled,

            PrimaryButtonStyle = PrimaryButtonStyle,
            SecondaryButtonStyle = SecondaryButtonStyle,
            CloseButtonStyle = CloseButtonStyle,

            RequestedTheme = RequestedTheme,

            MinWidth = (double) Application.Current.Resources["ContentDialogMinWidth"],
            MinHeight = (double) Application.Current.Resources["ContentDialogMinHeight"],
            MaxWidth = (double) Application.Current.Resources["ContentDialogMaxWidth"],
            MaxHeight = (double) Application.Current.Resources["ContentDialogMaxHeight"],
        };
        content.Loaded += DialogLoaded;
        content.CloseButtonClick += OnCloseButtonClick;
        content.PrimaryButtonClick += OnPrimaryButtonClick;
        content.SecondaryButtonClick += OnSecondaryButtonClick;
        base.Content = content;
        using SemaphoreSlim closed = new(0);
        void ClosedEventHandler(object sender, WindowEventArgs args) => DispatcherQueue.TryEnqueue(() => closed.Release());
        Closed += ClosedEventHandler;
        await closed.WaitAsync();  // await 过程中，会开始加载根元素，加载完成后进入 DialogLoaded，在此最后 AppWindow.Show();
        Closed -= ClosedEventHandler;
        return Result;
    }

    public bool IsModal { get; set; }

    public Window? OwnerWindow
    {
        get => field;
        set
        {
            if (field == value || (field = value) is null)
                return;

            IntPtr ownerHwnd = Win32Interop.GetWindowFromWindowId(OwnerWindow!.AppWindow.Id);
            IntPtr ownedHwnd = Win32Interop.GetWindowFromWindowId(AppWindow.Id);

            if (IntPtr.Size == 8)  // Check if the system is 64-bit
            {
                SetWindowLongPtr(ownedHwnd, -8, ownerHwnd);  // -8 = GWLP_HWNDPARENT
            }
            else // 32-bit system
            {
                SetWindowLong(ownedHwnd, -8, ownerHwnd); // -8 = GWL_HWNDPARENT
            }
            //presenter.IsModal = true;  // 必须在设置好父窗口之后设置，否则将因为没有父窗口引发异常。

            void OnClosed(object sender, WindowEventArgs args) => OwnerWindow!.Activate();
            Closed += OnClosed;  // 弹窗关闭后，激活所属窗口。
            OwnerWindow.Closed += (o, e) =>  // 如果所属窗口直接被关闭，则不执行上述操作，并且关闭自己。
            {
                Closed -= OnClosed;
                Close();
            };
        }
    }

    public ElementTheme RequestedTheme { get; set; } = ElementTheme.Default;
    public Brush? Foreground { get; set; } = (Brush) Application.Current.Resources["ApplicationForegroundThemeBrush"];
    public Brush? Background { get; set; }
    public Brush? BorderBrush { get; set; }
    public Thickness BorderThickness { get; set; }
    public CornerRadius CornerRadius { get; set; }
    public FlowDirection FlowDirection { get; set; }
    public DataTemplate? TitleTemplate { get; set; }
    public DataTemplate? ContentTemplate { get; set; }
    public string? PrimaryButtonText { get; set; }
    public string? SecondaryButtonText { get; set; }
    public string? CloseButtonText { get; set; }
    public bool IsPrimaryButtonEnabled { get; set; }
    public bool IsSecondaryButtonEnabled { get; set; }
    public ContentDialogButton DefaultButton { get; set; } = ContentDialogButton.Close;
    public Style? PrimaryButtonStyle { get; set; }
    public Style? SecondaryButtonStyle { get; set; }
    public Style? CloseButtonStyle { get; set; }

    public ContentDialogResult Result { get; private set; }

    public new object? Content { get; set; }  // 此 Content 表示对话框内容

    /// <summary>
    /// 初始时设置好 Min/Max Width/Height，并且横竖不拉伸，为了让对话框出现时的尺寸符合 ContentDialog 的行为。
    /// 窗口大小适应后，改为横竖都可拉伸，取消 Max Width/Height 限制，以跟随窗口大小。
    /// </summary>
    private void DialogLoaded(object sender, RoutedEventArgs e)
    {
        AppWindow.ResizeClient(new Windows.Graphics.SizeInt32(
            (int) (content.ActualWidth * content.XamlRoot.RasterizationScale),
            (int) (content.ActualHeight * content.XamlRoot.RasterizationScale) - 30));
        content.HorizontalAlignment = HorizontalAlignment.Stretch;
        content.VerticalAlignment = VerticalAlignment.Stretch;
        content.MaxHeight = double.PositiveInfinity;
        content.MaxWidth = double.PositiveInfinity;
        SetTitleBar(content.TitleArea);

        if (OwnerWindow is null)  // 在屏幕中间显示
        {
            presenter.IsModal = false;
            DisplayArea displayArea = DisplayArea.GetFromWindowId(AppWindow.Id, DisplayAreaFallback.Primary);
            AppWindow.Move(new Windows.Graphics.PointInt32(
                (displayArea.OuterBounds.Width - AppWindow.Size.Width) / 2,
                (displayArea.OuterBounds.Height - AppWindow.Size.Height) / 2));
        }
        else  // 在所属窗口中间显示
        {
            presenter.IsModal = IsModal;
            AppWindow.Move(new Windows.Graphics.PointInt32(
                OwnerWindow.AppWindow.Position.X + (OwnerWindow.AppWindow.Size.Width - AppWindow.Size.Width) / 2,
                OwnerWindow.AppWindow.Position.Y + (OwnerWindow.AppWindow.Size.Height - AppWindow.Size.Height) / 2));
        }
        AppWindow.Show();
    }

    private void OnCloseButtonClick(object sender, RoutedEventArgs e)
    {
        Result = ContentDialogResult.None;
        ContentDialogWindowButtonClickEventArgs args = new() { Cancel = false };
        CloseButtonClick?.Invoke(this, args);
        if (args.Cancel)  // 事件处理时取消了操作
        {
            return;
        }
        Close();
    }

    private void OnPrimaryButtonClick(object sender, RoutedEventArgs e)
    {
        Result = ContentDialogResult.Primary;
        ContentDialogWindowButtonClickEventArgs args = new() { Cancel = false };
        PrimaryButtonClick?.Invoke(this, args);
        if (args.Cancel)  // 事件处理时取消了操作
        {
            return;
        }
        Close();
    }

    private void OnSecondaryButtonClick(object sender, RoutedEventArgs e)
    {
        Result = ContentDialogResult.Secondary;
        ContentDialogWindowButtonClickEventArgs args = new() { Cancel = false };
        SecondaryButtonClick?.Invoke(this, args);
        if (args.Cancel)  // 事件处理时取消了操作
        {
            return;
        }
        Close();
    }

    // 64-bit systems
    [LibraryImport("user32.dll", EntryPoint = "SetWindowLongPtrW")]
    private static partial IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

    // 32-bit systems
    [LibraryImport("user32.dll", EntryPoint = "SetWindowLongW")]
    private static partial IntPtr SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

    private ContentDialogContent content;

    private readonly OverlappedPresenter presenter;
}
