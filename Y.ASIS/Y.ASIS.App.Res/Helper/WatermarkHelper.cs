using System.Windows;
using System.Windows.Media;

namespace Y.ASIS.App.Res.Helper
{
    public class WatermarkHelper
    {
        public static readonly DependencyProperty WatermarkProperty = DependencyProperty.RegisterAttached(
            "Watermark",
            typeof(string),
            typeof(WatermarkHelper),
            new FrameworkPropertyMetadata(default, FrameworkPropertyMetadataOptions.Inherits));

        public static readonly DependencyProperty WatermarkBrushProperty = DependencyProperty.RegisterAttached(
            "WatermarkBrush",
            typeof(Brush),
            typeof(WatermarkHelper),
            new PropertyMetadata(default));

        public static string GetWatermark(DependencyObject obj)
        {
            return (string)obj.GetValue(WatermarkProperty);
        }

        public static void SetWatermark(DependencyObject obj, string value)
        {
            obj.SetValue(WatermarkProperty, value);
        }

        public static Brush GetWatermarkBrush(DependencyObject obj)
        {
            return (Brush)obj.GetValue(WatermarkBrushProperty);
        }

        public static void SetWatermarkBrush(DependencyObject obj, Brush value)
        {
            obj.SetValue(WatermarkBrushProperty, value);
        }
    }
}
