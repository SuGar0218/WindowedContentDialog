using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentDialogWindow;

public class WindowedContentDialog
{
    public string Title { get; set; }
    public object? Content { get; set; }

    public ElementTheme RequestedTheme { get; set; } = ElementTheme.Default;
    public SystemBackdrop? SystemBackdrop { get; set; } = new MicaBackdrop();
    public Brush Foreground { get; set; } = (Brush) Application.Current.Resources["ApplicationForegroundThemeBrush"];
    public Brush Background { get; set; }
    public Brush BorderBrush { get; set; }
    public Thickness BorderThickness { get; set; }
    public CornerRadius CornerRadius { get; set; }
    public FlowDirection FlowDirection { get; set; }
    public DataTemplate TitleTemplate { get; set; }
    public DataTemplate ContentTemplate { get; set; }
    public string PrimaryButtonText { get; set; } = string.Empty;
    public string SecondaryButtonText { get; set; } = string.Empty;
    public string CloseButtonText { get; set; } = string.Empty;
    public bool IsPrimaryButtonEnabled { get; set; } = true;
    public bool IsSecondaryButtonEnabled { get; set; } = true;
    public ContentDialogButton DefaultButton { get; set; } = ContentDialogButton.Close;

    public Style PrimaryButtonStyle { get; set; }
    public Style SecondaryButtonStyle { get; set; }
    public Style CloseButtonStyle { get; set; }

    public Window? OwnerWindow { get; set; }
    public bool IsModel { get; set; }

    /// <summary>
    /// 显示对话框窗口，关闭时返回用户选择结果。
    /// <br/>
    /// 注意：FrameworkElement 不能多处共享。
    /// 如果 Content 是 FrameworkElement，那么此 FrameworkElement 不能已被其他父级持有，例如 new MainWindow().Content；
    /// 下次更改 Content 前，此弹窗也只能弹出一次，因为每次弹窗都创建一个新的窗口示例，使得同一个 FrameworkElement 被多处共享。
    /// </summary>
    /// <param name="modal">阻塞所属窗口。默认为 true，但是当 OwnerWindow is null 时不会起作用，仍然弹出普通窗口。</param>
    /// <returns>用户选择结果</returns>
    public async Task<ContentDialogResult> ShowAsync(bool modal)
    {
        ContentDialogWindow window = new()
        {
            Title = Title,
            Content = Content,

            SystemBackdrop = SystemBackdrop,
            PrimaryButtonText = PrimaryButtonText,
            SecondaryButtonText = SecondaryButtonText,
            CloseButtonText = CloseButtonText,
            DefaultButton = DefaultButton,
            IsPrimaryButtonEnabled = IsPrimaryButtonEnabled,
            IsSecondaryButtonEnabled = IsSecondaryButtonEnabled,

            PrimaryButtonStyle = PrimaryButtonStyle,
            SecondaryButtonStyle = SecondaryButtonStyle,
            CloseButtonStyle = CloseButtonStyle,

            OwnerWindow = OwnerWindow,
            IsModal = modal
        };
        return await window.ShowAsync();
    }
}
