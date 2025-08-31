using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Markup;
using Microsoft.UI.Xaml.Media;

using SuGarToolkit.SourceGenerators;

using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

using Windows.Foundation;
using Windows.UI;

namespace SuGarToolkit.Controls.Dialogs;

/// <summary>
/// The window that contains ContentDialogContent.
/// <br/>
/// Never show window immediately after construction,
/// <br/>
/// because the size and position have not been computed.
/// Please use Open() when handling Loaded event.
/// Don't use Activate() only,
/// it will not make window modal even if OverlappedPresenter.IsModal is true.
/// </summary>
[ContentProperty(Name = nameof(DialogContent))]
public partial class ContentDialogWindow : Window
{
    public ContentDialogWindow()
    {
        // Why not use XAML and generated InitializeComponent ?
        // [ContentProperty] affects which property XAML direct content sets on current class and derived classes.
        // But here needs to set Content instead of DialogContent.
        Content = new ContentDialogContent();
        ContentDialogContent.PrimaryButtonClick += OnPrimaryButtonClick;
        ContentDialogContent.SecondaryButtonClick += OnSecondaryButtonClick;
        ContentDialogContent.CloseButtonClick += OnCloseButtonClick;
        ContentDialogContent.Loaded += OnContentLoaded;
        ContentDialogContent.Loaded += (o, e) => DetermineTitleBarButtonForegroundColor();
        ContentDialogContent.ActualThemeChanged += (o, e) => DetermineTitleBarButtonForegroundColor();
        InitializeWindow();
    }

    private ContentDialogWindow(ContentDialogContent? content)
    {
        Content = content;
        InitializeWindow();
    }

    internal static ContentDialogWindow CreateWithoutComponent() => new(null);

    private ContentDialogContent ContentDialogContent => (ContentDialogContent) Content;

    internal void InitializeComponent(ContentDialogContent component)
    {
        component.PrimaryButtonClick += OnPrimaryButtonClick;
        component.SecondaryButtonClick += OnSecondaryButtonClick;
        component.CloseButtonClick += OnCloseButtonClick;
        component.Loaded += OnContentLoaded;
        component.Loaded += DetermineTitleBarButtonForegroundColor;
        component.ActualThemeChanged += DetermineTitleBarButtonForegroundColor;

        Content = component;

        Closed += (sender, args) =>
        {
            ContentDialogContent.PrimaryButtonClick -= OnPrimaryButtonClick;
            ContentDialogContent.SecondaryButtonClick -= OnSecondaryButtonClick;
            ContentDialogContent.CloseButtonClick -= OnCloseButtonClick;
            ContentDialogContent.Loaded -= OnContentLoaded;
            ContentDialogContent.Loaded -= DetermineTitleBarButtonForegroundColor;
            ContentDialogContent.ActualThemeChanged -= DetermineTitleBarButtonForegroundColor;
        };
    }

    private void InitializeWindow()
    {
        ExtendsContentIntoTitleBar = true;
        _presenter = OverlappedPresenter.CreateForDialog();
        _presenter.IsResizable = true;
        AppWindow.SetPresenter(_presenter);
        AppWindow.Closing += (appWindow, e) => OnClosingRequestedBySystem();
        Activated += OnActivated;
        Closed += OnClosed;
    }

    /// <summary>
    /// When showing accent color in title bar is enabled,
    /// title bar buttons in the default custom title bar in WinUI3
    /// will become white like system title bar.
    /// But there is no accent color background here.
    /// </summary>
    private void DetermineTitleBarButtonForegroundColor()
    {
        switch (ContentDialogContent.ActualTheme)
        {
            case ElementTheme.Light:
                AppWindow.TitleBar.ButtonForegroundColor = Colors.Black;
                break;
            case ElementTheme.Dark:
                AppWindow.TitleBar.ButtonForegroundColor = Colors.White;
                break;
        }
    }

    private void DetermineTitleBarButtonForegroundColor(object sender, object args) => DetermineTitleBarButtonForegroundColor();

    public event TypedEventHandler<ContentDialogWindow, CancelEventArgs>? PrimaryButtonClick;
    public event TypedEventHandler<ContentDialogWindow, CancelEventArgs>? SecondaryButtonClick;
    public event TypedEventHandler<ContentDialogWindow, CancelEventArgs>? CloseButtonClick;

    public event TypedEventHandler<ContentDialogWindow, EventArgs>? Loaded;
    public event TypedEventHandler<ContentDialogWindow, EventArgs>? Opened;

    public bool IsLoaded { get; private set; }

    private void OnActivated(object sender, WindowActivatedEventArgs args)
    {
        if (!ContentDialogContent.IsLoaded)
            return;

        if (args.WindowActivationState is WindowActivationState.Deactivated)
        {
            ContentDialogContent.AfterLostFocus();
        }
        else
        {
            ContentDialogContent.AfterGotFocus();
        }
    }

    /// <summary>
    /// AppWindow.Closing event happens when title bar close button clicked or ALT+F4 pressed.
    /// </summary>
    private void OnClosingRequestedBySystem()
    {
        _parent?.Activate();
        AppWindow.Hide();
    }

    /// <summary>
    /// Close() will not make AppWindow.Closing event happen.
    /// </summary>
    private void OnClosingRequstedByCode()
    {
        _parent?.Activate();
        AppWindow.Hide();
    }

    private void OnClosed(object sender, WindowEventArgs args)
    {
        _parent?.Closed -= OnParentClosed;
    }

    private void OnParentClosed(object sender, WindowEventArgs args)
    {
        _parent = null;
    }

    /// <summary>
    /// Set parent window, whether modal, whether to show at center of parent.
    /// </summary>
    public void SetParent(Window? parent, bool modal = true, bool center = true)
    {
        _center = center;

        if (_parent == parent)
            return;

        _parent?.Closed -= OnParentClosed;
        _parent = parent;
        _parent?.Closed += OnParentClosed;

        if (!modal || parent is null)
            return;

        IntPtr ownerHwnd = parent is null ? IntPtr.Zero : Win32Interop.GetWindowFromWindowId(parent.AppWindow.Id);
        IntPtr selfHwnd = Win32Interop.GetWindowFromWindowId(AppWindow.Id);
        if (IntPtr.Size == 8)  // 64-bit
        {
            SetWindowLongPtr(selfHwnd, -8, ownerHwnd);  // -8 = GWLP_HWNDPARENT
        }
        else // 32-bit
        {
            SetWindowLong(selfHwnd, -8, ownerHwnd); // -8 = GWL_HWNDPARENT
        }

        // IsModal must be set after set parent window; otherwise, it will cause exception.
        _presenter.IsModal = true;
    }

    public ElementTheme RequestedTheme
    {
        get => ContentDialogContent.RequestedTheme;
        set
        {
            ContentDialogContent.RequestedTheme = value;
            AppWindow.TitleBar.PreferredTheme = value switch
            {
                ElementTheme.Light => TitleBarTheme.Light,
                ElementTheme.Dark => TitleBarTheme.Dark,
                _ => TitleBarTheme.UseDefaultAppMode,
            };
        }
    }

    public ContentDialogResult Result { get; private set; }

    /// <summary>
    /// 此 DialogTitle 表示对话框标题部分的内容，可以是文本也可以是 UI。
    /// </summary>
    [RelayProperty("ContentDialogContent.Title")]
    public partial object? DialogTitle { get; set; }

    /// <summary>
    /// 此 DialogContent 表示对话框正文部分的内容，而不是整个窗口的内容。
    /// </summary>
    [RelayProperty("ContentDialogContent.Content")]
    public partial object? DialogContent { get; set; }

    #region ContentDialogContent properties

    [RelayProperty("ContentDialogContent.Foreground")]
    public partial Brush? Foreground { get; set; }

    [RelayProperty("ContentDialogContent.Background")]
    public partial Brush? Background { get; set; }

    [RelayProperty("ContentDialogContent.BorderBrush")]
    public partial Brush? BorderBrush { get; set; }

    [RelayProperty("ContentDialogContent.BorderThickness")]
    public partial Thickness BorderThickness { get; set; }

    [RelayProperty("ContentDialogContent.FlowDirection")]
    public partial FlowDirection FlowDirection { get; set; }

    [RelayProperty("ContentDialogContent.TitleTemplate")]
    public partial DataTemplate? TitleTemplate { get; set; }

    [RelayProperty("ContentDialogContent.ContentTemplate")]
    public partial DataTemplate? ContentTemplate { get; set; }

    [RelayProperty("ContentDialogContent.PrimaryButtonText")]
    public partial string? PrimaryButtonText { get; set; }

    [RelayProperty("ContentDialogContent.SecondaryButtonText")]
    public partial string? SecondaryButtonText { get; set; }

    [RelayProperty("ContentDialogContent.CloseButtonText")]
    public partial string? CloseButtonText { get; set; }

    [RelayProperty("ContentDialogContent.IsPrimaryButtonEnabled")]
    public partial bool IsPrimaryButtonEnabled { get; set; }

    [RelayProperty("ContentDialogContent.IsSecondaryButtonEnabled")]
    public partial bool IsSecondaryButtonEnabled { get; set; }

    [RelayProperty("ContentDialogContent.DefaultButton")]
    public partial ContentDialogButton DefaultButton { get; set; }

    [RelayProperty("ContentDialogContent.PrimaryButtonStyle")]
    public partial Style? PrimaryButtonStyle { get; set; }

    [RelayProperty("ContentDialogContent.SecondaryButtonStyle")]
    public partial Style? SecondaryButtonStyle { get; set; }

    [RelayProperty("ContentDialogContent.CloseButtonStyle")]
    public partial Style? CloseButtonStyle { get; set; }

    #endregion

    private void OnContentLoaded(object sender, RoutedEventArgs e)
    {
        // AppWindow.Resize is inaccurate.
        // AppWindow.ResizeCilent is inaccurate when ExtendsContentInfoTitleBar = false.
        // AppWindow.ResizeCilent is accurate in width but there is an extra height of title bar (30 DIP) when ExtendsContentInfoTitleBar = true.
        // No matter whether ExtendsContentInfoTitleBar, the size is the same after use AppWindow.ResizeCilent.
        AppWindow.ResizeClient(new Windows.Graphics.SizeInt32(
            (int) ((ContentDialogContent.DesiredSize.Width + 1) * ContentDialogContent.XamlRoot.RasterizationScale) + 1,
            (int) ((ContentDialogContent.DesiredSize.Height - 30) * ContentDialogContent.XamlRoot.RasterizationScale) + 1));
        SetTitleBar(ContentDialogContent.TitleArea);

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

        DispatcherQueue.TryEnqueue(() => Loaded?.Invoke(this, EventArgs.Empty));
    }

    public void Open()
    {
        AppWindow.Show();
        DispatcherQueue.TryEnqueue(() => Opened?.Invoke(this, EventArgs.Empty));
    }

    public void OpenAfterLoaded()
    {
        if (IsLoaded)
        {
            Open();
        }
        else
        {
            Loaded += (sender, args) => Open();
        }
    }

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
        {
            Result = ContentDialogResult.None;
            return;
        }
        OnClosingRequstedByCode();
        Close();
    }

    protected static void SizeToXamlRoot(FrameworkElement element, XamlRoot root)
    {
        element.Width = root.Size.Width;
        element.Height = root.Size.Height;
    }

    protected static Style DefaultButtonStyle => (Style) Application.Current.Resources["DefaultButtonStyle"];
    protected static Color SmokeFillColor => (Color) Application.Current.Resources["SmokeFillColorDefault"];

    // 64-bit systems
    [LibraryImport("user32.dll", EntryPoint = "SetWindowLongPtrW")]
    private static partial IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

    // 32-bit systems
    [LibraryImport("user32.dll", EntryPoint = "SetWindowLongW")]
    private static partial IntPtr SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

    private OverlappedPresenter _presenter = null!;

    private Window? _parent;

    private bool _center;
}
