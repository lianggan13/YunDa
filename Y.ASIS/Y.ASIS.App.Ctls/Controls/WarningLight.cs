using System.Windows;
using System.Windows.Controls;

namespace Y.ASIS.App.Ctls.Controls
{
    /// <summary>
    /// 报警灯状态
    /// </summary>
    public enum WarningLightState
    {
        Unknown,
        Warning,
        NoWarning
    }

    /// <summary>
    /// 描述一个报警灯控件
    /// </summary>
    public class WarningLight : Control
    {
        static WarningLight()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WarningLight),
                new FrameworkPropertyMetadata(typeof(WarningLight)));
        }

        public double LightSize
        {
            get { return (double)GetValue(LightSizeProperty); }
            set { SetValue(LightSizeProperty, value); }
        }

        public WarningLightState State
        {
            get { return (WarningLightState)GetValue(StateProperty); }
            set { SetValue(StateProperty, value); }
        }

        public static readonly DependencyProperty LightSizeProperty =
            DependencyProperty.Register("LightSize", typeof(double), typeof(WarningLight), new PropertyMetadata(0d));

        public static readonly DependencyProperty StateProperty =
            DependencyProperty.Register("State", typeof(WarningLightState), typeof(WarningLight), new PropertyMetadata(WarningLightState.Unknown));
    }
}
