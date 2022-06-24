using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using Y.ASIS.App.Communication.Query;
using Y.ASIS.App.Models;
using Y.ASIS.Common.ExtensionMethod;
using Y.ASIS.Common.Models;

namespace Y.ASIS.App.UserControls
{
    /// <summary>
    /// QueryTrainNumberControl.xaml 的交互逻辑
    /// </summary>
    public partial class QueryTrainNumberControl : UserControl, INotifyPropertyChanged
    {
        private const int PageCount = 12;

        public QueryTrainNumberControl()
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

        private int? trackId;
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

        private void QueryButtonClick(object sender, RoutedEventArgs e)
        {
            startTime = StartDate.Date;
            endTime = EndDate.Date;
            trackId = TrackComboBox.SelectedItem != null ? (int?)TrackComboBox.SelectedValue : null;
            Query(1);
        }

        private void Query(int index)
        {
            TrainNumberQueryRequest request = new TrainNumberQueryRequest()
            {
                TrackId = trackId,
                StartTime = startTime,
                EndTime = endTime,
                Index = index,
                Count = PageCount
            };

            request.RequestAsync<ResponseData<PageData<TrainNumberRecord>>>(resp =>
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
