using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Imago.Converter;
using Imago.Models;
using Imago.Models.Base;
using Imago.Models.Enum;
using Imago.Repository;
using Imago.Repository.WrappingDatabase;
using Imago.Services;
using Imago.Util;
using Imago.Views;
using Xamarin.Forms;

namespace Imago.ViewModels
{
    public class SkillPageViewModel : BindableBase
    {
        public CharacterViewModel CharacterViewModel { get; }
        private SkillGroupDetailViewModel _skillGroupDetailViewModel;
        private SkillDetailViewModel _skillDetailViewModel;
        
        public SkillGroupViewModel Bewegung => new SkillGroupViewModel(CharacterViewModel.Character.SkillGroups[SkillGroupModelType.Bewegung], CharacterViewModel);
        public SkillGroupViewModel Nahkampf => new SkillGroupViewModel(CharacterViewModel.Character.SkillGroups[SkillGroupModelType.Nahkampf], CharacterViewModel);
        public SkillGroupViewModel Heimlichkeit => new SkillGroupViewModel(CharacterViewModel.Character.SkillGroups[SkillGroupModelType.Heimlichkeit], CharacterViewModel);
        public SkillGroupViewModel Fernkampf => new SkillGroupViewModel(CharacterViewModel.Character.SkillGroups[SkillGroupModelType.Fernkampf], CharacterViewModel);
        public SkillGroupViewModel Webkunst => new SkillGroupViewModel(CharacterViewModel.Character.SkillGroups[SkillGroupModelType.Webkunst], CharacterViewModel);
        public SkillGroupViewModel Wissenschaft => new SkillGroupViewModel(CharacterViewModel.Character.SkillGroups[SkillGroupModelType.Wissenschaft], CharacterViewModel);
        public SkillGroupViewModel Handwerk => new SkillGroupViewModel(CharacterViewModel.Character.SkillGroups[SkillGroupModelType.Handwerk], CharacterViewModel);
        public SkillGroupViewModel Soziales => new SkillGroupViewModel(CharacterViewModel.Character.SkillGroups[SkillGroupModelType.Soziales], CharacterViewModel);

        public ICommand OpenSkillDetailCommand { get; set; }
        public ICommand OpenSkillGroupDetailCommand { get; set; }

        public SkillDetailViewModel SkillDetailViewModel
        {
            get => _skillDetailViewModel;
            set => SetProperty(ref _skillDetailViewModel, value);
        }

        public SkillGroupDetailViewModel SkillGroupDetailViewModel
        {
            get => _skillGroupDetailViewModel;
            set => SetProperty(ref _skillGroupDetailViewModel, value);
        }

        private int _totalSkillExperience;
        public int TotalSkillExperience
        {
            get => _totalSkillExperience;
            set
            {
                SetProperty(ref _totalSkillExperience, value);
                OnPropertyChanged(nameof(SkillExperienceBalance));
            }
        }
        public int SkillExperienceBalance => TotalSkillExperience - CharacterViewModel.Character.SkillGroups.Values.SelectMany(model => model.Skills).Sum(model => model.TotalExperience);
        
        public SkillPageViewModel(CharacterViewModel characterViewModel, IWikiService wikiService, 
            IMasteryRepository masteryRepository,
            ITalentRepository talentRepository,
            IRuleRepository ruleRepository)
        {
            CharacterViewModel = characterViewModel;
            TotalSkillExperience = 1350;

            foreach (var skill in characterViewModel.Character.SkillGroups.Values.SelectMany(model => model.Skills))
            {
                skill.PropertyChanged += (sender, args) =>
                {
                    if (args.PropertyName.Equals(nameof(SkillModel.TotalExperience)))
                    {
                        OnPropertyChanged(nameof(SkillExperienceBalance));
                    }
                };
            }

            OpenSkillDetailCommand = new Command<(SkillModel Skill, SkillGroupModel SkillGroup)>(parameter =>
            {
                var vm = new SkillDetailViewModel(parameter.Skill, parameter.SkillGroup, characterViewModel,
                    wikiService, masteryRepository, talentRepository, ruleRepository);
                vm.CloseRequested += (sender, args) => { SkillDetailViewModel = null; };

                SkillDetailViewModel = vm;
            });

            OpenSkillGroupDetailCommand = new Command<SkillGroupModel>(group =>
            {
                var vm = new SkillGroupDetailViewModel(group, characterViewModel, wikiService);
                vm.CloseRequested += (sender, args) => { SkillGroupDetailViewModel = null; };

                SkillGroupDetailViewModel = vm;
            });
        }
    }
}