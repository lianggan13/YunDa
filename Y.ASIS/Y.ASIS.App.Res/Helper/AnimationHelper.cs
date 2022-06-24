using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace Y.ASIS.App.Res.Helper
{
    public class AnimationHelper
    {
        private readonly static Storyboard flashStoryboard;

        static AnimationHelper()
        {
            flashStoryboard = new Storyboard()
            {
                AutoReverse = true,
                RepeatBehavior = RepeatBehavior.Forever
            };
            DoubleAnimationUsingKeyFrames keyFrames = new DoubleAnimationUsingKeyFrames();
            Storyboard.SetTargetProperty(keyFrames, new PropertyPath(UIElement.OpacityProperty));
            keyFrames.KeyFrames.Add(new SplineDoubleKeyFrame(1, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(0))));
            keyFrames.KeyFrames.Add(new SplineDoubleKeyFrame(.1, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(600)), new KeySpline(.4, 0, .6, 1)));
            flashStoryboard.Children.Add(keyFrames);
        }

        public static bool GetFlash(DependencyObject obj)
        {
            return (bool)obj.GetValue(FlashProperty);
        }

        public static void SetFlash(DependencyObject obj, bool value)
        {
            obj.SetValue(FlashProperty, value);
        }

        public static readonly DependencyProperty FlashProperty =
            DependencyProperty.RegisterAttached("Flash", typeof(bool), typeof(AnimationHelper), new PropertyMetadata(false, OnFlashChanged));

        private static void OnFlashChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement element = d as FrameworkElement;
            if ((bool)e.NewValue)
            {
                flashStoryboard.Begin(element, true);
            }
            else
            {
                flashStoryboard.Stop(element);
            }
        }
    }
}
