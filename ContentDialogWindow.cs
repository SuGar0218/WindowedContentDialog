using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace ContentDialogWindow;

/// <summary>
/// 容纳 ContentDialog 的窗口
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
        //InitializeComponent();
        //SystemBackdrop = new MicaBackdrop();
        ExtendsContentIntoTitleBar = true;
        AppWindow.Closing += (o, e) => AppWindow.Hide();
        Activated += OnActivated;
        presenter = (OverlappedPresenter) AppWindow.Presenter;
        presenter.IsMinimizable = false;
        presenter.IsMaximizable = false;

        Closed += (o, e) =>
        {
            dialog?.Content = null;  // 窗口关闭后，对话框与其子元素脱离，避免再次弹出窗口时因 FrameworkElement 不能被多处共享而崩溃。
        };
    }

    private void OnActivated(object sender, WindowActivatedEventArgs args)
    {
        if (args.WindowActivationState is WindowActivationState.Deactivated)
        {
            dialog.AfterLostFocus();
        }
        else
        {
            dialog.AfterGotFocus();
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
        // 必须延迟到准备弹出窗口时初始化，因为只在第一次初始化时不拉伸，初始化后需要改为拉伸以跟随窗口大小。
        dialog = new ContentDialogContent()
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
        dialog.Loaded += DialogLoaded;
        dialog.CloseButtonClick += OnCloseButtonClick;
        dialog.PrimaryButtonClick += OnPrimaryButtonClick;
        dialog.SecondaryButtonClick += OnSecondaryButtonClick;
        base.Content = dialog;
        //using SemaphoreSlim loaded = new(0, 1);
        using SemaphoreSlim closed = new(0, 1);
        //void LoadedEventHandler(object sender, RoutedEventArgs args) => loaded.Release();
        void ClosedEventHandler(object sender, WindowEventArgs args) => closed.Release();
        //dialog.Loaded += LoadedEventHandler;
        Closed += ClosedEventHandler;
        //await loaded.WaitAsync();
        //if (OwnerWindow is null)  // 在屏幕中间显示
        //{
        //    presenter.IsModal = false;
        //    DisplayArea displayArea = DisplayArea.GetFromWindowId(AppWindow.Id, DisplayAreaFallback.Primary);
        //    AppWindow.Move(new Windows.Graphics.PointInt32(
        //        (displayArea.OuterBounds.Width - AppWindow.Size.Width) / 2,
        //        (displayArea.OuterBounds.Height - AppWindow.Size.Height) / 2));
        //}
        //else  // 在所属窗口中间显示
        //{
        //    presenter.IsModal = IsModal;
        //    AppWindow.Move(new Windows.Graphics.PointInt32(
        //        OwnerWindow.AppWindow.Position.X + (OwnerWindow.AppWindow.Size.Width - AppWindow.Size.Width) / 2,
        //        OwnerWindow.AppWindow.Position.Y + (OwnerWindow.AppWindow.Size.Height - AppWindow.Size.Height) / 2));
        //}
        //AppWindow.Show();  // 不调用 Activate 是因为经过测试它不能让模态弹窗的形式生效
        await closed.WaitAsync();
        //dialog.Loaded -= LoadedEventHandler;
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
    public Brush Foreground { get; set; } = (Brush) Application.Current.Resources["ApplicationForegroundThemeBrush"];
    public Brush Background { get; set; }
    public Brush BorderBrush { get; set; }
    public Thickness BorderThickness { get; set; }
    public CornerRadius CornerRadius { get; set; }
    public FlowDirection FlowDirection { get; set; }
    public DataTemplate TitleTemplate { get; set; }
    public DataTemplate ContentTemplate { get; set; }
    public string PrimaryButtonText { get; set; }
    public string SecondaryButtonText { get; set; }
    public string CloseButtonText { get; set; }
    public bool IsPrimaryButtonEnabled { get; set; }
    public bool IsSecondaryButtonEnabled { get; set; }
    public ContentDialogButton DefaultButton { get; set; } = ContentDialogButton.Close;
    public Style PrimaryButtonStyle { get; set; }
    public Style SecondaryButtonStyle { get; set; }
    public Style CloseButtonStyle { get; set; }

    public ContentDialogResult Result { get; private set; }

    public new object? Content { get; set; }

    /// <summary>
    /// 初始时设置好 Min/Max Width/Height，并且横竖不拉伸，为了让对话框出现时的尺寸符合 ContentDialog 的行为。
    /// 窗口大小适应后，改为横竖都可拉伸，取消 Max Width/Height 限制，以跟随窗口大小。
    /// </summary>
    private void DialogLoaded(object sender, RoutedEventArgs e)
    {
        AppWindow.ResizeClient(new Windows.Graphics.SizeInt32(
            (int) (dialog.ActualWidth * dialog.XamlRoot.RasterizationScale),
            (int) (dialog.ActualHeight * dialog.XamlRoot.RasterizationScale) - 30));
        dialog.HorizontalAlignment = HorizontalAlignment.Stretch;
        dialog.VerticalAlignment = VerticalAlignment.Stretch;
        dialog.MaxHeight = double.PositiveInfinity;
        dialog.MaxWidth = double.PositiveInfinity;
        SetTitleBar(dialog.TitleArea);

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
        Close();
        Result = ContentDialogResult.None;
    }

    private void OnPrimaryButtonClick(object sender, RoutedEventArgs e)
    {
        Close();
        Result = ContentDialogResult.Primary;
    }

    private void OnSecondaryButtonClick(object sender, RoutedEventArgs e)
    {
        Close();
        Result = ContentDialogResult.Secondary;
    }

    // 64-bit systems
    [LibraryImport("user32.dll", EntryPoint = "SetWindowLongPtrW")]
    private static partial IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

    // 32-bit systems
    [LibraryImport("user32.dll", EntryPoint = "SetWindowLongW")]
    private static partial IntPtr SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

    private ContentDialogContent dialog;

    private readonly OverlappedPresenter presenter;
}
