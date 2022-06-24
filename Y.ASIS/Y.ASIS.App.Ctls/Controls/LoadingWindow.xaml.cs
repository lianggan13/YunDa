using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;


namespace Y.ASIS.App.Ctls.Controls
{
    /// <summary>
    /// LoadingWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LoadingWindow : Window
    {
        public LoadingWindow()
        {
            InitializeComponent();
        }
        public double Radius
        {
            get { return (double)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            Refresh(Radius);
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.Register(nameof(Radius), typeof(double), typeof(LoadingWindow), new FrameworkPropertyMetadata(60d,
        FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, Radius_PropertyChangedCallback));

        private static void Radius_PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as LoadingWindow).Refresh((double)e.NewValue);
        }

        private void Refresh(double radius)
        {
            viewbox.Width = radius * 2;
            viewbox.Height = radius * 2;

            var ellipses = canvas.Children.OfType<UIElement>().Where(u => u is Ellipse);
            for (int i = 0; i < ellipses.Count(); i++)
            {
                var ellipse = ellipses.ElementAt(i);
                MeasureXY(i, out double x, out double y);

                Canvas.SetLeft(ellipse, x);
                Canvas.SetTop(ellipse, y);
            }
        }

        private void MeasureXY(int i, out double x, out double y)
        {
            double Virtual_Radius = 50;
            x = Virtual_Radius + Virtual_Radius * Math.Sin(i * Math.PI * 2 / 10.0);
            y = Virtual_Radius + Virtual_Radius * Math.Cos(i * Math.PI * 2 / 10.0);
        }


        public static async Task Show(Window owner, Action action)
        {
            LoadingWindow loading = new LoadingWindow();

            await Task.Run(() =>
            {
                Task.Run(() =>
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        loading.Owner = owner;
                       //loading.Width = loading.Owner.Width;
                       //loading.Height = loading.Owner.Height;
                       loading.ShowDialog();
                    });
                });
            });

            await Task.Run(() =>
            {
                action?.Invoke();
            }).ConfigureAwait(true);

            loading.Close();
        }
    }
}
