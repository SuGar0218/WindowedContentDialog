using Microsoft.UI.Xaml.Controls;

namespace SuGarToolkit.Sample.Dialogs.ViewModels;

public class ContentDialogLikeExampleViewModel
{
    public string Title { get; set; } = "WindowedContentDialog";
    public string PrimaryButtonText { get; set; } = "PrimaryButton";
    public string SecondaryButtonText { get; set; } = "SecondaryButton";
    public string CloseButtonText { get; set; } = "Close";
    public ContentDialogButton DefaultButton { get; set; } = ContentDialogButton.Primary;
    public bool IsChild { get; set; } = true;
    public bool IsModal { get; set; } = true;
    public bool IsTitleBarVisible { get; set; } = true;
    public bool ClickPrimaryButtonToClose { get; set; } = true;
    public bool ClickSecondaryButtonToClose { get; set; } = true;
    public BuiltInSystemBackdropType BackdropType { get; set; } = BuiltInSystemBackdropType.Arcylic;
}
