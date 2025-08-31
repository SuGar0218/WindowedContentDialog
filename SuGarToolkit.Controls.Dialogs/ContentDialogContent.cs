using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;

using SuGarToolkit.SourceGenerators;

using System;

using Windows.Foundation;

namespace SuGarToolkit.Controls.Dialogs;

public partial class ContentDialogContent : ContentControl
{
    public ContentDialogContent() : base()
    {
        DefaultStyleKey = typeof(ContentDialogContent);
        Loaded += OnLoaded;
        Unloaded += (o, e) => isCustomMeasureFinishedAfterLoaded = false;
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

    [DependencyProperty(DefaultValue = true)]
    public partial bool IsPrimaryButtonEnabled { get; set; }

    [DependencyProperty(DefaultValue = true)]
    public partial bool IsSecondaryButtonEnabled { get; set; }

    [DependencyProperty(DefaultValue = ContentDialogButton.Close)]
    public partial ContentDialogButton DefaultButton { get; set; }

    [DependencyProperty(DefaultValuePath = nameof(DefaultButtonStyle))]
    public partial Style? PrimaryButtonStyle { get; set; }

    [DependencyProperty(DefaultValuePath = nameof(DefaultButtonStyle))]
    public partial Style? SecondaryButtonStyle { get; set; }

    [DependencyProperty(DefaultValuePath = nameof(DefaultButtonStyle))]
    public partial Style? CloseButtonStyle { get; set; }

    public event TypedEventHandler<ContentDialogContent, EventArgs>? PrimaryButtonClick;
    public event TypedEventHandler<ContentDialogContent, EventArgs>? SecondaryButtonClick;
    public event TypedEventHandler<ContentDialogContent, EventArgs>? CloseButtonClick;

    public UIElement TitleArea { get; private set; }
    public Grid DialogSpace { get; private set; }
    public Grid CommandSpace { get; private set; }

    private Button PrimaryButton;
    private Button SecondaryButton;
    private Button CloseButton;

    protected override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        TitleArea = (UIElement) GetTemplateChild(nameof(TitleArea));
        DialogSpace = (Grid) GetTemplateChild(nameof(DialogSpace));
        CommandSpace = (Grid) GetTemplateChild(nameof(CommandSpace));

        PrimaryButton = (Button) GetTemplateChild(nameof(PrimaryButton));
        SecondaryButton = (Button) GetTemplateChild(nameof(SecondaryButton));
        CloseButton = (Button) GetTemplateChild(nameof(CloseButton));

        PrimaryButton.Click += (sender, args) => PrimaryButtonClick?.Invoke(this, EventArgs.Empty);
        SecondaryButton.Click += (sender, args) => SecondaryButtonClick?.Invoke(this, EventArgs.Empty);
        CloseButton.Click += (sender, args) => CloseButtonClick?.Invoke(this, EventArgs.Empty);
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        buttonsVisibilityState = DetermineButtonsVisibilityState();
        defaultButtonState = DetermineDefaultButtonState();
    }

    /// <summary>
    /// Whether customized measurement in MeasureOverride is needed.
    /// <br/>
    /// This variable is set to avoid redundant calculations.
    /// <br/>
    /// If the first measurement after Loaded is finished, there will be no need for customized measurement until Unloaded.
    /// </summary>
    private bool isCustomMeasureFinishedAfterLoaded;

    protected override Size MeasureOverride(Size availableSize)
    {
        if (isCustomMeasureFinishedAfterLoaded)
            return base.MeasureOverride(availableSize);

        isCustomMeasureFinishedAfterLoaded = IsLoaded;
        return CustomMeasure(availableSize);
    }

    private Size CustomMeasure(Size availableSize)
    {
        int countButtons = 0;
        double buttonLongestWidth = 0.0;
        double buttonMaxWidth = (double) Application.Current.Resources["ContentDialogButtonMaxWidth"];
        if (PrimaryButton.Visibility is Visibility.Visible)
        {
            PrimaryButton.Measure(availableSize);
            buttonLongestWidth = Math.Min(Math.Max(buttonLongestWidth, PrimaryButton.DesiredSize.Width), buttonMaxWidth);
            countButtons++;
        }
        if (SecondaryButton.Visibility is Visibility.Visible)
        {
            SecondaryButton.Measure(availableSize);
            buttonLongestWidth = Math.Min(Math.Max(buttonLongestWidth, SecondaryButton.DesiredSize.Width), buttonMaxWidth);
            countButtons++;
        }
        if (CloseButton.Visibility is Visibility.Visible)
        {
            CloseButton.Measure(availableSize);
            buttonLongestWidth = Math.Min(Math.Max(buttonLongestWidth, CloseButton.DesiredSize.Width), buttonMaxWidth);
            countButtons++;
        }

        double commandSpaceExpectedWidth = CommandSpace.Padding.Left + CommandSpace.Padding.Right
            + countButtons * buttonLongestWidth
            + (countButtons - 1) * ((GridLength) Application.Current.Resources["ContentDialogButtonSpacing"]).Value;

        double minWidth = Math.Max((double) Application.Current.Resources["ContentDialogMinWidth"], commandSpaceExpectedWidth);
        double maxWidth = Math.Max((double) Application.Current.Resources["ContentDialogMaxWidth"], commandSpaceExpectedWidth);
        if (availableSize.Width > maxWidth)
        {
            availableSize.Width = maxWidth;
        }
        Size desiredSize = base.MeasureOverride(availableSize);
        if (desiredSize.Width < minWidth)
        {
            desiredSize.Width = minWidth;
        }
        return desiredSize;
    }

    public void AfterGotFocus()
    {
        VisualStateManager.GoToState(this, defaultButtonState, false);
    }

    public void AfterLostFocus()
    {
        VisualStateManager.GoToState(this, "NoDefaultButton", false);
    }

    private string buttonsVisibilityState = string.Empty;
    private string defaultButtonState = string.Empty;

    private string DetermineButtonsVisibilityState()
    {
        if (!string.IsNullOrEmpty(PrimaryButtonText) && !string.IsNullOrEmpty(SecondaryButtonText) && !string.IsNullOrEmpty(CloseButtonText))
        {
            VisualStateManager.GoToState(this, "AllVisible", false);
            //IsPrimaryButtonEnabled = true;
            //IsSecondaryButtonEnabled = true;
            return "AllVisible";
        }
        else if (!string.IsNullOrEmpty(PrimaryButtonText))
        {
            if (!string.IsNullOrEmpty(SecondaryButtonText))
            {
                VisualStateManager.GoToState(this, "PrimaryAndSecondaryVisible", false);
                //IsPrimaryButtonEnabled = true;
                IsSecondaryButtonEnabled = true;
                return "PrimaryAndSecondaryVisible";
            }
            else if (!string.IsNullOrEmpty(CloseButtonText))
            {
                VisualStateManager.GoToState(this, "PrimaryAndCloseVisible", false);
                //IsPrimaryButtonEnabled = true;
                IsSecondaryButtonEnabled = false;
                return "PrimaryAndCloseVisible";
            }
            else
            {
                VisualStateManager.GoToState(this, "PrimaryVisible", false);
                //IsPrimaryButtonEnabled = true;
                IsSecondaryButtonEnabled = false;
                return "PrimaryVisible";
            }
        }
        else if (!string.IsNullOrEmpty(SecondaryButtonText))
        {
            IsPrimaryButtonEnabled = false;
            if (!string.IsNullOrEmpty(CloseButtonText))
            {
                VisualStateManager.GoToState(this, "SecondaryAndCloseVisible", false);
                return "SecondaryAndCloseVisible";
            }
            else
            {
                VisualStateManager.GoToState(this, "SecondaryVisible", false);
                return "SecondaryAndCloseVisible";
            }
            //IsSecondaryButtonEnabled = true;
        }
        else if (!string.IsNullOrEmpty(CloseButtonText))
        {
            VisualStateManager.GoToState(this, "CloseVisible", false);
            return "CloseVisible";
        }
        else
        {
            VisualStateManager.GoToState(this, "NoneVisible", false);
            IsPrimaryButtonEnabled = false;
            IsSecondaryButtonEnabled = false;
            return "NoneVisible";
        }
    }

    private string DetermineDefaultButtonState()
    {
        switch (DefaultButton)
        {
            case ContentDialogButton.Primary:
                VisualStateManager.GoToState(this, "PrimaryAsDefaultButton", false);
                PrimaryButton.KeyboardAccelerators.Add(new KeyboardAccelerator { Key = Windows.System.VirtualKey.Enter });
                PrimaryButton.Focus(FocusState.Programmatic);
                return "PrimaryAsDefaultButton";
            case ContentDialogButton.Secondary:
                VisualStateManager.GoToState(this, "SecondaryAsDefaultButton", false);
                SecondaryButton.KeyboardAccelerators.Add(new KeyboardAccelerator { Key = Windows.System.VirtualKey.Enter });
                SecondaryButton.Focus(FocusState.Programmatic);
                return "SecondaryAsDefaultButton";
            case ContentDialogButton.Close:
                VisualStateManager.GoToState(this, "CloseAsDefaultButton", false);
                CloseButton.KeyboardAccelerators.Add(new KeyboardAccelerator { Key = Windows.System.VirtualKey.Enter });
                CloseButton.Focus(FocusState.Programmatic);
                return "CloseAsDefaultButton";
            case ContentDialogButton.None:
                VisualStateManager.GoToState(this, "NoDefaultButton", false);
                return "NoDefaultButton";
            default:
                VisualStateManager.GoToState(this, "NoDefaultButton", false);
                return "NoDefaultButton";
        }
    }

    private static Style DefaultButtonStyle => field ??= (Style) Application.Current.Resources["DefaultButtonStyle"];
}
