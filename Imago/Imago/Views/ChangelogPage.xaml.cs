using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Imago.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Imago.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChangelogPage : ContentPage
    {
        public ChangelogPage()
        {
            InitializeComponent();
        }

        private void WebView_OnNavigating(object sender, WebNavigatingEventArgs e)
        {
            if (BindingContext is ChangelogViewModel viewModel)
            {
                if (e.Url != viewModel.ChangelogWikiView.BaseUrl)
                    e.Cancel = true;
            }
        }
    }
}