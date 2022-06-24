using Emgu.CV;
using Emgu.CV.CvEnum;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Y.ASIS.App.Common;
using Y.ASIS.App.Ctls.Controls;

namespace Y.ASIS.App.Windows
{
    /// <summary>
    /// TakeUserPhotoWindow.xaml 的交互逻辑
    /// </summary>
    public partial class TakeUserPhotoWindow : PopupWindow
    {
        private VideoCapture capture;

        public string PhotoString { get; set; }

        public TakeUserPhotoWindow()
        {
            InitializeComponent();
        }


        private CancellationTokenSource cts = new CancellationTokenSource();

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CvInvoke.UseOpenCL = CvInvoke.HaveOpenCLCompatibleGpuDevice;
            string fronttalfacefile = Path.Combine(AppGlobal.Instance.ExecuteDirectory, "Config", "haarcascade_frontalface_alt.xml");
            CascadeClassifier face = new CascadeClassifier(fronttalfacefile);

            double width = GridBlock.Width;
            double height = GridBlock.Height;

            await LoadingWindow.Show(this, () =>
            {
                capture = new VideoCapture();
            });

            await Task.Run(() =>
            {
                int i = 0;
                while (true)
                {
                    //await Task.Delay(100);
                    if (cts.IsCancellationRequested)
                    {
                        capture.Stop();
                        capture?.Dispose();
                        capture = null;
                        break;
                    }

                    Mat mat = capture?.QueryFrame();

                    if (mat == null)
                    {
                        Dispatcher.Invoke(() =>
                        {
                            MessageWindow.Show("系统未能检测到摄像头，请检查摄像头");
                        });
                        return;
                    }
                    if (mat.IsEmpty)
                    {
                        Dispatcher.Invoke(() =>
                        {
                            MessageWindow.Show("系统未能检测到摄像头，请检查摄像头");
                        });
                        mat.Dispose();
                        return;
                    }
                    CvInvoke.Resize(mat, mat, new System.Drawing.Size((int)width, (int)height));
                    CvInvoke.Flip(mat, mat, FlipType.Horizontal);
                    using (UMat gray = new UMat())
                    {
                        CvInvoke.CvtColor(mat, gray, ColorConversion.Bgr2Gray);

                        System.Drawing.Rectangle[] faces = face.DetectMultiScale(gray, 1.1, 2, new System.Drawing.Size(150, 150));
                        System.Drawing.Rectangle first = faces.FirstOrDefault();
                        bool verified = false;
                        if (first != null
                            && first.X > 90
                            && first.Y > 30
                            && first.X + first.Width < 390
                            && first.Y + first.Height < 300)
                        {
                            i++;
                            verified = true;
                        }
                        else
                        {
                            i = 0;
                        }

                        Dispatcher.Invoke(async () =>
                        {
                            VisualBorder.Background = verified ? new SolidColorBrush(Colors.SpringGreen) : new SolidColorBrush(Colors.White);

                            try
                            {
                                CameraBlock.Source = BitmapToBitmapImage(mat.Bitmap);
                            }
                            catch (Exception ex)
                            {
                                MessageWindow.Show($"{ex.Message}");
                            }

                            if (verified && i == 10)
                            {
                                Mat roi = new Mat(mat, new System.Drawing.Rectangle(100, 0, 280, 360));
                                PhotoString = BitmapToBase64String(roi.Bitmap);
                                if (Owner != null)
                                {
                                    //cts.Cancel();
                                    //capture.Dispose();
                                    DialogResult = true;
                                    Close();
                                    await Task.Delay(100);
                                }
                            }
                        });
                    }
                }

            }, cts.Token);

        }

        private BitmapImage BitmapToBitmapImage(System.Drawing.Bitmap bitmap)
        {
            System.Drawing.Bitmap ImageOriginalBase = new System.Drawing.Bitmap(bitmap);
            BitmapImage bitmapImage = new BitmapImage();
            using (MemoryStream ms = new MemoryStream())
            {
                ImageOriginalBase.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = ms;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();
            }
            return bitmapImage;
        }

        private string BitmapToBase64String(Bitmap bmp)
        {
            try
            {
                MemoryStream ms = new MemoryStream();
                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] arr = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(arr, 0, (int)ms.Length);
                ms.Close();
                return Convert.ToBase64String(arr);
            }
            catch
            {
                return null;
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            cts.Cancel();
            base.OnClosing(e);
        }
    }
}
