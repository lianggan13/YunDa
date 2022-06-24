using System.Windows;
using System.Windows.Threading;

namespace Y.ASIS.App.Windows
{
    /// <summary>
    /// MessageWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MessageWindow : PopupWindow
    {
        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register("Message", typeof(string), typeof(MessageWindow), new PropertyMetadata(string.Empty));

        public MessageBoxResult Result { get; private set; }

        private MessageWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        public static MessageBoxResult Show(string message, string title, MessageBoxButton messageBoxButton)
        {
            MessageBoxResult result = default;
            Dispatcher.CurrentDispatcher.Invoke(() =>
            {
                MessageWindow window = new MessageWindow()
                {
                    Message = message,
                    Title = title
                };
                if (messageBoxButton == MessageBoxButton.OK)
                {
                    window.CancelButton.Visibility = Visibility.Collapsed;
                }
                window.ShowDialog();
                result = window.Result;
            });
            return result;
        }

        public static MessageBoxResult Show(string message, string title)
        {
            return Show(message, title, MessageBoxButton.OK);
        }

        public static MessageBoxResult Show(string message)
        {
            string title = "提示";
            return Show(message, title, MessageBoxButton.OK);
        }

        private void ComfirmButtonClick(object sender, RoutedEventArgs e)
        {
            Result = MessageBoxResult.OK;
            Close();
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            Result = MessageBoxResult.Cancel;
            Close();
        }
    }
}
