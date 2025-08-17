using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;

using SuGarToolkit.SourceGenerators;

using System;

namespace SuGarToolkit.Controls.Dialogs;

internal sealed partial class ContentDialogContent : ContentControl
{
    public ContentDialogContent() : base()
    {
        DefaultStyleKey = typeof(ContentDialogContent);

        VerticalAlignment = VerticalAlignment.Center;
        HorizontalAlignment = HorizontalAlignment.Center;

        CommandSpace = null!;
        PrimaryButton = null!;
        SecondaryButton = null!;
        CloseButton = null!;
        TitleArea = null!;
    }

    private Button PrimaryButton;
    private Button SecondaryButton;
    private Button CloseButton;

    public event RoutedEventHandler? PrimaryButtonClick;
    public event RoutedEventHandler? SecondaryButtonClick;
    public event RoutedEventHandler? CloseButtonClick;

    public UIElement TitleArea { get; private set; }
    public Grid CommandSpace { get; private set; }

    protected override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        TitleArea = (UIElement) GetTemplateChild(nameof(TitleArea));
        CommandSpace = (Grid) GetTemplateChild(nameof(CommandSpace));

        PrimaryButton = (Button) GetTemplateChild(nameof(PrimaryButton));
        SecondaryButton = (Button) GetTemplateChild(nameof(SecondaryButton));
        CloseButton = (Button) GetTemplateChild(nameof(CloseButton));

        PrimaryButton.Click += PrimaryButtonClick;
        SecondaryButton.Click += SecondaryButtonClick;
        CloseButton.Click += CloseButtonClick;

        VisualStateManager.GoToState(this, "DialogShowingWithoutSmokeLayer", false);
        DetermineButtonsVisibilityState();
        DetermineDefaultButtonStates();
        DetermineWidthLimit();
    }

    public void AfterGotFocus()
    {
        DetermineDefaultButtonStates();
    }

    public void AfterLostFocus()
    {
        VisualStateManager.GoToState(this, "NoDefaultButton", false);
    }

    private void DetermineButtonsVisibilityState()
    {
        if (!string.IsNullOrEmpty(PrimaryButtonText) && !string.IsNullOrEmpty(SecondaryButtonText) && !string.IsNullOrEmpty(CloseButtonText))
        {
            VisualStateManager.GoToState(this, "AllVisible", false);
            //IsPrimaryButtonEnabled = true;
            //IsSecondaryButtonEnabled = true;
        }
        else if (!string.IsNullOrEmpty(PrimaryButtonText))
        {
            if (!string.IsNullOrEmpty(SecondaryButtonText))
            {
                VisualStateManager.GoToState(this, "PrimaryAndSecondaryVisible", false);
                //IsPrimaryButtonEnabled = true;
                IsSecondaryButtonEnabled = true;
            }
            else if (!string.IsNullOrEmpty(CloseButtonText))
            {
                VisualStateManager.GoToState(this, "PrimaryAndCloseVisible", false);
                //IsPrimaryButtonEnabled = true;
                IsSecondaryButtonEnabled = false;
            }
            else
            {
                VisualStateManager.GoToState(this, "PrimaryVisible", false);
                //IsPrimaryButtonEnabled = true;
                IsSecondaryButtonEnabled = false;
            }
        }
        else if (!string.IsNullOrEmpty(SecondaryButtonText))
        {
            if (!string.IsNullOrEmpty(CloseButtonText))
            {
                VisualStateManager.GoToState(this, "SecondaryAndCloseVisible", false);
            }
            else
            {
                VisualStateManager.GoToState(this, "SecondaryVisible", false);
            }
            IsPrimaryButtonEnabled = false;
            //IsSecondaryButtonEnabled = true;
        }
        else if (!string.IsNullOrEmpty(CloseButtonText))
        {
            VisualStateManager.GoToState(this, "CloseVisible", false);
        }
        else
        {
            VisualStateManager.GoToState(this, "NoneVisible", false);
            IsPrimaryButtonEnabled = false;
            IsSecondaryButtonEnabled = false;
        }
    }

    private void DetermineDefaultButtonStates()
    {
        switch (DefaultButton)
        {
            case ContentDialogButton.None:
                VisualStateManager.GoToState(this, "NoDefaultButton", false);
                break;
            case ContentDialogButton.Primary:
                VisualStateManager.GoToState(this, "PrimaryAsDefaultButton", false);
                PrimaryButton.KeyboardAccelerators.Add(new KeyboardAccelerator { Key = Windows.System.VirtualKey.Enter });
                PrimaryButton.Focus(FocusState.Programmatic);
                break;
            case ContentDialogButton.Secondary:
                VisualStateManager.GoToState(this, "SecondaryAsDefaultButton", false);
                SecondaryButton.KeyboardAccelerators.Add(new KeyboardAccelerator { Key = Windows.System.VirtualKey.Enter });
                SecondaryButton.Focus(FocusState.Programmatic);
                break;
            case ContentDialogButton.Close:
                VisualStateManager.GoToState(this, "CloseAsDefaultButton", false);
                CloseButton.KeyboardAccelerators.Add(new KeyboardAccelerator { Key = Windows.System.VirtualKey.Enter });
                CloseButton.Focus(FocusState.Programmatic);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Determine min/max width.
    /// Should be used after DetermineDefaultButtonStates() because it depends on visibilies of buttons.
    /// </summary>
    private void DetermineWidthLimit()
    {
        int countButtons = 0;
        double buttonLongestWidth = 0.0;
        double buttonMaxWidth = (double) Application.Current.Resources["ContentDialogButtonMaxWidth"];
        if (PrimaryButton.Visibility is Visibility.Visible)
        {
            PrimaryButton.InvalidateMeasure();
            PrimaryButton.Measure(new Windows.Foundation.Size(double.PositiveInfinity, double.PositiveInfinity));
            buttonLongestWidth = Math.Min(Math.Max(buttonLongestWidth, PrimaryButton.DesiredSize.Width), buttonMaxWidth);
            countButtons++;
        }
        if (SecondaryButton.Visibility is Visibility.Visible)
        {
            SecondaryButton.InvalidateMeasure();
            SecondaryButton.Measure(new Windows.Foundation.Size(double.PositiveInfinity, double.PositiveInfinity));
            buttonLongestWidth = Math.Min(Math.Max(buttonLongestWidth, SecondaryButton.DesiredSize.Width), buttonMaxWidth);
            countButtons++;
        }
        if (CloseButton.Visibility is Visibility.Visible)
        {
            CloseButton.InvalidateMeasure();
            CloseButton.Measure(new Windows.Foundation.Size(double.PositiveInfinity, double.PositiveInfinity));
            buttonLongestWidth = Math.Min(Math.Max(buttonLongestWidth, CloseButton.DesiredSize.Width), buttonMaxWidth);
            countButtons++;
        }

        Thickness padding = (Thickness) Application.Current.Resources["ContentDialogPadding"];
        double expectedWidth = padding.Left + padding.Right;
        expectedWidth += countButtons * buttonLongestWidth;
        expectedWidth += (countButtons - 1) * ((GridLength) Application.Current.Resources["ContentDialogButtonSpacing"]).Value;

        MinWidth = Math.Max(expectedWidth, (double) Application.Current.Resources["ContentDialogMinWidth"]);
        MaxWidth = Math.Max(expectedWidth, (double) Application.Current.Resources["ContentDialogMaxWidth"]);

        Loaded += (o, e) => RemoveSizeLimit();
    }

    private void RemoveSizeLimit()
    {
        MaxWidth = double.PositiveInfinity;
        MaxHeight = double.PositiveInfinity;
        MinWidth = 0.0;
        MinHeight = 0.0;

        VerticalAlignment = VerticalAlignment.Stretch;
        HorizontalAlignment = HorizontalAlignment.Stretch;
    }

    [DependencyProperty]
    public partial object? Title { get; set; }

    [DependencyProperty]
    public partial DataTemplate? TitleTemplate { get; set; }

    [DependencyProperty]
    public partial string? PrimaryButtonText { get; set; }

    [DependencyProperty]
    public partial string? SecondaryButtonText { get; set; }

    [DependencyProperty]
    public partial string? CloseButtonText { get; set; }

    [DependencyProperty<bool>(DefaultValue = true)]
    public partial bool IsPrimaryButtonEnabled { get; set; }

    [DependencyProperty<bool>(DefaultValue = true)]
    public partial bool IsSecondaryButtonEnabled { get; set; }

    [DependencyProperty<ContentDialogButton>(DefaultValue = ContentDialogButton.Close)]
    public partial ContentDialogButton DefaultButton { get; set; }

    [DependencyProperty(DefaultValueName = nameof(DefaultButtonStyle))]
    public partial Style? PrimaryButtonStyle { get; set; }

    [DependencyProperty(DefaultValueName = nameof(DefaultButtonStyle))]
    public partial Style? SecondaryButtonStyle { get; set; }

    [DependencyProperty(DefaultValueName = nameof(DefaultButtonStyle))]
    public partial Style? CloseButtonStyle { get; set; }

    private static Style DefaultButtonStyle => (Style) Application.Current.Resources["DefaultButtonStyle"];
}
