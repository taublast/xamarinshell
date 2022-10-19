using AppoMobi.Framework.iOS.Renderers;
using EnhancedShellExample.Infrastructure;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(EnhancedShell), typeof(AppShellRenderer))]
namespace AppoMobi.Framework.iOS.Renderers
{
    public class AppShellRenderer : ShellRenderer
    {
        public static void Init()
        {

            EnhancedShell.Screen.Density = UIScreen.MainScreen.Scale;
            EnhancedShell.Screen.WidthDip = UIScreen.MainScreen.Bounds.Width;
            EnhancedShell.Screen.HeightDip = UIScreen.MainScreen.Bounds.Height;

            if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
            {
                var window = new UIWindow(frame: UIScreen.MainScreen.Bounds)
                { BackgroundColor = Xamarin.Forms.Color.Transparent.ToUIColor() };

                EnhancedShell.Screen.TopInset = (int)(window.SafeAreaInsets.Top);
                EnhancedShell.Screen.BottomInset = (int)(window.SafeAreaInsets.Bottom);
                EnhancedShell.Screen.LeftInset = (int)(window.SafeAreaInsets.Left);
                EnhancedShell.Screen.RightInset = (int)(window.SafeAreaInsets.Right);
            }

            EnhancedShell.StatusBarHeight = EnhancedShell.Screen.TopInset;
            if (EnhancedShell.StatusBarHeight == 0)
                EnhancedShell.StatusBarHeight = 20;

            EnhancedShell.NavBarHeight = 47; //manual

        }

        public static UINavigationController NavigationController { get; set; }

        public EnhancedShell FormsControl;

        protected override void OnCurrentItemChanged()
        {
            base.OnCurrentItemChanged();
        }





        public override void ViewWillAppear(bool animated)
        {

            base.ViewWillAppear(animated);

            //fixing BUG: first visible tab images will NOT be set
            // FormsControl.InvalidateTabs();
        }


        public override void ViewDidAppear(bool animated)
        {
            //if (!_onceInitTabs)
            //{
            //    _onceInitTabs = true;
            //    FormsControl.InvalidateTabs();
            //}

            base.ViewDidAppear(animated);

            //if (!FormsControl.TabsInitialized)
            //{
            //    Device.BeginInvokeOnMainThread(() =>
            //    {
            //        FormsControl.InvalidateTabs();
            //    });
            //}

        }


        protected override void OnElementSet(Shell shell)
        {
            base.OnElementSet(shell);

            FormsControl = shell as EnhancedShell;
        }

        //protected override IShellSectionRenderer CreateShellSectionRenderer(ShellSection shellSection)
        //{
        //    var renderer = base.CreateShellSectionRenderer(shellSection);
        //    if (renderer != null)
        //    {

        //    }
        //    return renderer;
        //}

        //protected override IShellNavBarAppearanceTracker CreateNavBarAppearanceTracker()
        //{
        //    return new CustomizeNavigationBar();

        //    //            return base.CreateNavBarAppearanceTracker();
        //}

        protected override IShellTabBarAppearanceTracker CreateTabBarAppearanceTracker()
        {
            return new CustomizeBottomMenuItems(this);
        }

    }
}

