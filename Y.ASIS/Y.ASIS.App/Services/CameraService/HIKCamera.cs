using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Y.ASIS.App.Common;
using Y.ASIS.App.Models;
using Y.ASIS.App.Utils;
using Y.ASIS.App.Windows;
using Y.HIKNVR.SDK;

namespace Y.ASIS.App.Services.CameraService
{
    public partial class HIKCamera : Camera
    {
        private Image image;
        public HIKCamera(VideoStream vs, Image image)
        {
            this.VideoStream = vs;
            this.image = image;
        }

        public override void Play()
        {
            try
            {
                HIKNVRClient.Preview(VideoStream.Channel, image);
            }
            catch (Exception ex)
            {
                MessageWindow.Show($"{ex.Message}");
            }
        }

        public override void Stop()
        {
            HIKNVRClient.StopPreview(image);
        }

        public override void FullScreenVideo(object sender, MouseButtonEventArgs e)
        {
            ViewVideoWindow window = new ViewVideoWindow()
            {
                Owner = Application.Current.MainWindow,
            };
            window.Loaded += (s, a) =>
            {
                window.VideoBlock.Source = image.Source;
            };

            void ImageSourceUpdated(object s, EventArgs args)
            {
                window.VideoBlock.Source = image.Source;
            };

            image.SourceUpdated += ImageSourceUpdated;

            window.Closing += (s, a) =>
            {
                image.SourceUpdated -= ImageSourceUpdated;
            };
            window.Show();
        }


        public override void SaveCapture(string name)
        {
            try
            {
                var buffer = HIKNVRClient.CaptureCache(VideoStream.Channel);

                string path = AppGlobal.CreatePhotoPath("视频联动",
                                            $"{VideoStream.Name}_{name}_{DateTime.Now:HHmmss)}.png");
                ImageUtil.SaveImage(buffer, path);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message, ex);
            }
        }
    }
}
