using CoreGraphics;
using System;
using System.Diagnostics;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace AppoMobi.Framework.iOS.Renderers
{
    public class CustomizeShellToolbar : IShellNavBarAppearanceTracker, IDisposable
    {
        private UIColor _defaultBarTint;

        private UIColor _defaultTint;

        private UIStringAttributes _defaultTitleAttributes;

        private float _shadowOpacity = float.MinValue;

        private CGColor _shadowColor;

        public void UpdateLayout(UINavigationController controller)
        {
        }

        public void ResetAppearance(UINavigationController controller)
        {
            if (_defaultTint != null)
            {
                UINavigationBar navigationBar = controller.NavigationBar;
                navigationBar.BarTintColor = _defaultBarTint;
                navigationBar.TintColor = _defaultTint;
                navigationBar.TitleTextAttributes = _defaultTitleAttributes;
            }
        }

        public void SetAppearance(UINavigationController controller, ShellAppearance appearance)
        {
            AppShellRenderer.NavigationController = controller;

            UINavigationBar navigationBar = controller.NavigationBar;
            if (_defaultTint == null)
            {
                _defaultBarTint = navigationBar.BarTintColor;
                _defaultTint = navigationBar.TintColor;
                _defaultTitleAttributes = navigationBar.TitleTextAttributes;
            }

            if (UIDevice.CurrentDevice.CheckSystemVersion(15, 0))
            {
                UpdateiOS15NavigationBarAppearance(controller, appearance);
            }
            else
            {
                UpdateNavigationBarAppearance(controller, appearance);
            }

            var value = controller.PreferredStatusBarStyle();
            Debug.WriteLine($"[CHECK1] {value}");

        }
        private void UpdateNavigationBarAppearance(UINavigationController controller, ShellAppearance appearance)
        {


            Color backgroundColor = appearance.BackgroundColor;

            Color foregroundColor = appearance.ForegroundColor;

            Color titleColor = appearance.TitleColor;

            UINavigationBar navigationBar = controller.NavigationBar;
            if (!backgroundColor.IsDefault)
            {
                navigationBar.BarTintColor = backgroundColor.ToUIColor();
            }

            if (!foregroundColor.IsDefault)
            {
                navigationBar.TintColor = foregroundColor.ToUIColor();
            }

            if (!titleColor.IsDefault)
            {
                navigationBar.TitleTextAttributes = new UIStringAttributes
                {
                    ForegroundColor = titleColor.ToUIColor()
                };
            }

            var value = controller.PreferredStatusBarStyle();
            Debug.WriteLine($"[CHECK2] {value}");

        }
        private void UpdateiOS15NavigationBarAppearance(UINavigationController controller, ShellAppearance appearance)
        {
            UINavigationBar navigationBar = controller.NavigationBar;
            UINavigationBarAppearance uINavigationBarAppearance = new UINavigationBarAppearance();
            uINavigationBarAppearance.ConfigureWithOpaqueBackground();
            navigationBar.Translucent = false;
            Color foregroundColor = appearance.ForegroundColor;
            if (!foregroundColor.IsDefault)
            {
                navigationBar.TintColor = foregroundColor.ToUIColor();
            }

            Color backgroundColor = appearance.BackgroundColor;
            if (!backgroundColor.IsDefault)
            {
                uINavigationBarAppearance.BackgroundColor = backgroundColor.ToUIColor();
            }

            Color titleColor = appearance.TitleColor;
            if (!titleColor.IsDefault)
            {
                uINavigationBarAppearance.TitleTextAttributes = new UIStringAttributes
                {
                    ForegroundColor = titleColor.ToUIColor()
                };
            }

            UINavigationBarAppearance uINavigationBarAppearance4 = navigationBar.StandardAppearance = (navigationBar.ScrollEdgeAppearance = uINavigationBarAppearance);
        }

        public virtual void SetHasShadow(UINavigationController controller, bool hasShadow)
        {
            UINavigationBar navigationBar = controller.NavigationBar;
            if (_shadowOpacity == float.MinValue)
            {
                if (!hasShadow)
                {
                    return;
                }

                _shadowOpacity = navigationBar.Layer.ShadowOpacity;
                _shadowColor = navigationBar.Layer.ShadowColor;
            }

            if (hasShadow)
            {
                navigationBar.Layer.ShadowColor = UIColor.Black.CGColor;
                navigationBar.Layer.ShadowOpacity = 1f;
            }
            else
            {
                navigationBar.Layer.ShadowColor = _shadowColor;
                navigationBar.Layer.ShadowOpacity = _shadowOpacity;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
        }

        public void Dispose()
        {
            Dispose(disposing: true);
        }
    }
}