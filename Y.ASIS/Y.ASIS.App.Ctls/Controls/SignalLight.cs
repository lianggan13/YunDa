using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Effects;
using System.Windows.Shapes;

namespace Y.ASIS.App.Ctls.Controls
{
    /// <summary>
    /// 信号种类
    /// </summary>
    public enum SignalLightState
    {
        Unknown,
        Red,
        White
    }

    /// <summary>
    /// 描述一个信号灯控件
    /// </summary>
    public class SignalLight : Control
    {
        private Ellipse top;
        private Ellipse bottom;

        static SignalLight()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SignalLight),
                new FrameworkPropertyMetadata(typeof(SignalLight)));
        }

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(SignalLight), new PropertyMetadata(default));

        public static readonly DependencyProperty StateProperty =
            DependencyProperty.Register("State", typeof(SignalLightState), typeof(SignalLight), new PropertyMetadata(SignalLightState.Unknown));

        public static readonly DependencyProperty LightSizeProperty =
            DependencyProperty.Register("LightSize", typeof(double), typeof(SignalLight), new PropertyMetadata(0d, OnLightSizeChanged));

        public static readonly DependencyProperty EnableAllowProperty =
            DependencyProperty.Register("EnableAllow", typeof(bool), typeof(SignalLight), new PropertyMetadata(false));

        public static readonly DependencyProperty EnableForbidProperty =
            DependencyProperty.Register("EnableForbid", typeof(bool), typeof(SignalLight), new PropertyMetadata(false));

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(SignalLight), new PropertyMetadata(null));

        public static readonly DependencyProperty AllowCommandParameterProperty =
            DependencyProperty.Register("AllowCommandParameter", typeof(object), typeof(SignalLight), new PropertyMetadata(null));

        public static readonly DependencyProperty ForbidCommandParameterProperty =
            DependencyProperty.Register("ForbidCommandParameter", typeof(object), typeof(SignalLight), new PropertyMetadata(null));

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public object AllowCommandParameter
        {
            get { return (object)GetValue(AllowCommandParameterProperty); }
            set { SetValue(AllowCommandParameterProperty, value); }
        }

        public object ForbidCommandParameter
        {
            get { return (object)GetValue(ForbidCommandParameterProperty); }
            set { SetValue(ForbidCommandParameterProperty, value); }
        }

        public bool EnableAllow
        {
            get { return (bool)GetValue(EnableAllowProperty); }
            set { SetValue(EnableAllowProperty, value); }
        }

        public bool EnableForbid
        {
            get { return (bool)GetValue(EnableForbidProperty); }
            set { SetValue(EnableForbidProperty, value); }
        }

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        public SignalLightState State
        {
            get { return (SignalLightState)GetValue(StateProperty); }
            set { SetValue(StateProperty, value); }
        }

        public double LightSize
        {
            get { return (double)GetValue(LightSizeProperty); }
            set { SetValue(LightSizeProperty, value); }
        }

        private static void OnLightSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SignalLight light = d as SignalLight;
            double blurRadius = (double)e.NewValue / 3;
            light.AdjustBlurRadius(blurRadius);
        }

        private void AdjustBlurRadius(double blurRadius)
        {
            if (top != null && bottom != null)
            {
                (top.Effect as DropShadowEffect).BlurRadius = blurRadius;
                (bottom.Effect as DropShadowEffect).BlurRadius = blurRadius;
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            top = Template.FindName("Top", this) as Ellipse;
            bottom = Template.FindName("Bottom", this) as Ellipse;
            double blurRadius = LightSize / 3;
            AdjustBlurRadius(blurRadius);
        }
    }
}
