using EnhancedShellExample.Content.ViewModels;
using EnhancedShellExample.Content.Views;
using EnhancedShellExample.Infrastructure;

namespace EnhancedShellExample
{
    public partial class AppShell : EnhancedShell
    {
        public override void OnNavBarInvalidated()
        {
            base.OnNavBarInvalidated();

            _viewModel.UpdateControls();
        }

        public AppShell()
        {
            _viewModel = DependencyService.Get<StateViewModel>();
            BindingContext = _viewModel;

            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
        }


        private readonly StateViewModel _viewModel;


    }
}