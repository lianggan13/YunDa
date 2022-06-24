using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Y.ASIS.App.Ctls.Controls
{
    public class StatusDisplayer : Control
    {
        static StatusDisplayer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(StatusDisplayer),
                new FrameworkPropertyMetadata(typeof(StatusDisplayer)));
        }

        public bool Connected
        {
            get { return (bool)GetValue(ConnectedProperty); }
            set { SetValue(ConnectedProperty, value); }
        }

        public static readonly DependencyProperty ConnectedProperty =
            DependencyProperty.Register("Connected", typeof(bool), typeof(StatusDisplayer), new PropertyMetadata(false));
    }
}
