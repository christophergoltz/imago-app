using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using ImagoApp.Application;
using ImagoApp.Application.Models;
using ImagoApp.Application.Services;
using ImagoApp.Shared.Enums;
using Xamarin.Forms;

namespace ImagoApp.ViewModels
{
    public class WeaveTalentList : ObservableCollection<WeaveTalentModel>
    {
        public ObservableCollection<WeaveTalentModel> Talents => this;
        public string WeaveSourceGroup { get; set; }
        public List<SkillModel> Skills { get; set; }

        public new event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void RaiseOnPropertyChanged(string propertyName)
        {
            OnPropertyChanged(propertyName);
        }
    }

    public class WeaveTalentPageViewModel : BindableBase
    {
        public event EventHandler<SkillModelType> OpenSkillPageRequested;

        public WeaveTalentDetailViewModel WeaveTalentDetailViewModel
        {
            get => _weaveTalentDetailViewModel;
            set => SetProperty(ref _weaveTalentDetailViewModel, value);
        }

        private readonly CharacterViewModel _characterViewModel;
        private readonly IWikiDataService _wikiDataService;
        private WeaveTalentDetailViewModel _weaveTalentDetailViewModel;
        public ObservableCollection<WeaveTalentList> WeaveTalents { get; set; }

        private ICommand _openWeaponCommand;

        public ICommand OpenWeaveTalentCommand => _openWeaponCommand ?? (_openWeaponCommand =
            new Command<WeaveTalentModel>(weaveTalent =>
            {
                try
                {
                    var weaveTalentList = WeaveTalents.First(list => list.WeaveSourceGroup == weaveTalent.WeaveSource);
                    var detailViewModel = new WeaveTalentDetailViewModel(weaveTalent, weaveTalentList.Skills);
                    detailViewModel.CloseRequested += (sender, args) =>
                    {
                        WeaveTalentDetailViewModel = null;
                    };
                    detailViewModel.OpenSkillPageRequested += (sender, type) =>
                    {
                        OpenSkillPageRequested?.Invoke(sender, type);
                    };

                    WeaveTalentDetailViewModel = detailViewModel;
                }
                catch (Exception exception)
                {
                    App.ErrorManager.TrackException(exception, _characterViewModel.CharacterModel.Name);
                }
            }));

        public WeaveTalentPageViewModel(CharacterViewModel characterViewModel, IWikiDataService wikiDataService)
        {
            _characterViewModel = characterViewModel;
            _wikiDataService = wikiDataService;
            WeaveTalents = new ObservableCollection<WeaveTalentList>();
            InitializeWeaveTalentList();
        }
        
     
        public void InitializeWeaveTalentList()
        {
            lock (WeaveTalents)
            {
                
            }
        }
    }
}