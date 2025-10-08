using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Markup;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;

using SuGarToolkit.SourceGenerators;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

using Windows.Foundation;
using Windows.UI;

namespace SuGarToolkit.Controls.Dialogs;

[ContentProperty(Name = nameof(Content))]
public partial class WindowedContentDialog : Control, IStandaloneContentDialog
{
    public WindowedContentDialog()
    {
        DefaultStyleKey = typeof(WindowedContentDialog);
        ContentDialogContent = new ContentDialogContent();
        InitializeRelayDependencyProperties();
    }

    private void InitializeContentDialogWindow()
    {
        ContentDialogWindow = ContentDialogWindow.CreateWithoutComponent();
        ContentDialogWindow.InitializeComponent(ContentDialogContent);
        ContentDialogWindow.RequestedTheme = DetermineTheme();
        ContentDialogWindow.Title = WindowTitle;
        ContentDialogWindow.SystemBackdrop = SystemBackdrop;
        ContentDialogWindow.PrimaryButtonClick += (sender, args) => PrimaryButtonClick?.Invoke(this, args);
        ContentDialogWindow.SecondaryButtonClick += (sender, args) => SecondaryButtonClick?.Invoke(this, args);
        ContentDialogWindow.CloseButtonClick += (sender, args) => CloseButtonClick?.Invoke(this, args);
    }

    public event TypedEventHandler<WindowedContentDialog, CancelEventArgs>? PrimaryButtonClick;
    public event TypedEventHandler<WindowedContentDialog, CancelEventArgs>? SecondaryButtonClick;
    public event TypedEventHandler<WindowedContentDialog, CancelEventArgs>? CloseButtonClick;

    public IList<KeyboardAccelerator> PrimaryButtonKeyboardAccelerators => ContentDialogContent.PrimaryButtonKeyboardAccelerators;
    public IList<KeyboardAccelerator> SecondaryButtonKeyboardAccelerators => ContentDialogContent.SecondaryButtonKeyboardAccelerators;
    public IList<KeyboardAccelerator> CloseButtonKeyboardAccelerators => ContentDialogContent.CloseButtonKeyboardAccelerators;

    #region properties

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

    [DependencyProperty(DefaultValue = true)]
    public partial bool SmokeBehind { get; set; }

    [DependencyProperty]
    public partial string? WindowTitle { get; set; }

    [DependencyProperty]
    public partial SystemBackdrop? SystemBackdrop { get; set; }

    [DependencyProperty(DefaultValue = true)]
    public partial bool IsTitleBarVisible { get; set; }

    [DependencyProperty(DefaultValue = true)]
    public partial bool CenterInParent { get; set; }

    public bool IsModal { get; set; }

    public Window? OwnerWindow { get; set; }

    #endregion

    public ContentDialogResult Result { get; private set; }

    public async Task<ContentDialogResult> ShowAsync(bool isModal)
    {
        IsModal = isModal;
        return await ShowAsync();
    }

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
        InitializeContentDialogWindow();
        ContentDialogWindow.SetParent(OwnerWindow, IsModal, CenterInParent);
        if (!IsTitleBarVisible)
        {
            ContentDialogWindow.AppWindow.TitleBar.PreferredHeightOption = TitleBarHeightOption.Collapsed;
        }

        if (SmokeBehind && OwnerWindow?.Content?.XamlRoot is not null)
        {
            Rectangle darkLayer = new()
            {
                Opacity = 0.0,
                OpacityTransition = new ScalarTransition { Duration = TimeSpan.FromSeconds(0.25) },
                Fill = new SolidColorBrush(SmokeFillColor),
            };
            SizeToXamlRoot(darkLayer, OwnerWindow.Content.XamlRoot);
            Popup behindOverlayPopup = new()
            {
                Child = darkLayer,
                XamlRoot = OwnerWindow.Content.XamlRoot,
                RequestedTheme = RequestedTheme
            };

            void OnOwnerWindowSizeChanged(object sender, WindowSizeChangedEventArgs args) => SizeToXamlRoot(darkLayer, OwnerWindow.Content.XamlRoot);
            ContentDialogWindow.Opened += (o, e) =>
            {
                behindOverlayPopup.IsOpen = true;
                behindOverlayPopup.Child.Opacity = 1.0;
                OwnerWindow.SizeChanged += OnOwnerWindowSizeChanged;
            };
            ContentDialogWindow.Closed += async (o, e) =>
            {
                behindOverlayPopup.Child.Opacity = 0.0;
                await Task.Delay(behindOverlayPopup.Child.OpacityTransition.Duration);
                behindOverlayPopup.IsOpen = false;
                OwnerWindow.SizeChanged -= OnOwnerWindowSizeChanged;
            };
        }

        TaskCompletionSource<ContentDialogResult> resultCompletionSource = new();
        ContentDialogWindow.Loaded += (window, e) => window.Open();
        ContentDialogWindow.Closed += (o, e) => resultCompletionSource.SetResult(ContentDialogWindow.Result);
        Result = await resultCompletionSource.Task;
        return Result;
    }

    /// <summary>
    /// ElementTheme.Default is treated as following owner window
    /// </summary>
    public ElementTheme DetermineTheme()
    {
        if (RequestedTheme is not ElementTheme.Default)
            return RequestedTheme;

        if (OwnerWindow?.Content is FrameworkElement element)
            return element.ActualTheme;

        return RequestedTheme;
    }

    protected static void SizeToXamlRoot(FrameworkElement element, XamlRoot root)
    {
        element.Width = root.Size.Width;
        element.Height = root.Size.Height;
    }

    [DisallowNull]
    private ContentDialogContent ContentDialogContent { get; init; }
    private ContentDialogWindow ContentDialogWindow { get; set; }

    protected static Style DefaultButtonStyle => (Style) Application.Current.Resources["DefaultButtonStyle"];
    protected static Color SmokeFillColor => (Color) Application.Current.Resources["SmokeFillColorDefault"];
}
