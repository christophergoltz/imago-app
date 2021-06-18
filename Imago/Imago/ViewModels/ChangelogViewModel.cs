using System;
using System.Collections.Generic;
using System.Text;
using Imago.Models;
using Imago.Repository;
using Imago.Services;
using Imago.Util;
using Xamarin.Forms;

namespace Imago.ViewModels
{
    public class ChangelogViewModel : BindableBase
    {
        private readonly IWikiService _wikiService;

        public ChangelogViewModel(IWikiService wikiService)
        {
            _wikiService = wikiService;
            LoadWikiPage();
        }

        private void LoadWikiPage()
        {
            var html = _wikiService.GetChangelogHtml(new List<string>() {"firstHeading", "siteSub", "contentSub", "jump-to-nav" });
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
