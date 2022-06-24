using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Y.ASIS.App.Common;
using Y.ASIS.App.Models;
using Y.ASIS.App.ViewModels;
using Y.ASIS.Common.Models.Enums;

namespace Y.ASIS.App.Windows
{
    /// <summary>
    /// AuthorityManagerWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AuthorityManagerWindow : PopupWindow
    {
        private readonly AuthorityManagerViewModel vm;

        public AuthorityManagerWindow(IEnumerable<Track> tracks)
        {
            vm = new AuthorityManagerViewModel(tracks);
            InitializeComponent();
            if (AppGlobal.Instance.Project == ProjectType.NationalRailway_BaiSe)
            {
                chkInspect.Visibility = Visibility.Collapsed;
            }

            PositionComboBox.Visibility = Visibility.Hidden;
            DataContext = vm;
        }

        private void TrackComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TrackComboBox.SelectedValue is Track track)
            {
                PositionComboBox.ItemsSource = track.Positions;
                PositionComboBox.SelectedIndex = 0;
                PositionComboBox.Visibility = track.Positions.Count >= 1 ? Visibility.Visible : Visibility.Hidden;
            }
            else
            {
                PositionComboBox.Visibility = Visibility.Hidden;
            }
        }
    }
}
