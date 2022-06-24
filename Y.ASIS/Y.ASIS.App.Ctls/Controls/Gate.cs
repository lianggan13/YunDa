using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Y.ASIS.App.Ctls.Controls
{
    /// <summary>
    /// 道闸机状态
    /// </summary>
    public enum GateState
    {
        Close,
        Open,
        Offline,
    }

    /// <summary>
    /// 道闸机方向
    /// </summary>
    public enum GateDirection
    {
        Left,
        Right
    }

    /// <summary>
    /// 道闸机控件
    /// </summary>
    public class Gate : Control
    {
        static Gate()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Gate),
                new FrameworkPropertyMetadata(typeof(Gate)));
        }

        public GateState State
        {
            get { return (GateState)GetValue(StateProperty); }
            set { SetValue(StateProperty, value); }
        }

        public GateDirection Direction
        {
            get { return (GateDirection)GetValue(DirectionProperty); }
            set { SetValue(DirectionProperty, value); }
        }

        public bool EnableUp
        {
            get { return (bool)GetValue(EnableUpProperty); }
            set { SetValue(EnableUpProperty, value); }
        }

        public bool EnableDown
        {
            get { return (bool)GetValue(EnableDownProperty); }
            set { SetValue(EnableDownProperty, value); }
        }

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public object UpCommandParameter
        {
            get { return (object)GetValue(UpCommandParameterProperty); }
            set { SetValue(UpCommandParameterProperty, value); }
        }

        public object DownCommandParameter
        {
            get { return (object)GetValue(DownCommandParameterProperty); }
            set { SetValue(DownCommandParameterProperty, value); }
        }

        public static readonly DependencyProperty StateProperty =
            DependencyProperty.Register("State", typeof(GateState), typeof(Gate), new PropertyMetadata(GateState.Close));

        public static readonly DependencyProperty DirectionProperty =
            DependencyProperty.Register("Direction", typeof(GateDirection), typeof(Gate), new PropertyMetadata(GateDirection.Left));

        public static readonly DependencyProperty EnableUpProperty =
            DependencyProperty.Register("EnableUp", typeof(bool), typeof(Gate), new PropertyMetadata(false));

        public static readonly DependencyProperty EnableDownProperty =
            DependencyProperty.Register("EnableDown", typeof(bool), typeof(Gate), new PropertyMetadata(false));

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(Gate), new PropertyMetadata(null));

        public static readonly DependencyProperty UpCommandParameterProperty =
            DependencyProperty.Register("UpCommandParameter", typeof(object), typeof(Gate), new PropertyMetadata(null));

        public static readonly DependencyProperty DownCommandParameterProperty =
            DependencyProperty.Register("DownCommandParameter", typeof(object), typeof(Gate), new PropertyMetadata(null));
    }
}
