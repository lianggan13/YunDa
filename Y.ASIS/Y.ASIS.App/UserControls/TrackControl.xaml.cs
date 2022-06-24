using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using Y.ASIS.App.Ctls.Controls;
using Y.ASIS.Models.Enums;

namespace Y.ASIS.App.UserControls
{
    /// <summary>
    /// TrackControl.xaml 的交互逻辑
    /// </summary>
    public partial class TrackControl : UserControl
    {
        public TrackControl()
        {
            InitializeComponent();
        }

        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        public TrackType Type
        {
            get { return (TrackType)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }

        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register("IsSelected", typeof(bool), typeof(TrackControl), new PropertyMetadata(false));

        public static readonly DependencyProperty TypeProperty =
            DependencyProperty.Register("Type", typeof(TrackType), typeof(TrackControl), new PropertyMetadata(TrackType.TPT));
    }
}
