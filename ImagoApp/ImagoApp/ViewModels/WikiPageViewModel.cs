using System.Collections.ObjectModel;
using System.Linq;

namespace ImagoApp.ViewModels
{
    public class WikiPageViewModel : Util.BindableBase
    {
        private WikiEntryPageViewModel _selectedWikiPageEntry;
        public static Models.WikiPageEntry RequestedWikiPage { get; set; }
        public ObservableCollection<WikiEntryPageViewModel> WikiEntryList { get; set; }
        public static WikiPageViewModel Instance { get; set; }

        public WikiEntryPageViewModel SelectedWikiPageEntry
        {
            get => _selectedWikiPageEntry;
            set => SetProperty(ref _selectedWikiPageEntry, value);
        }

        public WikiPageViewModel()
        {
            WikiEntryList = new ObservableCollection<WikiEntryPageViewModel>()
            {
                CreateNewWikiEntryPageViewModel(new Models.WikiPageEntry(Util.WikiConstants.WikiMainPageUrl) {Title = "Startseite"})
            };
        }

        public void OpenWikiPage()
        {
            if (RequestedWikiPage == null)
                return;

            var vm = CreateNewWikiEntryPageViewModel(RequestedWikiPage);
            WikiEntryList.Add(vm);
            SelectedWikiPageEntry = vm;

            RequestedWikiPage = null;
        }

        public void GoToStartWikiPage()
        {
            SelectedWikiPageEntry = WikiEntryList[0];
        }

        private WikiEntryPageViewModel CreateNewWikiEntryPageViewModel(Models.WikiPageEntry entry)
        {
            var vm = new WikiEntryPageViewModel(entry);

            vm.PageCloseRequested += (sender, model) =>
            {
                WikiEntryList.Remove(model);
                SelectedWikiPageEntry = WikiEntryList.LastOrDefault();
            };

            return vm;
        }

    }
}