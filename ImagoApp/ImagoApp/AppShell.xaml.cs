using Xamarin.Forms;

namespace ImagoApp
{
    public partial class AppShell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(Views.SkillPage), typeof(Views.SkillPage));
            Routing.RegisterRoute(nameof(Views.StatusPage), typeof(Views.StatusPage));
            Routing.RegisterRoute(nameof(Views.InventoryPage), typeof(Views.InventoryPage));
            Routing.RegisterRoute(nameof(Views.WikiPage), typeof(Views.WikiPage));
            Routing.RegisterRoute(nameof(Views.ChangelogPage), typeof(Views.ChangelogPage));
            Routing.RegisterRoute(nameof(Views.PerksPage), typeof(Views.PerksPage));
        }
        
        private void AppShell_OnNavigated(object sender, ShellNavigatedEventArgs e)
        {
            if (sender is AppShell shell)
            {
                if (shell.CurrentPage is Views.WikiPage page)
                {
                    if (page.BindingContext is ViewModels.WikiPageViewModel viewModel)
                    {
                        if (ViewModels.WikiPageViewModel.Instance == null)
                            ViewModels.WikiPageViewModel.Instance = viewModel;

                        viewModel.OpenWikiPage();
                    }
                }
            }
        }
    }
}
