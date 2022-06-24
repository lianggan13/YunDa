using System.Windows;
using System.Windows.Controls;

namespace Y.ASIS.App.Windows
{
    /// <summary>
    /// PopupWindow.xaml 的交互逻辑
    /// </summary>
    [TemplatePart(Name = PART_CloseButton, Type = typeof(PopupWindow))]
    public class PopupWindow : Window
    {
        private const string PART_CloseButton = "PART_CloseButton";

        static PopupWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PopupWindow),
                new FrameworkPropertyMetadata(typeof(PopupWindow)));
        }

        //public PopupWindow()
        //{
        //    InitializeComponent();
        //}

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            (Template.FindName(PART_CloseButton, this) as Button).Click += (s, args) => Close();
        }
    }
}
