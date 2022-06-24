using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Y.ASIS.App.Models;
using Y.ASIS.App.Utils;
using Y.ASIS.Common.ExtensionMethod;

namespace Y.ASIS.App.UserControls
{
    /// <summary>
    /// PositionDetailsControl.xaml 的交互逻辑
    /// </summary>
    public partial class PositionDetailsControl : UserControl
    {
        public PositionDetailsControl()
        {
            InitializeComponent();
        }

        public event RoutedEventHandler CurrentPositionChanged
        {
            add { AddHandler(CurrentPositionChangedEvent, value); }
            remove { RemoveHandler(CurrentPositionChangedEvent, value); }
        }


        public Role CurrentRole
        {
            get { return (Role)GetValue(CurrentRoleProperty); }
            set { SetValue(CurrentRoleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentRole.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentRoleProperty =
            DependencyProperty.Register("CurrentRole", typeof(Role), typeof(PositionDetailsControl), new PropertyMetadata(null, RoleChanged));

        private static void RoleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        public User CurrentUser
        {
            get { return (User)GetValue(CurrentUserProperty); }
            set { SetValue(CurrentUserProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentUser.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentUserProperty =
            DependencyProperty.Register("CurrentUser", typeof(User), typeof(PositionDetailsControl), new PropertyMetadata(null, UserChanged));

        private static void UserChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        public static readonly RoutedEvent CurrentPositionChangedEvent =
            EventManager.RegisterRoutedEvent(nameof(CurrentPositionChanged), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(PositionDetailsControl));

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
            Track track = DataContext as Track;
            track.Positions.ForEach(i =>
            {
                if (i != null)
                {
                    i.IsSelected = false;
                }
            });
            position.IsSelected = true;
            CurrentPositionChangedRoutedEventArgs args = new CurrentPositionChangedRoutedEventArgs()
            {
                RoutedEvent = CurrentPositionChangedEvent,
                Position = position
            };
            RaiseEvent(args);
        }
    }

    class CurrentPositionChangedRoutedEventArgs : RoutedEventArgs
    {
        public Position Position { get; set; }
    }
}
