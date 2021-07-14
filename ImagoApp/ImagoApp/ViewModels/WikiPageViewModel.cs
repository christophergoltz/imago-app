using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using ImagoApp.Application;
using ImagoApp.Application.Models;
using ImagoApp.Util;
using Xamarin.Forms;

namespace ImagoApp.ViewModels
{
    public class WikiPageViewModel : BindableBase
    {
        private WikiTabModel _selectedWikiTab;
        public ObservableCollection<WikiTabModel> WikiTabList { get; set; }

        public WikiTabModel SelectedWikiTab
        {
            get => _selectedWikiTab;
            set => SetProperty(ref _selectedWikiTab, value);
        }

        public WikiPageViewModel()
        {
            WikiTabList = new ObservableCollection<WikiTabModel>
            {
                new WikiTabModel(WikiConstants.WikiMainPageUrl)
                {
                    Title = "Startseite"
                }
            };
        }

        private ICommand _closeWikiTabCommand;
        public ICommand CloseWikiTabCommand => _closeWikiTabCommand ?? (_closeWikiTabCommand = new Command<WikiTabModel>(wikiTabModel =>
        {
            WikiTabList.Remove(wikiTabModel);
        }));

        private ICommand _openWikiPageCommand;
        public ICommand OpenWikiPageCommand => _openWikiPageCommand ?? (_openWikiPageCommand = new Command<string>(url =>
        {
            OpenWikiPage(url);
        }));

        private ICommand _goBackToWikiMainpageCommand;
        public ICommand GoBackToWikiMainpageCommand => _goBackToWikiMainpageCommand ?? (_goBackToWikiMainpageCommand = new Command(() =>
        {
            SelectedWikiTab = WikiTabList[0];
        }));

        public void OpenWikiPage(string url)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                var wikiTabModel = new WikiTabModel(url);
                WikiTabList.Add(wikiTabModel);
                SelectedWikiTab = wikiTabModel;
            });
        }
    }
}