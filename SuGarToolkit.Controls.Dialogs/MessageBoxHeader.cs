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

    //[DependencyProperty<MessageBoxIcon>(DefaultValue = MessageBoxIcon.None)]
    //public partial MessageBoxIcon Icon { get; set; }

    protected override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
        DetermineIconState();
    }

    public MessageBoxIcon Icon
    {
        get => (MessageBoxIcon) GetValue(ImageProperty);
        set => SetValue(ImageProperty, value);
    }

    public static readonly DependencyProperty ImageProperty = DependencyProperty.Register(
        nameof(Icon),
        typeof(MessageBoxIcon),
        typeof(MessageBoxHeader),
        new PropertyMetadata(MessageBoxIcon.None, (d, e) =>
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
        switch (Icon)
        {
            case MessageBoxIcon.None:
                VisualStateManager.GoToState(this, "NoIconVisible", false);
                break;
            case MessageBoxIcon.Error:
                VisualStateManager.GoToState(this, "Error", false);
                VisualStateManager.GoToState(this, "StandardIconVisible", false);
                break;
            case MessageBoxIcon.Question:
                VisualStateManager.GoToState(this, "Questional", false);
                VisualStateManager.GoToState(this, "StandardIconVisible", false);
                break;
            case MessageBoxIcon.Warning:
                VisualStateManager.GoToState(this, "Warning", false);
                VisualStateManager.GoToState(this, "StandardIconVisible", false);
                break;
            case MessageBoxIcon.Information:
                VisualStateManager.GoToState(this, "Informational", false);
                VisualStateManager.GoToState(this, "StandardIconVisible", false);
                break;
            case MessageBoxIcon.Success:
                VisualStateManager.GoToState(this, "Success", false);
                VisualStateManager.GoToState(this, "StandardIconVisible", false);
                break;
            default:
                VisualStateManager.GoToState(this, "NoIconVisible", false);
                break;
        }
    }
}
