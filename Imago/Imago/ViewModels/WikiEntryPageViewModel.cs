using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Input;
using Imago.Models;
using Imago.Util;
using Xamarin.Forms;

namespace Imago.ViewModels
{
    public class WikiEntryPageViewModel : BindableBase
    {
        public event EventHandler<WikiEntryPageViewModel> PageCloseRequested;

        public ICommand ClosePageCommand { get; }
        public WikiPageEntry WikiPageEntry { get; set; }

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
                var isClosable = WikiPageEntry.Url != WikiPageViewModel.WikiMainPageUrl;
                Debug.WriteLine("Check if " + WikiPageEntry.Url + " is closable.. " + isClosable);
                return isClosable;
            });
        }
    }
}
