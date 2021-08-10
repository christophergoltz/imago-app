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
                    var detailViewModel = new WeaveTalentDetailViewModel(weaveTalent, weaveTalentList.Skills,
                        _characterViewModel, _wikiDataService);
                    detailViewModel.CloseRequested += (sender, args) => { WeaveTalentDetailViewModel = null; };

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
        
        private List<SkillModel> GetSkillsFromRequirements(List<SkillRequirementModel> requirements)
        {
            var result = new List<SkillModel>();
            foreach (var requirementModel in requirements)
            {
                if (requirementModel.Type == SkillModelType.Philosophie)
                    continue;

                var item = _characterViewModel.CharacterModel.SkillGroups.SelectMany(model => model.Skills)
                    .First(model => model.Type == requirementModel.Type);
                result.Add(item);
            }

            return result;
        }
        
        public void InitializeWeaveTalentList()
        {
            lock (WeaveTalents)
            {
                var allWeaveTalents = _wikiDataService.GetAllWeaveTalents();
                foreach (var weaveTalent in allWeaveTalents)
                {
                    var available = _characterViewModel.CheckTalentRequirement(weaveTalent.Requirements);
                    if (available)
                    {
                        //add
                        var list = WeaveTalents.FirstOrDefault(talentList =>
                            talentList.WeaveSourceGroup == weaveTalent.WeaveSource);

                        if (list == null)
                        {
                            //add new
                            var talentList = new WeaveTalentList
                            {
                                WeaveSourceGroup = weaveTalent.WeaveSource,
                                Skills = GetSkillsFromRequirements(weaveTalent.Requirements)
                            };
                            talentList.Talents.Add(weaveTalent);

                            WeaveTalents.Add(talentList);
                        }
                        else
                        {
                            //extend existing, if required
                            if (list.All(model => model.Name != weaveTalent.Name))
                            {
                                list.Add(weaveTalent);
                            }
                        }
                    }
                    else
                    {
                        //remove
                        var weaveTalentList = WeaveTalents.FirstOrDefault(list => list.WeaveSourceGroup == weaveTalent.WeaveSource);
                        if (weaveTalentList != null)
                        {
                            var elementToRemove = weaveTalentList.FirstOrDefault(model => model.Name == weaveTalent.Name);
                            if (elementToRemove != null)
                            {
                                weaveTalentList.Remove(elementToRemove);
                            }

                            if (weaveTalentList.Talents.Count == 0)
                            {
                                WeaveTalents.Remove(weaveTalentList);
                            }
                        }
                    }

                    OnPropertyChanged(nameof(WeaveTalents));
                    foreach (var group in WeaveTalents)
                    {
                        group.RaiseOnPropertyChanged(nameof(WeaveTalentList.Talents));
                    }
                }
            }
        }
    }
}