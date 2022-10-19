using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace AppoMobi.Framework.Droid.Renderers
{
    public partial class AppShellRenderer
    {
        public class CustomShellItemRenderer : ShellItemRenderer
        {
            private AppShellRenderer _parent;

            public CustomShellItemRenderer(IShellContext shellContext) : base(shellContext)
            {
                _parent = shellContext as AppShellRenderer;
            }

            protected override void OnTabReselected(ShellSection shellSection)
            {
                base.OnTabReselected(shellSection);

                _parent.FormsControl.SetTabReselected(shellSection.TabIndex);
            }

            protected override void OnShellSectionChanged()
            {
                base.OnShellSectionChanged();


                //if (_parent.FormsControl is IShellController controller)
                //{
                //    Device.StartTimer(TimeSpan.FromMilliseconds(50), () =>
                //    {
                //        controller.AppearanceChanged(ShellItemController as Element, false); ;
                //        return false;
                //    });
                //}
            }

            protected override void Dispose(bool disposing)
            {
                if (disposing)
                {
                    _parent = null;
                }

                base.Dispose(disposing);
            }
        }
    }
}