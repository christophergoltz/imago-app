using System;
using System.Diagnostics;
using System.Windows.Input;
using Xamarin.Forms;

namespace ImagoApp.ViewModels
{
    public class WikiEntryPageViewModel : Util.BindableBase
    {
        public event EventHandler<WikiEntryPageViewModel> PageCloseRequested;

        public ICommand ClosePageCommand { get; }
        public Models.WikiPageEntry WikiPageEntry { get; set; }

        public WikiEntryPageViewModel(Models.WikiPageEntry entry)
        {
            WikiPageEntry = entry;
            WikiPageEntry.PropertyChanged += (sender, args) => { ((Command) ClosePageCommand).ChangeCanExecute(); };

            ClosePageCommand = new Command(() =>
            {
                Debug.WriteLine("Wikipage close requested: " + entry.Url);
                PageCloseRequested?.Invoke(this, this);
            }, () =>
            {
                var isClosable = WikiPageEntry.Url != Util.WikiConstants.WikiMainPageUrl;
                Debug.WriteLine("Check if " + WikiPageEntry.Url + " is closable.. " + isClosable);
                return isClosable;
            });
        }
    }
}
