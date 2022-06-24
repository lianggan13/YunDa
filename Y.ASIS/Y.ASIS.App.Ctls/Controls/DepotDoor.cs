using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Y.ASIS.App.Ctls.Controls
{
    public enum DepotDoorState
    {
        Unknown,
        Close,
        Open
    }

    public class DepotDoor : Control
    {
        static DepotDoor()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DepotDoor),
                new FrameworkPropertyMetadata(typeof(DepotDoor)));
        }

        public DepotDoorState State
        {
            get { return (DepotDoorState)GetValue(StateProperty); }
            set { SetValue(StateProperty, value); }
        }

        public static readonly DependencyProperty StateProperty =
            DependencyProperty.Register("State", typeof(DepotDoorState), typeof(DepotDoor), new PropertyMetadata(DepotDoorState.Unknown));


    }
}
