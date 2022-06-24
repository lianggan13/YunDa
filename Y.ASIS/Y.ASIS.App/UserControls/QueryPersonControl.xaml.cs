using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using Y.ASIS.App.Communication.Query;
using Y.ASIS.App.Models;
using Y.ASIS.App.Windows;
using Y.ASIS.Common.ExtensionMethod;
using Y.ASIS.Common.Models;
using Y.ASIS.Models.Enums;

namespace Y.ASIS.App.UserControls
{
    /// <summary>
    /// QueryPersonControl.xaml 的交互逻辑
    /// </summary>
    public partial class QueryPersonControl : UserControl, INotifyPropertyChanged
    {
        private const int PageCount = 12;

        public QueryPersonControl()
        {
            InitializeComponent();
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
        private int? positionId;
        private int? userNo;
        private IssueType issueType;
        private bool revoked;
        private DateTime startTime;
        private DateTime endTime;

        private void QueryButtonClick(object sender, System.Windows.RoutedEventArgs e)
        {
            bool flag = int.TryParse(UserNoTextBlock.Text, out int userNo);
            if (!UserNoTextBlock.Text.IsNullOrEmptyOrWhiteSpace()
                && !flag)
            {
                MessageWindow.Show("人员工号必须是数字");
                return;
            }

            issueType = (IssueType)IssueTypeComboBox.SelectedIndex;
            revoked = Convert.ToBoolean(RevokedComboBox.SelectedIndex);
            startTime = StartDate.Date;
            endTime = EndDate.Date;

            trackId = TrackComboBox.SelectedItem != null ? (int?)TrackComboBox.SelectedValue : null;
            positionId = PositionComboBox.SelectedItem != null ? (int?)PositionComboBox.SelectedValue : null;
            this.userNo = userNo != 0 ? (int?)userNo : null;
            Query(1);
        }

        private void FirstPageButtonClick(object sender, System.Windows.RoutedEventArgs e)
        {
            Query(1);
        }

        private void PreviousPageButtonClick(object sender, System.Windows.RoutedEventArgs e)
        {
            if (Index > 1)
            {
                Query(Index - 1);
            }
        }

        private void NextPageButtonClick(object sender, System.Windows.RoutedEventArgs e)
        {
            int index = Index + 1;
            if (index <= (int)Math.Ceiling(Total * 1.0 / PageCount))
            {
                Query(index);
            }
        }

        private void LastPageButtonClick(object sender, System.Windows.RoutedEventArgs e)
        {
            Query((int)Math.Ceiling(Total * 1.0 / PageCount));
        }

        private void Query(int queryindex)
        {
            UserAuthQueryRequest request = new UserAuthQueryRequest()
            {
                TrackId = trackId,
                PositionId = positionId,
                UserNo = userNo,
                IssueType = issueType,
                Revoked = revoked,
                StartTime = startTime,
                EndTime = endTime,
                Index = queryindex,
                Count = PageCount
            };

            request.RequestAsync<ResponseData<PageData<UserAuthRecord>>>(resp =>
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
    }
}
