using System.Windows;
using System.Windows.Controls;

namespace Y.ASIS.App.Ctls.Controls
{
    public class PromptBubbleButton : Button
    {
        static PromptBubbleButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PromptBubbleButton),
                new FrameworkPropertyMetadata(typeof(PromptBubbleButton)));
        }

        public int PromptValue
        {
            get { return (int)GetValue(PromptValueProperty); }
            set { SetValue(PromptValueProperty, value); }
        }

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        public bool IsActive
        {
            get { return (bool)GetValue(IsActiveProperty); }
            set { SetValue(IsActiveProperty, value); }
        }

        public static readonly DependencyProperty PromptValueProperty =
            DependencyProperty.Register(
                "PromptValue",
                typeof(int),
                typeof(PromptBubbleButton),
                new PropertyMetadata(0));

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(
                "CornerRadius",
                typeof(CornerRadius),
                typeof(PromptBubbleButton),
                new PropertyMetadata(new CornerRadius(0)));

        public static readonly DependencyProperty IsActiveProperty =
            DependencyProperty.Register(
                "IsActive", 
                typeof(bool), 
                typeof(PromptBubbleButton), 
                new PropertyMetadata(false));


    }
}
