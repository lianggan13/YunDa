using System.Windows;
using System.Windows.Controls;

namespace Y.ASIS.App.Controls
{
    /// <summary>
    /// VideoPanel.xaml 的交互逻辑
    /// </summary>
    public partial class VideoPanel : Panel
    {
        public VideoPanel()
        {
            InitializeComponent();
        }


        protected override Size ArrangeOverride(Size arrangeSize)
        {
            double currentX = 0;
            double elementWidth = arrangeSize.Width / Children.Count;
            double elementHeight = arrangeSize.Height;

            for (int i = 0; i < Children.Count; i++)
            {
                (Children[i] as FrameworkElement).Margin = i != Children.Count - 1
                    ? new Thickness(0, 0, 5, 0)
                    : new Thickness(0);

                Children[i].Arrange(new Rect(currentX, 0, elementWidth, elementHeight));
                currentX += elementWidth;
            }
            return arrangeSize;
        }
    }
}
