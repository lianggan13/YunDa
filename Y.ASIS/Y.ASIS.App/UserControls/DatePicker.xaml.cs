using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace Y.ASIS.App.UserControls
{
    /// <summary>
    /// DatePicker.xaml 的交互逻辑
    /// </summary>
    public partial class DatePicker : UserControl
    {
        public DatePicker()
        {
            InitializeComponent();
        }

        public DateTime Date
        {
            get { return (DateTime)GetValue(DateProperty); }
            set { SetValue(DateProperty, value); }
        }

        public DateTime? StartDate
        {
            get { return (DateTime?)GetValue(StartDateProperty); }
            set { SetValue(StartDateProperty, value); }
        }

        public DateTime? EndDate
        {
            get { return (DateTime?)GetValue(EndDateProperty); }
            set { SetValue(EndDateProperty, value); }
        }

        public static readonly DependencyProperty DateProperty =
            DependencyProperty.Register("Date", typeof(DateTime), typeof(DatePicker), new PropertyMetadata(DateTime.Now, OnDateChanged));

        private static void OnDateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DatePicker picker = d as DatePicker;
            picker.SetDate(picker.Date);
        }

        public static readonly DependencyProperty StartDateProperty =
            DependencyProperty.Register("StartDate", typeof(DateTime?), typeof(DatePicker), new PropertyMetadata(DateTime.MinValue));

        public static readonly DependencyProperty EndDateProperty =
            DependencyProperty.Register("EndDate", typeof(DateTime?), typeof(DatePicker), new PropertyMetadata(DateTime.MaxValue));

        private void SetDate(DateTime date)
        {
            Calendar.SelectedDate = date;
            Calendar.DisplayDate = date;
        }

        private void SelectDateTimeButtonClick(object sender, RoutedEventArgs e)
        {
            if (!PopupBlock.IsOpen)
            {
                PopupBlock.IsOpen = true;
            }
        }

        private void TodayButtonClick(object sender, RoutedEventArgs e)
        {
            Calendar.SelectedDate = DateTime.Now;
        }

        private void ComfirmButtonClick(object sender, RoutedEventArgs e)
        {
            DateTime? date = Calendar.SelectedDate;
            Storyboard storyboard = FindResource("HintStoryboard") as Storyboard;

            if (date == null)
            {
                HintBlock.Text = "需正确选择时间";
                BeginStoryboard(storyboard);
                return;
            }

            //if (StartDate != null && date <= StartDate)
            if (StartDate != null && date < StartDate)
            {
                HintBlock.Text = "需大于开始时间";
                BeginStoryboard(storyboard);
            }
            else if (StartDate != null && date >= EndDate)
            {
                HintBlock.Text = "需小于结束时间";
                BeginStoryboard(storyboard);
            }
            else
            {
                Date = (DateTime)date;
                PopupBlock.IsOpen = false;
            }
        }

        private void Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime? date = Calendar.SelectedDate;
            DateTime? date2 = Calendar.DisplayDate;

        }

        private void Calendar_PreviewMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // release mouse capture (don't need focuse)
            // way1
            if (Mouse.Captured is CalendarItem)
            {
                Mouse.Capture(null);
            }
            return;
            // way2
            if (sender is UIElement ui)
            {
                ui.ReleaseMouseCapture();
            }
        }
    }
}
