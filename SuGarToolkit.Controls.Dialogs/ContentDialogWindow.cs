using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

using System;
using System.Runtime.InteropServices;

using Windows.Foundation;
using Windows.UI;

namespace SuGarToolkit.Controls.Dialogs;

/// <summary>
/// 容纳 ContentDialog 的窗口
/// <br/>
/// 请不要构造后立即显示窗口，因为此时窗口大小和位置尚未完成计算。
/// 请在 Loaded 事件中通过 AppWindow.Show 显示窗口，否则模态窗口的效果不起作用。
/// </summary>
public partial class ContentDialogWindow : Window
{
    public ContentDialogWindow()
    {
        ExtendsContentIntoTitleBar = true;
        _presenter = (OverlappedPresenter) AppWindow.Presenter;
        _presenter.IsMinimizable = false;
        _presenter.IsMaximizable = false;

        AppWindow.Closing += (appWindow, e) => appWindow.Hide();
        Activated += OnActivated;
        Closed += OnClosed;

        _content = new()
        {
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,

            MinWidth = (double) Application.Current.Resources["ContentDialogMinWidth"],
            //MinHeight = (double) Application.Current.Resources["ContentDialogMinHeight"],
            MaxWidth = (double) Application.Current.Resources["ContentDialogMaxWidth"],
            MaxHeight = (double) Application.Current.Resources["ContentDialogMaxHeight"],
        };
        _content.Loaded += DialogLoaded;
        _content.CloseButtonClick += OnCloseButtonClick;
        _content.PrimaryButtonClick += OnPrimaryButtonClick;
        _content.SecondaryButtonClick += OnSecondaryButtonClick;
        base.Content = _content;
    }

    public event TypedEventHandler<ContentDialogWindow, ContentDialogWindowButtonClickEventArgs>? PrimaryButtonClick;
    public event TypedEventHandler<ContentDialogWindow, ContentDialogWindowButtonClickEventArgs>? SecondaryButtonClick;
    public event TypedEventHandler<ContentDialogWindow, ContentDialogWindowButtonClickEventArgs>? CloseButtonClick;

    public event TypedEventHandler<ContentDialogWindow, EventArgs>? Loaded;

    private void OnActivated(object sender, WindowActivatedEventArgs args)
    {
        if (!_content.IsLoaded)
            return;

        if (args.WindowActivationState is WindowActivationState.Deactivated)
        {
            _content.AfterLostFocus();
        }
        else
        {
            _content.AfterGotFocus();
        }
    }

    private void OnClosed(object sender, WindowEventArgs args)
    {
        _parent?.Activate();
    }

    private void OnParentClosed(object sender, WindowEventArgs args)
    {
        _parent?.Closed -= OnParentClosed;
        _parent = null;
        DispatcherQueue.TryEnqueue(Close);
    }

    /// <summary>
    /// 设置父窗口，此窗口会显示在父窗口中间。
    /// </summary>
    /// <param name="parent">父窗口的 Window 对象。如果是相对于桌面，设置为 null，此时模态窗口设置不起作用。</param>
    /// <param name="modal">模态窗口，阻止操作父窗口。</param>
    public void SetParent(Window? parent, bool modal = true, bool center = true)
    {
        _center = center;

        if (_parent == parent)
            return;

        _parent?.Closed -= OnParentClosed;
        _parent = parent;
        _parent?.Closed += OnParentClosed;

        IntPtr ownerHwnd = parent is null ? IntPtr.Zero : Win32Interop.GetWindowFromWindowId(parent.AppWindow.Id);
        IntPtr ownedHwnd = Win32Interop.GetWindowFromWindowId(AppWindow.Id);
        if (IntPtr.Size == 8)  // 64-bit
        {
            SetWindowLongPtr(ownedHwnd, -8, ownerHwnd);  // -8 = GWLP_HWNDPARENT
        }
        else // 32-bit
        {
            SetWindowLong(ownedHwnd, -8, ownerHwnd); // -8 = GWL_HWNDPARENT
        }

        // 必须在设置好父窗口之后设置，否则将因为没有父窗口引发异常。
        _presenter.IsModal = parent is not null && modal;
    }

    public ElementTheme RequestedTheme
    {
        get => _content is not null ? _content.RequestedTheme : field;
        set
        {
            field = value;
            _content.RequestedTheme = value;
            AppWindow.TitleBar.PreferredTheme = value switch
            {
                ElementTheme.Light => TitleBarTheme.Light,
                ElementTheme.Dark => TitleBarTheme.Dark,
                _ => TitleBarTheme.UseDefaultAppMode,
            };
        }
    }

    #region ContentDialogContent properties

    public Brush? Foreground
    {
        get => _content.Foreground;
        set => _content.Foreground = value;
    }

    public Brush? Background
    {
        get => _content.Background;
        set => _content.Background = value;
    }

    public Brush? BorderBrush
    {
        get => _content.BorderBrush;
        set => _content.BorderBrush = value;
    }

    public Thickness BorderThickness
    {
        get => _content.BorderThickness;
        set => _content.BorderThickness = value;
    }

    public FlowDirection FlowDirection
    {
        get => _content.FlowDirection;
        set => _content.FlowDirection = value;
    }

    public DataTemplate? TitleTemplate
    {
        get => _content.TitleTemplate;
        set => _content.TitleTemplate = value;
    }

    public DataTemplate? ContentTemplate
    {
        get => _content.ContentTemplate;
        set => _content.ContentTemplate = value;
    }

    public string? PrimaryButtonText
    {
        get => _content.PrimaryButtonText;
        set => _content.PrimaryButtonText = value;
    }

    public string? SecondaryButtonText
    {
        get => _content.SecondaryButtonText;
        set => _content.SecondaryButtonText = value;
    }

    public string? CloseButtonText
    {
        get => _content.CloseButtonText;
        set => _content.CloseButtonText = value;
    }

    public bool IsPrimaryButtonEnabled
    {
        get => _content.IsPrimaryButtonEnabled;
        set => _content.IsPrimaryButtonEnabled = value;
    }

    public bool IsSecondaryButtonEnabled
    {
        get => _content.IsSecondaryButtonEnabled;
        set => _content.IsSecondaryButtonEnabled = value;
    }

    public ContentDialogButton DefaultButton
    {
        get => _content.DefaultButton;
        set => _content.DefaultButton = value;
    }

    public Style? PrimaryButtonStyle
    {
        get => _content.PrimaryButtonStyle;
        set => _content.PrimaryButtonStyle = value;
    }

    public Style? SecondaryButtonStyle
    {
        get => _content.SecondaryButtonStyle;
        set => _content.SecondaryButtonStyle = value;
    }

    public Style? CloseButtonStyle
    {
        get => _content.CloseButtonStyle;
        set => _content.CloseButtonStyle = value;
    }
    #endregion

    public ContentDialogResult Result { get; private set; }

    /// <summary>
    /// 此 Content 表示对话框正文部分的内容，而不是整个窗口的内容。
    /// </summary>
    public new object? Content
    {
        get => _content.Content;
        set => _content.Content = value;
    }

    public bool IsTitleBarVisible { get; set; } = true;

    /// <summary>
    /// 初始时设置好 Min/Max Width/Height，并且横竖不拉伸，为了让对话框出现时的尺寸符合 ContentDialog 的行为。
    /// 窗口大小适应后，改为横竖都可拉伸，取消 Max Width/Height 限制，以跟随窗口大小。
    /// </summary>
    private void DialogLoaded(object sender, RoutedEventArgs e)
    {
        _presenter.SetBorderAndTitleBar(hasBorder: true, IsTitleBarVisible);
        _content.Title = Title;

        AppWindow.ResizeClient(new Windows.Graphics.SizeInt32(
            (int) (_content.ActualWidth * _content.XamlRoot.RasterizationScale) + 1,
            (int) (_content.ActualHeight * _content.XamlRoot.RasterizationScale) + 1));
        _content.HorizontalAlignment = HorizontalAlignment.Stretch;
        _content.VerticalAlignment = VerticalAlignment.Stretch;
        _content.MaxHeight = double.PositiveInfinity;
        _content.MaxWidth = double.PositiveInfinity;
        SetTitleBar(_content.TitleArea);

        if (_center)
        {
            if (_parent is not null)
            {
                AppWindow.Move(new Windows.Graphics.PointInt32(
                    _parent.AppWindow.Position.X + (_parent.AppWindow.Size.Width - AppWindow.Size.Width) / 2,
                    _parent.AppWindow.Position.Y + (_parent.AppWindow.Size.Height - AppWindow.Size.Height) / 2));
            }
            else
            {
                DisplayArea displayArea = DisplayArea.GetFromWindowId(AppWindow.Id, DisplayAreaFallback.Primary);
                AppWindow.Move(new Windows.Graphics.PointInt32(
                    (displayArea.OuterBounds.Width - AppWindow.Size.Width) / 2,
                    (displayArea.OuterBounds.Height - AppWindow.Size.Height) / 2));
            }
        }

        if (SystemBackdrop is null)
        {
            Background = RequestedTheme switch
            {
                ElementTheme.Light => new SolidColorBrush(Colors.White),
                ElementTheme.Dark => new SolidColorBrush(Color.FromArgb(0xFF, 0x20, 0x20, 0x20)),
                _ => null
            };
        }
        if (SystemBackdrop is null || SystemBackdrop is DesktopAcrylicBackdrop)
        {
            _content.CommandSpace.Background.Opacity = 1.0;
        }
        else
        {
            _content.CommandSpace.Background.Opacity = 0.5;
        }

        Loaded?.Invoke(this, EventArgs.Empty);
    }

    private void OnPrimaryButtonClick(object sender, RoutedEventArgs e)
    {
        Result = ContentDialogResult.Primary;
        ContentDialogWindowButtonClickEventArgs args = new() { Cancel = false };
        PrimaryButtonClick?.Invoke(this, args);
        AfterCommandBarButtonClick(args);
    }

    private void OnSecondaryButtonClick(object sender, RoutedEventArgs e)
    {
        Result = ContentDialogResult.Secondary;
        ContentDialogWindowButtonClickEventArgs args = new() { Cancel = false };
        SecondaryButtonClick?.Invoke(this, args);
        AfterCommandBarButtonClick(args);
    }

    private void OnCloseButtonClick(object sender, RoutedEventArgs e)
    {
        Result = ContentDialogResult.None;
        ContentDialogWindowButtonClickEventArgs args = new() { Cancel = false };
        CloseButtonClick?.Invoke(this, args);
        AfterCommandBarButtonClick(args);
    }

    private void AfterCommandBarButtonClick(ContentDialogWindowButtonClickEventArgs args)
    {
        if (args.Cancel)  // 事件处理时取消了操作
            return;

        AppWindow.Hide();
        DispatcherQueue.TryEnqueue(Close);
    }

    // 64-bit systems
    [LibraryImport("user32.dll", EntryPoint = "SetWindowLongPtrW")]
    private static partial IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

    // 32-bit systems
    [LibraryImport("user32.dll", EntryPoint = "SetWindowLongW")]
    private static partial IntPtr SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

    private readonly ContentDialogContent _content;

    private readonly OverlappedPresenter _presenter;

    private Window? _parent;

    private bool _center;
}
