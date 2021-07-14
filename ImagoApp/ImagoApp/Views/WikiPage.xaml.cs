using ImagoApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ImagoApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WikiPage : TabbedPage
    {
        public WikiPageViewModel WikiPageViewModel { get; }
        public WikiPage(WikiPageViewModel wikiPageViewModel)
        {
            BindingContext = WikiPageViewModel = wikiPageViewModel;
            InitializeComponent();
        }
    }
}