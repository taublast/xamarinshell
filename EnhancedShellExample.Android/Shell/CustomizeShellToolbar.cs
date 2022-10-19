using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using EnhancedShellExample.Infrastructure;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Platform.Android;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;

namespace AppoMobi.Framework.Droid.Renderers
{
    public class CustomizeShellToolbar : ShellToolbarAppearanceTracker
    {
        private AppShellRenderer myShellRenderer;

        public override void SetAppearance(Toolbar toolbar, IShellToolbarTracker toolbarTracker, ShellAppearance appearance)
        {
            base.SetAppearance(toolbar, toolbarTracker, appearance);

            // toolbar.SetPadding(0, (int)Manager.UI.StatusBarHeight, 0, 0);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.Kitkat)
            {
                var p = toolbar.LayoutParameters as ViewGroup.MarginLayoutParams;

                var add = (int)((EnhancedShell.StatusBarHeight) * EnhancedShell.Screen.Density);
                //if (add == 0)
                //{
                //   // var check = NativePlatform.Activity.Window.DecorView.RootWindowInsets; //null =(
                //    //add = Android.App.Application.Context.wNativePlatform.Activity.Window.DecorView.RootWindowInsets.SystemWindowInsetTop;
                //}
                //p.TopMargin = add;
                p.Height = (int)(EnhancedShell.NavBarHeight * EnhancedShell.Screen.Density) + add;

                toolbar.LayoutParameters = p;
                toolbar.SetPadding(0, add, 0, 0);

            }

            toolbar.SetContentInsetsAbsolute(0, 0);

            for (int i = 0; i < toolbar.ChildCount; i++)
            {
                var view = toolbar.GetChildAt(i);
                var label = view as TextView;
                if (label != null && label.Text == toolbar.Title)
                {

                    var info = FontRegistrar.HasFont("FontText");
                    if (info.hasFont)
                    {
                        label.Typeface = Typeface.CreateFromFile(info.fontPath);//.createFromAsset(view.context.assets, "fonts/customFont");
                    }
                    break;
                }
            }
        }

        public CustomizeShellToolbar(AppShellRenderer myShellRenderer) : base(myShellRenderer)
        {
            this.myShellRenderer = myShellRenderer;


        }

        public void Dispose()
        {

        }

        public void ResetAppearance(Toolbar toolbar, IShellToolbarTracker toolbarTracker)
        {

        }


    }
}