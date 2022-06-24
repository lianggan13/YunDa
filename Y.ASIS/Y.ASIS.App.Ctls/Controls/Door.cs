using System.Windows;
using System.Windows.Controls;

namespace Y.ASIS.App.Ctls.Controls
{
    /// <summary>
    /// 门禁状态
    /// </summary>
    public enum DoorState
    {
        Unknown,
        PowerOffOpen,
        PowerOffClose,
        PowerOnOpen,
        PowerOnClose,
    }

    /// <summary>
    /// 描述一个门禁控件
    /// </summary>
    public class Door : Control
    {
        static Door()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Door),
                new FrameworkPropertyMetadata(typeof(Door)));
        }

        public DoorState State
        {
            get { return (DoorState)GetValue(StateProperty); }
            set { SetValue(StateProperty, value); }
        }

        public static readonly DependencyProperty StateProperty =
            DependencyProperty.Register("State", typeof(DoorState), typeof(Door), new PropertyMetadata(DoorState.Unknown));
    }
}
