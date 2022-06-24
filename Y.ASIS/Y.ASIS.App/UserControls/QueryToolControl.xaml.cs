using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using Y.ASIS.App.Communication.Query;
using Y.ASIS.App.Models;
using Y.ASIS.App.Windows;
using Y.ASIS.Common.ExtensionMethod;
using Y.ASIS.Common.Models;

namespace Y.ASIS.App.UserControls
{
    /// <summary>
    /// QueryToolControl.xaml 的交互逻辑
    /// </summary>
    public partial class QueryToolControl : UserControl, INotifyPropertyChanged
    {
        private const int PageCount = 12;

        public QueryToolControl()
        {
            InitializeComponent();

            TrackComboBox.SelectionChanged += OnTrackComboBoxSelectionChanged;
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

        private int? trackId;
        private int? toolId;
        private int? userNo;
        private bool revoked;
        private DateTime startTime;
        private DateTime endTime;

        private void OnTrackComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TrackComboBox.SelectedValue == null)
            {
                return;
            }
            trackId = (int)TrackComboBox.SelectedValue;
            ToolListRequest request = new ToolListRequest((int)trackId);
            request.RequestAsync<ResponseData<IEnumerable<KeyOrTool>>>(resp =>
            {
                if (resp != null && resp.IsSuccess)
                {
                    Dispatcher.Invoke(() =>
                    {
                        ToolComboBox.ItemsSource = resp.Data;
                    });
                }
            });
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
            revoked = Convert.ToBoolean(RevokedComboBox.SelectedIndex);
            startTime = StartDate.Date;
            endTime = EndDate.Date;
            trackId = TrackComboBox.SelectedItem != null ? (int?)TrackComboBox.SelectedValue : null;
            toolId = ToolComboBox.SelectedItem != null ? (int?)ToolComboBox.SelectedValue : null;
            this.userNo = userNo != 0 ? (int?)userNo : null;
            Query(1);
        }

        private void Query(int index)
        {
            ToolAuthQueryRequest request = new ToolAuthQueryRequest()
            {
                TrackId = trackId,
                ToolId = toolId,
                UserNo = userNo,
                Revoked = revoked,
                StartTime = startTime,
                EndTime = endTime,
                Index = index,
                Count = PageCount
            };

            request.RequestAsync<ResponseData<PageData<ToolAuthRecord>>>(resp =>
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
    }
}
