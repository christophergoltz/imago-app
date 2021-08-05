using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public SkillModel Skill { get; set; }
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

        public WeaveTalentPageViewModel(CharacterViewModel characterViewModel, IWikiDataService wikiDataService)
        {
            _characterViewModel = characterViewModel;
            _wikiDataService = wikiDataService;
            WeaveTalents = new ObservableCollection<WeaveTalentList>();

            InitializeWeaveTalentList().Wait();
        }

        public void CreateDetailView(WeaveTalentModel weaveTalent)
        {
            var weaveTalentList = WeaveTalents.First(list => list.Skill.Type == weaveTalent.TargetSkillModel);
            WeaveTalentDetailViewModel = new WeaveTalentDetailViewModel(weaveTalent, weaveTalentList.Skill);
        }

        public async Task InitializeWeaveTalentList()
        {
            var allWeaveTalents = _wikiDataService.GetAllWeaveTalents();
            foreach (var weaveTalent in allWeaveTalents)
            {
                var available = _characterViewModel.CheckTalentRequirement(weaveTalent.Requirements);
                if (available)
                {
                    //add
                    if (WeaveTalents.Select(list => list.Skill.Type).Contains(weaveTalent.TargetSkillModel))
                    {
                        var weaveTalentList = WeaveTalents.First(list => list.Skill.Type == weaveTalent.TargetSkillModel);
                        await Device.InvokeOnMainThreadAsync(() =>
                        {
                            weaveTalentList.Add(weaveTalent);
                        });
                    }
                    else
                    {
                        var talentList = new WeaveTalentList
                        {
                            Skill = _characterViewModel.CharacterModel.SkillGroups
                                .SelectMany(model => model.Skills)
                                .First(model => model.Type == weaveTalent.TargetSkillModel)
                        };
                        talentList.Talents.Add(weaveTalent);

                        await Device.InvokeOnMainThreadAsync(() =>
                        {
                            WeaveTalents.Add(talentList);
                        });
                    }
                }
                else
                {
                    //remove
                    var weaveTalentList = WeaveTalents.FirstOrDefault(list => list.Talents.Contains(weaveTalent));
                    if (weaveTalentList != null)
                    {
                        await Device.InvokeOnMainThreadAsync(() =>
                        {
                            weaveTalentList.Remove(weaveTalent);
                        });

                        if (weaveTalentList.Talents.Count == 0)
                        {
                            await Device.InvokeOnMainThreadAsync(() =>
                            {
                                WeaveTalents.Remove(weaveTalentList);
                            });
                        }
                    }
                }
            }
        }
    }
}
