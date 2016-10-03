using System;
using System.Configuration;
using System.Linq;
using System.Windows;
using Rair.Utilities.Core;
using Rair.Utilities.Core.Extensions;

namespace Rair.Utilities.Windows.Extensions
{
    public static class WindowExtensions
    {
        public static bool IsOpen(this Window window)
        {
            return window != null && WindowIsOpen(window);
        }

        public static bool WindowIsOpen<T>(T t) where T : Window
        {
            var open = false;
            Application.Current.Dispatcher.Invoke(() =>
            {
                open = Application.Current.Windows.OfType<T>().Any(w => w.Name.Equals(t.Name));
            });
            return t != null && open;
        }

        public static bool? ShowModal(this Window window, Window parent = null)
        {
            var activeWindow = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
            window.Owner = parent ?? activeWindow;
            return window.ShowDialog();
        }

        public static Result SaveSizeAndPosition(this Window window, ApplicationSettingsBase settings, string name = "")
        {
            if (window == null) { return Result.Failure("Windows was null"); }
            if (window.Name.IsEmpty() && name.IsEmpty()) { return Result.Failure("Cannot save settings for a window with no name."); }

            var sizeRes = window.SaveSize(settings, name);
            var posRes = window.SavePosition(settings, name);
            settings.Save();

            return Result.Success();
        }

        private static Result SaveSize(this Window window, ApplicationSettingsBase settings, string name = "")
        {
            try
            {
                var prefix = name.IsEmpty() ? window.Name : name;

                var settingName = prefix + "_Size";
                settings.SaveSetting(settingName, new Size(window.Width, window.Height));
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(ex);
            }
        }

        private static Result SavePosition(this Window window, ApplicationSettingsBase settings, string name = "")
        {
            try
            {
                var prefix = name.IsEmpty() ? window.Name : name;
                var settingName = prefix + "_Position";
                settings.SaveSetting(settingName, new Point(window.Left, window.Top));
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(ex);
            }
        }

        public static Result LoadSizeAndPosition(this Window window, ApplicationSettingsBase settings, string name = "")
        {
            if (window == null) { return Result.Failure("Windows was null"); }
            if (window.Name.IsEmpty() && name.IsEmpty()) { return Result.Failure("Cannot save settings for a window with no name."); }

            var sizeRes = window.LoadSize(settings, name);
            var posRes = window.LoadPosition(settings, name);
            return Result.Success();
        }

        private static Result LoadSize(this Window window, ApplicationSettingsBase settings, string name = "")
        {
            try
            {
                var prefix = name.IsEmpty() ? window.Name : name;
                var settingName = prefix + "_Size";
                var size = settings.LoadSetting<Size>(settingName);
                if (Math.Abs(size.Width) < .001 && Math.Abs(size.Height) < .001)
                {
                    size.Width = 450;
                    size.Height = 450;
                }
                window.Width = size.Width;
                window.Height = size.Height;

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(ex);
            }
        }

        private static Result LoadPosition(this Window window, ApplicationSettingsBase settings, string name = "")
        {
            try
            {
                var prefix = name.IsEmpty() ? window.Name : name;
                var settingName = prefix + "_Position";
                var position = settings.LoadSetting<Point>(settingName);
                if (Math.Abs(position.X) < .001 && Math.Abs(position.Y) < .001)
                {
                    position.X = 100;
                    position.Y = 100;
                }
                window.Top = position.Y;
                window.Left = position.X;

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(ex);
            }
        }
    }
}
