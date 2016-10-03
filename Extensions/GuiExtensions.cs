using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Rair.Utilities.Windows.Extensions
{
    public static class GuiExtensions
    {
        public static T FindParent<T>(this DependencyObject child) where T : DependencyObject
        {
            while (true)
            {
                var parentObject = VisualTreeHelper.GetParent(child);
                if (parentObject == null) { return null; }
                var parent = parentObject as T;
                if (parent != null) { return parent; }
                child = parentObject;
            }
        }

        public static DependencyObject GetTopmostParent(this DependencyObject element)
        {
            var current = element;
            var result = element;

            while (current != null)
            {
                result = current;
                current = (current is Visual || current is Visual3D) ?
                   VisualTreeHelper.GetParent(current) :
                   LogicalTreeHelper.GetParent(current);
            }
            return result;
        }
    }
}
