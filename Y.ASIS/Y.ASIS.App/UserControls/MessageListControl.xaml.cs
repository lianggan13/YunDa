using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Y.ASIS.App.Models;
using Y.ASIS.App.Utils;

namespace Y.ASIS.App.UserControls
{
    /// <summary>
    /// MessageListControl.xaml 的交互逻辑
    /// </summary>
    public partial class MessageListControl : UserControl
    {
        public MessageListControl()
        {
            InitializeComponent();
        }

        public double MessageMaxWidth
        {
            get { return (double)GetValue(MessageMaxWidthProperty); }
            set { SetValue(MessageMaxWidthProperty, value); }
        }

        public static readonly DependencyProperty MessageMaxWidthProperty =
            DependencyProperty.Register("MessageMaxWidth", typeof(double), typeof(MessageListControl), new PropertyMetadata(double.MaxValue));

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(MessageListControl), new PropertyMetadata(default, OnItemsSourceChanged));

        private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MessageListControl control = d as MessageListControl;
            if (e.NewValue != null && e.NewValue is ObservableCollection<Message> source)
            {
                source.CollectionChanged += (s, args) =>
                {
                    IEnumerable<ScrollViewer> viewers = VisualTreeUtil.GetChild<ScrollViewer>(control.ItemsControlBlock);
                    if (viewers.Any())
                    {
                        viewers.First().ScrollToBottom();
                    }
                };
            }
        }
    }
}
