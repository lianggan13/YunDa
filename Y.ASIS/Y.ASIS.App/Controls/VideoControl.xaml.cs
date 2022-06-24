using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using Y.ASIS.App.Models;
using Y.ASIS.App.Services.CameraService;


namespace Y.ASIS.App.Controls
{
    /// <summary>
    /// VideoControl.xaml 的交互逻辑
    /// </summary>
    [TemplatePart(Name = PART_Image, Type = typeof(VideoControl))]
    [TemplatePart(Name = PART_HwndRender, Type = typeof(HwndRender))]
    public partial class VideoControl : Control
    {
        private const string PART_Image = nameof(PART_Image);
        private const string PART_HwndRender = nameof(PART_HwndRender);

        public Image image;
        public HwndRender hwndrender;

        static VideoControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoControl),
                new FrameworkPropertyMetadata(typeof(VideoControl)));
        }

        public VideoControl()
        {
            InitializeComponent();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            image = Template.FindName(PART_Image, this) as Image;
            hwndrender = Template.FindName(PART_HwndRender, this) as HwndRender;
        }

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public bool? IsSafe
        {
            get { return (bool?)GetValue(IsSafeProperty); }
            set { SetValue(IsSafeProperty, value); }
        }


        public bool Play
        {
            get { return (bool)GetValue(PlayProperty); }
            set { SetValue(PlayProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty =
          DependencyProperty.Register(nameof(Title), typeof(string), typeof(VideoControl), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty IsSafeProperty =
            DependencyProperty.Register(nameof(IsSafe), typeof(bool?), typeof(VideoControl), new PropertyMetadata(null));

        public static readonly DependencyProperty PlayProperty =
            DependencyProperty.Register(nameof(Play), typeof(bool), typeof(VideoControl), new PropertyMetadata(false, OnPlayChanged));

        protected Camera camera = null;

        private void Control_Loaded(object sender, RoutedEventArgs e)
        {
            VideoStream vs = DataContext as VideoStream;
            if (vs.Model == "DaHua")
            {
                camera = new DaHuaCamera(vs, hwndrender);
                //hwndrender.Visibility = Visibility.Visible;
            }
            else if (vs.Model == "HIK")
            {
                camera = new HIKCamera(vs, image);
                hwndrender.Visibility = Visibility.Collapsed;
            }

            image.MouseLeftButtonDown -= camera.FullScreenVideo;
            image.MouseLeftButtonDown += camera.FullScreenVideo;
        }

        private void Control_Unloaded(object sender, RoutedEventArgs e)
        {

        }

        private static void OnPlayChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            VideoControl vctrl = d as VideoControl;
            if ((bool)e.NewValue)
            {
                vctrl.camera.Play();
                HIKNVRService.PlayingCameras.Add(vctrl.camera);
            }
            else
            {
                vctrl.camera.Stop();
                HIKNVRService.PlayingCameras.Remove(vctrl.camera);
            }
        }
    }



    // ref: https://blog.csdn.net/u013113678/article/details/121275982
    public class HwndRender : HwndHost
    {
        new public IntPtr Handle
        {
            get { return (IntPtr)GetValue(HandleProperty); }
            set { SetValue(HandleProperty, value); }
        }
        // Using a DependencyProperty as the backing store for Hwnd.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HandleProperty =
            DependencyProperty.Register("Handle", typeof(IntPtr), typeof(HwndRender), new PropertyMetadata(IntPtr.Zero));
        protected override HandleRef BuildWindowCore(HandleRef hwndParent)
        {
            Handle = CreateWindowEx(
                0, "static", "",
                WS_CHILD | WS_VISIBLE | LBS_NOTIFY,
                0, 0,
                (int)Width, (int)Height,
                hwndParent.Handle,
                IntPtr.Zero,
                IntPtr.Zero,
                0);

            var href = new HandleRef(this, Handle);
            var h = href.Handle;
            return href;
        }
        //protected override IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        //{
        //    handled = false;
        //    return IntPtr.Zero;
        //}

        protected override void DestroyWindowCore(HandleRef hwnd)
        {
            DestroyWindow(hwnd.Handle);
        }
        const int WS_CHILD = 0x40000000;
        const int WS_VISIBLE = 0x10000000;
        const int LBS_NOTIFY = 0x001;
        [DllImport("user32.dll")]
        internal static extern IntPtr CreateWindowEx(int exStyle, string className, string windowName, int style, int x, int y, int width, int height, IntPtr hwndParent, IntPtr hMenu, IntPtr hInstance, [MarshalAs(UnmanagedType.AsAny)] object pvParam);
        [DllImport("user32.dll")]
        public static extern bool DestroyWindow(IntPtr hwnd);
    }
}
