using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Y.ASIS.App.Utils
{
    class VisualTreeUtil
    {
        public static IEnumerable<T> GetChild<T>(DependencyObject dp) where T : FrameworkElement
        {
            if (dp != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(dp); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(dp, i);

                    if (child != null && child is T t)
                        yield return t;

                    foreach (T childOfChild in GetChild<T>(child))
                        yield return childOfChild;
                }
            }
        }

        public static T GetParent<T>(DependencyObject dp) where T : FrameworkElement
        {
            if (dp != null)
            {
                DependencyObject parent = VisualTreeHelper.GetParent(dp);
                if (parent is T t)
                {
                    return t;
                }
                if (parent == null)
                {
                    return default;
                }
                return GetParent<T>(parent);
            }
            return default;
        }
    }
}
