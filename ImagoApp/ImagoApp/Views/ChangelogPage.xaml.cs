using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ImagoApp.Views
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
            if (BindingContext is ViewModels.ChangelogViewModel viewModel)
            {
                if (e.Url != viewModel.ChangelogWikiView.BaseUrl)
                    e.Cancel = true;
            }
        }
    }
}