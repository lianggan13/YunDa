using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using Y.ASIS.App.Communication.Query;
using Y.ASIS.App.Models;
using Y.ASIS.App.Utils;
using Y.ASIS.App.Windows;
using Y.ASIS.Common.ExtensionMethod;
using Y.ASIS.Common.Models;

namespace Y.ASIS.App.UserControls
{
    /// <summary>
    /// QueryFaultRecordControl.xaml 的交互逻辑
    /// </summary>
    public partial class QueryFaultRecordControl : UserControl, INotifyPropertyChanged
    {
        public QueryFaultRecordControl()
        {
            InitializeComponent();
        }

        private const int PageCount = 12;

        private int? trackId;
        private int? userNo;
        private bool handled;
        private int? faultCode;
        private DateTime startTime;
        private DateTime endTime;

        private int index = 1;
        public int Index
        {
            get { return index; }
            set { SetProperty(ref index, value); }
        }

        private long total;
        public long Total
        {
            get { return total; }
            set { SetProperty(ref total, value); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 设置需通知的属性的值
        /// </summary>
        /// <typeparam name="T">属性的类型</typeparam>
        /// <param name="item">属性</param>
        /// <param name="value">值</param>
        /// <param name="propertyName">属性名称</param>
        protected virtual void SetProperty<T>(ref T item, T value, [CallerMemberName] string propertyName = null)
        {
            if (!EqualityComparer<T>.Default.Equals(item, value))
            {
                item = value;
                if (PropertyChanged == null) return;
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void QueryButtonClick(object sender, RoutedEventArgs e)
        {
            bool flag = int.TryParse(UserNoTextBlock.Text, out int userNo);
            if (!UserNoTextBlock.Text.IsNullOrEmptyOrWhiteSpace()
                && !flag)
            {
                MessageWindow.Show("人员工号必须是数字");
                return;
            }

            if (FaultTypeComboBox.SelectedItem != null)
            {
                faultCode = (int?)FaultTypeComboBox.SelectedValue;
            }
            trackId = TrackComboBox.SelectedItem != null ? (int?)TrackComboBox.SelectedValue : null;
            handled = Convert.ToBoolean(HandledComboBox.SelectedIndex);
            startTime = StartDate.Date;
            endTime = EndDate.Date;
            this.userNo = userNo != 0 ? (int?)userNo : null;
            Query(1);
        }

        private void Query(int index)
        {
            FaultQueryRequest request = new FaultQueryRequest()
            {
                TrackId = trackId,
                FaultCode = faultCode,
                StartTime = startTime,
                EndTime = endTime,
                Handled = handled,
                HandledBy = userNo,
                Index = index,
                Count = PageCount
            };
            request.RequestAsync<ResponseData<PageData<FaultMessages>>>(resp =>
            {
                if (resp != null && resp.Data != null)
                {
                    int i = resp.Data.Count * (resp.Data.Index - 1) + 1;
                    resp.Data.Items.ForEach(j => j.ViewIndex = i++);
                    Dispatcher.Invoke(() =>
                    {
                        ListViewBlock.ItemsSource = resp.Data.Items;
                        Total = resp.Data.Total;
                        Index = resp.Data.Index;
                    });
                }
            });
        }

        private void FirstPageButtonClick(object sender, RoutedEventArgs e)
        {
            Query(1);
        }

        private void PreviousPageButtonClick(object sender, RoutedEventArgs e)
        {
            if (Index > 1)
            {
                Query(Index - 1);
            }
        }

        private void NextPageButtonClick(object sender, RoutedEventArgs e)
        {
            int index = Index + 1;
            if (index <= (int)Math.Ceiling(Total * 1.0 / PageCount))
            {
                Query(index);
            }
        }

        private void LastPageButtonClick(object sender, RoutedEventArgs e)
        {
            Query((int)Math.Ceiling(Total * 1.0 / PageCount));
        }

        private void ListViewDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            IEnumerable<ListViewItem> items = VisualTreeUtil.GetChild<ListViewItem>(ListViewBlock);
            ListViewItem item = items.FirstOrDefault(i => i.IsSelected);
            if (item == null)
            {
                return;
            }
            Point relative = e.GetPosition(item);
            if (relative.X >= 0 && relative.X <= item.ActualWidth
                && relative.Y >= 0 && relative.Y <= item.ActualHeight)
            {
                FaultMessages clone = (item.DataContext as FaultMessages).JsonDeepCopy();

                ViewHandleWarningAndFaultWindow window = new ViewHandleWarningAndFaultWindow(ListViewBlock.ItemsSource as IEnumerable<FaultMessages>, clone)
                {
                    Owner = VisualTreeUtil.GetParent<Window>(this),
                };
                window.ShowDialog();
            }
        }
    }
}
