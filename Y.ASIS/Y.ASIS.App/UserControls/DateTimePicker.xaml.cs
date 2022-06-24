using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace Y.ASIS.App.UserControls
{
    /// <summary>
    /// DateTimePicker.xaml 的交互逻辑
    /// </summary>
    public partial class DateTimePicker : UserControl
    {
        public DateTimePicker()
        {
            InitializeComponent();
        }

        public DateTime DateTime
        {
            get { return (DateTime)GetValue(DateTimeProperty); }
            set { SetValue(DateTimeProperty, value); }
        }

        public static readonly DependencyProperty DateTimeProperty =
            DependencyProperty.Register("DateTime", typeof(DateTime), typeof(DateTimePicker), new PropertyMetadata(DateTime.Now, OnDateTimeChanged));

        private static void OnDateTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DateTimePicker picker = d as DateTimePicker;
            picker.SetTime(picker.DateTime);
        }

        public DateTime? StartTime
        {
            get { return (DateTime?)GetValue(StartTimeProperty); }
            set { SetValue(StartTimeProperty, value); }
        }

        public static readonly DependencyProperty StartTimeProperty =
            DependencyProperty.Register("StartTime", typeof(DateTime?), typeof(DateTimePicker), new PropertyMetadata(DateTime.MinValue));



        public DateTime? EndTime
        {
            get { return (DateTime?)GetValue(EndTimeProperty); }
            set { SetValue(EndTimeProperty, value); }
        }

        public static readonly DependencyProperty EndTimeProperty =
            DependencyProperty.Register("EndTime", typeof(DateTime?), typeof(DateTimePicker), new PropertyMetadata(DateTime.MaxValue));



        private void SetTime(DateTime time)
        {
            Calendar.SelectedDate = time;
            string[] array = time.ToString("HH:mm:ss").Split(':');
            Hour.SelectedItem = array[0];
            Minute.SelectedItem = array[1];
            Second.SelectedItem = array[2];
        }

        private void SelectDateTimeButtonClick(object sender, RoutedEventArgs e)
        {
            if (!PopupBlock.IsOpen)
            {
                PopupBlock.IsOpen = true;
            }
        }

        private void NowButtonClick(object sender, RoutedEventArgs e)
        {
            SetTime(DateTime.Now);
        }

        private void ComfirmButtonClick(object sender, RoutedEventArgs e)
        {
            DateTime time = DateTime.Parse($"{DateBlock.Text} {Hour.SelectedItem}:{Minute.SelectedItem}:{Second.SelectedItem}");
            Storyboard storyboard = FindResource("HintStoryboard") as Storyboard;
            if (StartTime != null && time <= StartTime)
            {
                HintBlock.Text = "需大于开始时间";
                BeginStoryboard(storyboard);
            }
            else if (EndTime != null && time >= EndTime)
            {
                HintBlock.Text = "需小于结束时间";
                BeginStoryboard(storyboard);
            }
            else
            {
                DateTime = time;
                PopupBlock.IsOpen = false;
            }
        }
    }
}
