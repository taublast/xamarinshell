using EnhancedShellExample.Content.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EnhancedShellExample.Content.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            this.BindingContext = new LoginViewModel();
        }
    }
}