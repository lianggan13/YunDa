using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Y.ASIS.App.Utils;
using Y.ASIS.Models.Enums;

namespace Y.ASIS.App.UserControls
{
    /// <summary>
    /// TrainDetailsControl.xaml 的交互逻辑
    /// </summary>
    public partial class TrainDetailsControl : UserControl
    {
        public TrainDetailsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 获取或设置车号
        /// </summary>
        public string No
        {
            get { return (string)GetValue(NoProperty); }
            set { SetValue(NoProperty, value); }
        }

        /// <summary>
        /// 获取或设置左受电弓状态
        /// </summary>
        public PantographState LeftPantograph
        {
            get { return (PantographState)GetValue(LeftPantographProperty); }
            set { SetValue(LeftPantographProperty, value); }
        }

        /// <summary>
        /// 获取或设置右受电弓状态
        /// </summary>
        public PantographState RightPantograph
        {
            get { return (PantographState)GetValue(RightPantographProperty); }
            set { SetValue(RightPantographProperty, value); }
        }

        public static readonly DependencyProperty NoProperty =
            DependencyProperty.Register("No", typeof(string), typeof(TrainDetailsControl), new PropertyMetadata(default));

        public static readonly DependencyProperty LeftPantographProperty =
            DependencyProperty.Register("LeftPantograph", typeof(PantographState), typeof(TrainDetailsControl), new PropertyMetadata(PantographState.Down));

        public static readonly DependencyProperty RightPantographProperty =
            DependencyProperty.Register("RightPantograph", typeof(PantographState), typeof(TrainDetailsControl), new PropertyMetadata(PantographState.Down));

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
    }
}
