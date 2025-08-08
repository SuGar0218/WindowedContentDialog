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
/// ���� ContentDialog �Ĵ��ڣ�����ͨ����ֱ��������������� WindowedContentDialog.
/// <br/>
/// �벻Ҫ�ڴ˵��� Activate ������ֱ����ʾ���ڵĲ�������Ϊֻ���� ShowAsync �вŻ��ʼ����ʾ���ݡ�
/// <br/>
/// ���ڳ�ʼ����
/// <br/>
/// ��ʼ��ʱ������������ƾ�����С����ʾ������ContentDialog��Ϊ��ͬ����������С�������������ݡ�
/// ���ڵ������û����Ե������ڴ�С���������������������ڡ�
/// ����Ҫ���״μ���ʱ���������ڴ�С��Ȼ������ݶ��뷽ʽ��Ϊ���죬������ݺ�����ֻ���ڳ�ʼ��ʱ����һ�Σ�������������ж�������õ�ʱ����
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
            content?.Content = null;  // ���ڹرպ�Ҫ�öԻ���������Ԫ�����룬�����ٴε�������ʱ�� FrameworkElement ���ܱ��ദ�����������
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
    /// ��ʾ�Ի��򴰿ڣ��ر�ʱ�����û�ѡ������
    /// <br/>
    /// 
    /// </summary>
    /// <param name="modal">�����������ڡ�Ĭ��Ϊ true�����ǵ� OwnerWindow is null ʱ���������ã���Ȼ������ͨ���ڡ�</param>
    /// <returns>�û�ѡ����</returns>
    public async Task<ContentDialogResult> ShowAsync()
    {
        AppWindow.TitleBar.PreferredTheme = RequestedTheme switch
        {
            ElementTheme.Default => TitleBarTheme.UseDefaultAppMode,
            ElementTheme.Light => TitleBarTheme.Light,
            ElementTheme.Dark => TitleBarTheme.Dark,
            _ => TitleBarTheme.UseDefaultAppMode,
        };

        // �����ӳٵ�׼����������ʱ��ʼ������Ϊֻ�ڵ�һ�γ�ʼ��ʱ�����죬��ʼ������Ҫ��Ϊ�����Ը��洰�ڴ�С��
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
        await closed.WaitAsync();  // await �����У��Ὺʼ���ظ�Ԫ�أ�������ɺ���� DialogLoaded���ڴ���� AppWindow.Show();
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
            //presenter.IsModal = true;  // ���������úø�����֮�����ã�������Ϊû�и����������쳣��

            void OnClosed(object sender, WindowEventArgs args) => OwnerWindow!.Activate();
            Closed += OnClosed;  // �����رպ󣬼����������ڡ�
            OwnerWindow.Closed += (o, e) =>  // �����������ֱ�ӱ��رգ���ִ���������������ҹر��Լ���
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

    public new object? Content { get; set; }  // �� Content ��ʾ�Ի�������

    /// <summary>
    /// ��ʼʱ���ú� Min/Max Width/Height�����Һ��������죬Ϊ���öԻ������ʱ�ĳߴ���� ContentDialog ����Ϊ��
    /// ���ڴ�С��Ӧ�󣬸�Ϊ�����������죬ȡ�� Max Width/Height ���ƣ��Ը��洰�ڴ�С��
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

        if (OwnerWindow is null)  // ����Ļ�м���ʾ
        {
            presenter.IsModal = false;
            DisplayArea displayArea = DisplayArea.GetFromWindowId(AppWindow.Id, DisplayAreaFallback.Primary);
            AppWindow.Move(new Windows.Graphics.PointInt32(
                (displayArea.OuterBounds.Width - AppWindow.Size.Width) / 2,
                (displayArea.OuterBounds.Height - AppWindow.Size.Height) / 2));
        }
        else  // �����������м���ʾ
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
        if (args.Cancel)  // �¼�����ʱȡ���˲���
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
        if (args.Cancel)  // �¼�����ʱȡ���˲���
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
        if (args.Cancel)  // �¼�����ʱȡ���˲���
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
