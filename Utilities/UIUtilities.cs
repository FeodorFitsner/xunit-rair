using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Rair.Utilities.Windows.Utilities
{
    public static class UIUtilities
    {
        public static void AddBinding(this DependencyObject control, object bindingSource, string bindingPath)
        {
            var myBinding = new Binding
            {
                Source = bindingSource,
                Path = new PropertyPath(bindingPath),
                Mode = BindingMode.TwoWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };
            BindingOperations.SetBinding(control, UIElement.VisibilityProperty, myBinding);
        }
    }
}
