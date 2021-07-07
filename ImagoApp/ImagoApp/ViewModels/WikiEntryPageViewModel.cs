using System;
using System.Diagnostics;
using System.Windows.Input;
using ImagoApp.Application.Models;
using ImagoApp.Util;
using Xamarin.Forms;

namespace ImagoApp.ViewModels
{
    public class WikiEntryPageViewModel : BindableBase
    {
        public event EventHandler<WikiEntryPageViewModel> PageCloseRequested;

        public ICommand ClosePageCommand { get; }
        public WikiPageEntry WikiPageEntry { get; set; }
        public event EventHandler<string> OpenWikiPageRequested;

        public void RaiseOpenWikiPageRequested(string url)
        {
            OpenWikiPageRequested?.Invoke(this, url);
        }

        public WikiEntryPageViewModel(WikiPageEntry entry)
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
