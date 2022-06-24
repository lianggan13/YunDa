using System.Windows.Input;

namespace Y.ASIS.App.Windows
{
    /// <summary>
    /// ViewVideoWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ViewVideoWindow : PopupWindow
    {
        public ViewVideoWindow()
        {
            InitializeComponent();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Close();
        }
    }
}
