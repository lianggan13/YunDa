using System.Windows;
using System.Windows.Media;

namespace Y.ASIS.App.Res.Helper
{
    public class IconHelper
    {
        public static Geometry GetIcon(DependencyObject obj)
        {
            return (Geometry)obj.GetValue(IconProperty);
        }

        public static void SetIcon(DependencyObject obj, Geometry value)
        {
            obj.SetValue(IconProperty, value);
        }

        public static readonly DependencyProperty IconProperty =
            DependencyProperty.RegisterAttached("Icon", typeof(Geometry), typeof(IconHelper), new PropertyMetadata(Geometry.Empty));
    }
}
