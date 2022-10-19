using EnhancedShellExample.Content.Views;
using Xamarin.Forms;

namespace EnhancedShellExample
{
    public partial class AppShellStandart : Xamarin.Forms.Shell
    {
        public AppShellStandart()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
        }

    }
}
