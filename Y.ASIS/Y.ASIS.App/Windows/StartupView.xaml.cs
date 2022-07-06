using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;
using Y.ASIS.App.Communication;
using Y.ASIS.App.Database;
using Y.ASIS.App.Models;
using Y.ASIS.App.Services.CameraService;
//using Y.ASIS.App.Utils;

namespace Y.ASIS.App.Windows
{
    /// <summary>
    /// StartupView.xaml 的交互逻辑
    /// </summary>
    public partial class StartupView : Window
    {
        public StartupView()
        {
            InitializeComponent();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            bool pdct = false;
            await Task.Run(() =>
            {
                if (!HIKNVRService.CheckRenderEnv())
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        MessageWindow.Show($"检测渲染环境是否存在，结果:[False]\r\n" +
                      $"需关闭程序,联系管理员确认渲染环境");
                        Close();
                    });
                }

                //设置全局动画帧
                Timeline.DesiredFrameRateProperty.OverrideMetadata(
                    typeof(Timeline),
                    new FrameworkPropertyMetadata { DefaultValue = 60 });

                try
                {
                    LoadTitles();
                }
                catch (Exception ex)
                {
                    Application.Current.Resources["TitleText"] = "检修作业安全联锁系统";
                }

                _ = Utils.CardUtil.GetCardUid();

                bool nvr = HIKNVRService.Login();
                bool srp = HeartRequest.Ping();

                pdct = srp; // nvr && svr;
            });

            AppGlobal.Env = pdct ? AppEnvironment.Production : AppEnvironment.Development;
            new MainWindow().Show();
            Close();
        }


        private void LoadTitles()
        {
            List<Title> titles = AppDatabase.Instance.GetTitles();
            foreach (Title title in titles)
            {
                Application.Current.Resources[title.ResourceKey] = title.Content;
            }
        }
    }
}
