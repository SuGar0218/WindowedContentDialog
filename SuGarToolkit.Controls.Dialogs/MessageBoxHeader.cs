using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using SuGarToolkit.SourceGenerators;

namespace SuGarToolkit.Controls.Dialogs;

public partial class MessageBoxHeader : Control
{
    public MessageBoxHeader()
    {
        DefaultStyleKey = typeof(MessageBoxHeader);
    }

    [DependencyProperty]
    public partial string? Text { get; set; }

    //[DependencyProperty<MessageBoxImage>(DefaultValue = MessageBoxImage.None)]
    //public partial MessageBoxImage Image { get; set; }

    protected override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
        DetermineIconState();
    }

    public MessageBoxImage Image
    {
        get => (MessageBoxImage) GetValue(ImageProperty);
        set => SetValue(ImageProperty, value);
    }

    public static readonly DependencyProperty ImageProperty = DependencyProperty.Register(
        nameof(Image),
        typeof(MessageBoxImage),
        typeof(MessageBoxHeader),
        new PropertyMetadata(MessageBoxImage.None, (d, e) =>
        {
            MessageBoxHeader self = (MessageBoxHeader) d;
            if (self.IsLoaded)
            {
                self.DetermineIconState();
            }
        })
    );

    private void DetermineIconState()
    {
        switch (Image)
        {
            case MessageBoxImage.None:
                VisualStateManager.GoToState(this, "NoIconVisible", false);
                break;
            case MessageBoxImage.Error:
                VisualStateManager.GoToState(this, "Error", false);
                VisualStateManager.GoToState(this, "StandardIconVisible", false);
                break;
            case MessageBoxImage.Question:
                VisualStateManager.GoToState(this, "Questional", false);
                VisualStateManager.GoToState(this, "StandardIconVisible", false);
                break;
            case MessageBoxImage.Warning:
                VisualStateManager.GoToState(this, "Warning", false);
                VisualStateManager.GoToState(this, "StandardIconVisible", false);
                break;
            case MessageBoxImage.Information:
                VisualStateManager.GoToState(this, "Informational", false);
                VisualStateManager.GoToState(this, "StandardIconVisible", false);
                break;
            case MessageBoxImage.Success:
                VisualStateManager.GoToState(this, "Success", false);
                VisualStateManager.GoToState(this, "StandardIconVisible", false);
                break;
            default:
                VisualStateManager.GoToState(this, "NoIconVisible", false);
                break;
        }
    }
}
