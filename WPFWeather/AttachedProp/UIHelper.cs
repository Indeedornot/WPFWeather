using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace WPFWeather.AttachedProp;
internal static class UIHelper {
    internal static IList<T> FindChildren<T>(DependencyObject element) where T : FrameworkElement {
        List<T> retval = new();

        for (int counter = 0; counter < VisualTreeHelper.GetChildrenCount(element); counter++) {
            FrameworkElement? toadd = VisualTreeHelper.GetChild(element, counter) as FrameworkElement;
            if (toadd == null) continue;

            if (toadd is T correctlyTyped) {
                retval.Add(correctlyTyped);
            }
            else {
                retval.AddRange(FindChildren<T>(toadd));
            }
        }

        return retval;
    }

    internal static T? FindParent<T>(DependencyObject element) where T : FrameworkElement {
        FrameworkElement? parent = VisualTreeHelper.GetParent(element) as FrameworkElement;
        if (parent == null) return null;

        if (parent is T correctlyTyped) {
            return correctlyTyped;
        }

        return FindParent<T>(parent);
    }
}