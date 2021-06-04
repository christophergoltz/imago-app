using Imago.ViewModels;
using Imago.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Imago
{
    public partial class AppShell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(SkillPage), typeof(SkillPage));
            Routing.RegisterRoute(nameof(StatusPage), typeof(StatusPage));
            Routing.RegisterRoute(nameof(InventoryPage), typeof(InventoryPage));
            Routing.RegisterRoute(nameof(WikiPage), typeof(WikiPage));
            Routing.RegisterRoute(nameof(ChangelogPage), typeof(ChangelogPage));
        }
        
        private void AppShell_OnNavigated(object sender, ShellNavigatedEventArgs e)
        {
            if (sender is AppShell shell)
            {
                if (shell.CurrentPage is WikiPage page)
                {
                    if (page.BindingContext is WikiPageViewModel viewModel)
                    {
                        if (WikiPageViewModel.Instance == null)
                            WikiPageViewModel.Instance = viewModel;

                        viewModel.OpenWikiPage();
                    }
                }
            }
        }
    }
}
