global using Xamarin.Forms;
global using Xamarin.Forms.Xaml;
using EnhancedShellExample.Content.Services;
using EnhancedShellExample.Content.ViewModels;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace EnhancedShellExample
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();

            DependencyService.RegisterSingleton(new StateViewModel());

            MainPage = new AppShell();

            //MainPage = new AppShellStandart();

        }

        public static Application Instance
        {
            get
            {
                return Current as Application;
            }
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
