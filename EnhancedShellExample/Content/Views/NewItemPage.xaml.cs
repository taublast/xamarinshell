using EnhancedShellExample.Content.Models;
using EnhancedShellExample.Content.ViewModels;
using Xamarin.Forms;

namespace EnhancedShellExample.Content.Views
{
    public partial class NewItemPage : ContentPage
    {
        public Item Item { get; set; }

        public NewItemPage()
        {
            InitializeComponent();
            BindingContext = new NewItemViewModel();
        }
    }
}