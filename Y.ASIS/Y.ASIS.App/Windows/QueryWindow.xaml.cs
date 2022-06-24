using System.Collections.Generic;
using Y.ASIS.App.Common;
using Y.ASIS.App.Models;
using Y.ASIS.App.ViewModels;
using Y.ASIS.Common.Models.Enums;

namespace Y.ASIS.App.Windows
{
    /// <summary>
    /// QueryWindow.xaml 的交互逻辑
    /// </summary>
    public partial class QueryWindow : PopupWindow
    {
        private readonly QueryViewModel vm;
        public QueryWindow(IEnumerable<Track> tracks)
        {
            vm = new QueryViewModel(tracks);
            InitializeComponent();
            DataContext = vm;

            switch (AppGlobal.Instance.Project)
            {
                case ProjectType.NationalRailway:
                    tabTool.Visibility = System.Windows.Visibility.Visible;
                    break;
                case ProjectType.NationalRailway_BaiSe:
                case ProjectType.CityRailway_1:
                case ProjectType.CityRailway_2:
                case ProjectType.Shenzhen12:
                    tabTool.Visibility = System.Windows.Visibility.Collapsed;
                    break;
                default:
                    break;
            }
        }
    }
}
