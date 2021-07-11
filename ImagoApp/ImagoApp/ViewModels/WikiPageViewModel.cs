using System;
using System.Collections.ObjectModel;
using System.Linq;
using ImagoApp.Application;
using ImagoApp.Application.Models;
using ImagoApp.Util;

namespace ImagoApp.ViewModels
{
    public class WikiPageViewModel : BindableBase
    {
        private WikiEntryPageViewModel _selectedWikiPageEntry;
        public ObservableCollection<WikiEntryPageViewModel> WikiEntryList { get; set; }
        public static WikiPageViewModel Instance { get; set; }

        public event EventHandler<string> OpenWikiPageRequested;

        public WikiEntryPageViewModel SelectedWikiPageEntry
        {
            get => _selectedWikiPageEntry;
            set => SetProperty(ref _selectedWikiPageEntry, value);
        }

        public WikiPageViewModel()
        {
            WikiEntryList = new ObservableCollection<WikiEntryPageViewModel>()
            {
                CreateNewWikiEntryPageViewModel(new WikiPageEntry(WikiConstants.WikiMainPageUrl)
                {
                    Title = "Startseite"
                })
            };
        }

        public void OpenWikiPage(string url)
        {
            var vm = CreateNewWikiEntryPageViewModel(new WikiPageEntry(url));
            WikiEntryList.Add(vm);
            SelectedWikiPageEntry = vm;
        }

        public void GoToStartWikiPage()
        {
            SelectedWikiPageEntry = WikiEntryList[0];
        }

        private WikiEntryPageViewModel CreateNewWikiEntryPageViewModel(WikiPageEntry entry)
        {
            var viewModel = new WikiEntryPageViewModel(entry);

            viewModel.PageCloseRequested += (sender, model) =>
            {
                WikiEntryList.Remove(model);
                SelectedWikiPageEntry = WikiEntryList.LastOrDefault();
            };
            viewModel.OpenWikiPageRequested += (sender, s) =>
            {
                OpenWikiPageRequested?.Invoke(this, s);
            };

            return viewModel;
        }

    }
}