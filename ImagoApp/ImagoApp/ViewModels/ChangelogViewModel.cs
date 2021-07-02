using Xamarin.Forms;

namespace ImagoApp.ViewModels
{
    public class ChangelogViewModel : Util.BindableBase
    {
        private readonly Services.IWikiService _wikiService;

        public ChangelogViewModel(Services.IWikiService wikiService)
        {
            _wikiService = wikiService;
            LoadWikiPage();
        }

        private void LoadWikiPage()
        {
            var html = _wikiService.GetChangelogHtml();
            var url = _wikiService.GetChangelogUrl();
            ChangelogWikiView = new HtmlWebViewSource()
            {
                BaseUrl = url,
                Html = html.Replace("[Download]", "")
            };
        }

        private HtmlWebViewSource _changelogWikiView;
        public HtmlWebViewSource ChangelogWikiView
        {
            get => _changelogWikiView;
            set => SetProperty(ref _changelogWikiView, value);
        }
    }
}
