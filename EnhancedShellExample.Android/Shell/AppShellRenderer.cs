using Android.App;
using Android.Content;
using Android.Views;
using AppoMobi.Framework.Droid.Renderers;
using EnhancedShellExample.Infrastructure;
using System.Reflection;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

//disabled
[assembly: ExportRenderer(typeof(EnhancedShell), typeof(AppShellRenderer))]
namespace AppoMobi.Framework.Droid.Renderers
{


    public partial class AppShellRenderer : ShellRenderer
    {
        public static void Init(Activity activity)
        {

            EnhancedShell.Screen.Density = activity.Resources.DisplayMetrics.Density;

            EnhancedShell.Screen.WidthDip = activity.Resources.DisplayMetrics.WidthPixels / EnhancedShell.Screen.Density;
            EnhancedShell.Screen.HeightDip = activity.Resources.DisplayMetrics.HeightPixels / EnhancedShell.Screen.Density;

            EnhancedShell.NavBarHeight = 45; //manual

            //var flags = Activity.Window.Attributes.Flags;

            var isFullscreen = (int)activity.Window.DecorView.SystemUiVisibility & (int)SystemUiFlags.LayoutStable;

            //if (((flags & WindowManagerFlags.TranslucentStatus) == WindowManagerFlags.TranslucentStatus) || isFullscreen>0 || (Activity.Window.StatusBarColor == Android.Graphics.Color.Transparent))
            if (!(isFullscreen > 0))
            {
                EnhancedShell.StatusBarHeight = GetStatusBarHeight(activity) / EnhancedShell.Screen.Density;
            }
            else
            {
                EnhancedShell.StatusBarHeight = 0;
            }

            //transparent status bar
            //if (Build.VERSION.SdkInt >= BuildVersionCodes.Kitkat)
            //{
            //    Activity.Window.SetFlags(WindowManagerFlags.LayoutNoLimits, WindowManagerFlags.LayoutNoLimits);
            //    var parentView = Activity.FindViewById(2131230767);
            //    insetsListener = new InsetsListener();
            //    parentView.SetOnApplyWindowInsetsListener(insetsListener);
            //}

        }

        public static int GetStatusBarHeight(Context context)
        {

            int statusBarHeight = 0, totalHeight = 0, contentHeight = 0;
            int resourceId = context.Resources.GetIdentifier("status_bar_height", "dimen", "android");
            if (resourceId > 0)
            {
                statusBarHeight = context.Resources.GetDimensionPixelSize(resourceId);
                totalHeight = context.Resources.DisplayMetrics.HeightPixels;
                contentHeight = totalHeight - statusBarHeight;
                statusBarHeight = statusBarHeight;
            }

            return statusBarHeight;
        }

        protected override IShellItemRenderer CreateShellItemRenderer(ShellItem shellItem)
        {
            return new CustomShellItemRenderer(this);
        }


        //protected override IShellToolbarAppearanceTracker CreateToolbarAppearanceTracker()
        //{
        //    return new CustomizeShellToolbar(this);
        //}

        protected override IShellBottomNavViewAppearanceTracker CreateBottomNavViewAppearanceTracker(ShellItem shellItem)
        {
            return new CustomizeBottomMenuItems(this, shellItem);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {

            }

            base.Dispose(disposing);
        }

        protected override void OnElementSet(Shell shell)
        {
            base.OnElementSet(shell);

            FormsControl = shell as EnhancedShell;

            var myBase = this.GetType().BaseType;
            var frame = myBase.GetField("_frameLayout", BindingFlags.NonPublic | BindingFlags.Instance);
            if (frame == null)
                return;

            FrameLayout = (CustomFrameLayout)frame.GetValue(this);

            FrameLayout.SetFitsSystemWindows(false);


        }

        public CustomFrameLayout FrameLayout { get; set; }

        public EnhancedShell FormsControl;



        public AppShellRenderer(Context context) : base(context)
        {

        }




    }



}


