using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using SuGarToolkit.SourceGenerators;

namespace SuGarToolkit.Controls.Dialogs;

internal sealed partial class SelectableTextBlock : ContentControl
{
    public SelectableTextBlock()
    {
        DefaultStyleKey = typeof(SelectableTextBlock);
    }

    [DependencyProperty]
    public partial TextWrapping TextWrapping { get; set; }

    public string? Text
    {
        get => (string) GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
        nameof(Text),
        typeof(string),
        typeof(SelectableTextBlock),
        new PropertyMetadata(default(string), (d, e) =>
        {
            SelectableTextBlock self = (SelectableTextBlock) d;
            self.Content = e.NewValue;
        })
    );
}
