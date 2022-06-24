using System.IO;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using Y.ASIS.App.Communication;
using Y.ASIS.App.Utils;
using Y.ASIS.Common.Models;

namespace Y.ASIS.App.Windows
{
    /// <summary>
    /// ViewUserPhotoWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ViewUserPhotoWindow : PopupWindow
    {
        private bool update;
        private string photoString;

        public string PhotoUrl
        {
            get { return (string)GetValue(PhotoUrlProperty); }
            set { SetValue(PhotoUrlProperty, value); }
        }

        public static readonly DependencyProperty PhotoUrlProperty =
            DependencyProperty.Register("PhotoUrl", typeof(string), typeof(ViewUserPhotoWindow), new PropertyMetadata(string.Empty));

        private readonly int userId;
        public ViewUserPhotoWindow(int userId, string photoUrl)
        {
            this.userId = userId;
            InitializeComponent();
            PhotoUrl = photoUrl;
        }

        private void TakeButtonClick(object sender, RoutedEventArgs e)
        {
            var window = new TakeUserPhotoWindow()
            {
                Owner = this
            };
            bool? dialogResult = window.ShowDialog();
            if ((bool)dialogResult)
            {
                photoString = window.PhotoString + "Internal";
                PhotoImage.Source = ImageUtil.Base64ToImage(window.PhotoString);
                update = true;
            }
        }

        private void UploadButtonClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog()
            {
                Filter = "JPEG File(*.jpg)|*.jpg",
            };
            DialogResult result = dialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                PhotoUrl = dialog.FileName;

                // 未使用ImageBrush的ImageSource直接绑定 会有缓存
                BitmapImage image = new BitmapImage();
                using (FileStream stream = new FileStream(PhotoUrl, FileMode.Open, FileAccess.Read))
                {
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.StreamSource = stream;
                    image.EndInit();
                }
                PhotoImage.Source = image;
                photoString = ImageUtil.BitmapImageToBase64String(image);
                update = true;
            }
        }

        private void ComfirmButtonClick(object sender, RoutedEventArgs e)
        {
            if (update)
            {
                UpdateUserPhotoRequest request = new UpdateUserPhotoRequest(userId, photoString);
                ResponseData<object> resp = request.Request<ResponseData<object>>();
                if (resp != null && !resp.IsSuccess)
                {
                    MessageWindow.Show("照片上传失败: " + resp.Message);
                }
            }
            if (Owner != null)
            {
                DialogResult = true;
            }
            Close();
        }
    }
}
