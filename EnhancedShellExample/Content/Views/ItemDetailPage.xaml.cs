using EnhancedShellExample.Content.ViewModels;
using Xamarin.Forms;

namespace EnhancedShellExample.Content.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}