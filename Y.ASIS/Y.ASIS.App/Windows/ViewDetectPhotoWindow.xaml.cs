using System.Windows.Media;

namespace Y.ASIS.App.Windows
{
    /// <summary>
    /// ViewDetectPhotoWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ViewDetectPhotoWindow : PopupWindow
    {
        public ImageSource Source
        {
            set => PhotoImage.Source = value;
        }

        public ViewDetectPhotoWindow(string title, ImageSource source)
        {
            InitializeComponent();
            Title = title;
            Source = source;
        }

        private void OnMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            WindowState = WindowState == System.Windows.WindowState.Maximized ? System.Windows.WindowState.Normal : System.Windows.WindowState.Maximized;
        }
    }
}
