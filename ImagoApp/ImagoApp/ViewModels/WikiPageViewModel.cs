using System;
using System.Collections.Generic;
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
        public CharacterViewModel CharacterViewModel { get; }
        private WikiTabModel _selectedWikiTab;
        public WikiTabModel SelectedWikiTab
        {
            get => _selectedWikiTab;
            set => SetProperty(ref _selectedWikiTab, value);
        }

        public WikiPageViewModel(CharacterViewModel characterViewModel)
        {
            CharacterViewModel = characterViewModel;
            if (CharacterViewModel.CharacterModel.WikiPages == null)
                CharacterViewModel.CharacterModel.WikiPages = new ObservableCollection<WikiTabModel>();

            if (!CharacterViewModel.CharacterModel.WikiPages.Any())
            {
                CharacterViewModel.CharacterModel.WikiPages.Add(new WikiTabModel(WikiConstants.WikiMainPageUrl)
                {
                    Title = "Startseite"
                });
            }
        }

        private ICommand _closeWikiTabCommand;
        public ICommand CloseWikiTabCommand => _closeWikiTabCommand ?? (_closeWikiTabCommand = new Command<WikiTabModel>(wikiTabModel =>
        {
            try
            {
                CharacterViewModel.CharacterModel.WikiPages.Remove(wikiTabModel);
            }
            catch (Exception e)
            {
               App.ErrorManager.TrackException(e, CharacterViewModel.CharacterModel.Name, new Dictionary<string, string>()
               {
                   { "url", wikiTabModel.Url}
               });
            }
        }));

        private ICommand _openWikiPageCommand;
        public ICommand OpenWikiPageCommand => _openWikiPageCommand ?? (_openWikiPageCommand = new Command<string>(url =>
        {
            try
            {
                OpenWikiPage(url);
            }
            catch (Exception e)
            {
                App.ErrorManager.TrackException(e, CharacterViewModel.CharacterModel.Name, new Dictionary<string, string>()
                {
                    { "url", url}
                });
            }
        }));

        private ICommand _goBackToWikiMainpageCommand;

        public ICommand GoBackToWikiMainpageCommand => _goBackToWikiMainpageCommand ?? (_goBackToWikiMainpageCommand =
            new Command(() =>
            {
                try
                {
                    SelectedWikiTab = CharacterViewModel.CharacterModel.WikiPages[0];
                }
                catch (Exception e)
                {
                    App.ErrorManager.TrackException(e, CharacterViewModel.CharacterModel.Name);
                }
            }));

        public void OpenWikiPage(string url)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                var wikiTabModel = new WikiTabModel(url);
                CharacterViewModel.CharacterModel.WikiPages.Add(wikiTabModel);
                SelectedWikiTab = wikiTabModel;
            });
        }
    }
}