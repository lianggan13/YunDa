using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Y.ASIS.App.Models;
using Y.ASIS.App.Utils;
using Y.ASIS.App.ViewModels;

namespace Y.ASIS.App.UserControls
{
    /// <summary>
    /// PositionControl.xaml 的交互逻辑
    /// </summary>
    public partial class PositionControl : UserControl
    {
        private DateTime lastMouseLeftButtonTime = DateTime.MinValue;
        private Point lastMouseLeftButtonPoint;

        public PositionControl()
        {
            InitializeComponent();
        }

        public event RoutedEventHandler CurrentTrackChanged
        {
            add { AddHandler(CurrentTrackChangedEvent, value); }
            remove { RemoveHandler(CurrentTrackChangedEvent, value); }
        }

        public static readonly RoutedEvent CurrentTrackChangedEvent =
            EventManager.RegisterRoutedEvent(nameof(CurrentTrackChanged), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(PositionControl));

        private void TrainNoPreviewKeyDown(object sender, KeyEventArgs e)
        {
            Button button = VisualTreeUtil.GetChild<Button>(sender as DependencyObject).FirstOrDefault();
            TextBox textbox = VisualTreeUtil.GetChild<TextBox>(sender as DependencyObject).FirstOrDefault();
            if (textbox.IsFocused && e.Key == Key.Enter)
            {
                e.Handled = true;
                button.Focus();
            }
        }

        private void PositionMouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            Border border = sender as Border;
            Position position = border.DataContext as Position;
            Point point = e.GetPosition(this);
            MainViewModel vm = Application.Current.MainWindow.DataContext as MainViewModel;
            
            if (Math.Abs(point.X - lastMouseLeftButtonPoint.X) < 4
                && Math.Abs(point.Y - lastMouseLeftButtonPoint.Y) < 4
                && DateTime.Now - lastMouseLeftButtonTime < TimeSpan.FromMilliseconds(240))
            {
                vm.ResetPositionSelectedState();
                position.IsSelected = true;
                CurrentTrackChangedRoutedEventArgs args = new CurrentTrackChangedRoutedEventArgs()
                {
                    RoutedEvent = CurrentTrackChangedEvent,
                    Track = DataContext as Track
                };
                RaiseEvent(args);
            }
            else
            {
                bool flag = position.IsSelected;
                vm.ResetPositionSelectedState();
                position.IsSelected = !flag;
            }
            lastMouseLeftButtonTime = DateTime.Now;
            lastMouseLeftButtonPoint = point;
        }
    }

    class CurrentTrackChangedRoutedEventArgs : RoutedEventArgs
    {
        public Track Track { get; set; }
    }
}
