using System;
using System.Linq;
using System.Windows.Input;
using ImagoApp.Application.Models;
using ImagoApp.Application.Services;
using ImagoApp.Services;
using ImagoApp.Shared.Enums;
using ImagoApp.Util;
using Xamarin.Forms;

namespace ImagoApp.ViewModels
{
    public class SkillPageViewModel : BindableBase
    {
        private readonly IWikiDataService _wikiDataService;
        public CharacterViewModel CharacterViewModel { get; }
        private SkillGroupDetailViewModel _skillGroupDetailViewModel;
        private SkillDetailViewModel _skillDetailViewModel;
        public event EventHandler<string> OpenWikiPageRequested;
        public SkillGroupViewModel Bewegung => new SkillGroupViewModel(CharacterViewModel.Character.SkillGroups.First(_=>_.Type == SkillGroupModelType.Bewegung), CharacterViewModel);
        public SkillGroupViewModel Nahkampf => new SkillGroupViewModel(CharacterViewModel.Character.SkillGroups.First(_ => _.Type == SkillGroupModelType.Nahkampf), CharacterViewModel);
        public SkillGroupViewModel Heimlichkeit => new SkillGroupViewModel(CharacterViewModel.Character.SkillGroups.First(_ => _.Type == SkillGroupModelType.Heimlichkeit), CharacterViewModel);
        public SkillGroupViewModel Fernkampf => new SkillGroupViewModel(CharacterViewModel.Character.SkillGroups.First(_ => _.Type == SkillGroupModelType.Fernkampf), CharacterViewModel);
        public SkillGroupViewModel Webkunst => new SkillGroupViewModel(CharacterViewModel.Character.SkillGroups.First(_ => _.Type == SkillGroupModelType.Webkunst), CharacterViewModel);
        public SkillGroupViewModel Wissenschaft => new SkillGroupViewModel(CharacterViewModel.Character.SkillGroups.First(_ => _.Type == SkillGroupModelType.Wissenschaft), CharacterViewModel);
        public SkillGroupViewModel Handwerk => new SkillGroupViewModel(CharacterViewModel.Character.SkillGroups.First(_ => _.Type == SkillGroupModelType.Handwerk), CharacterViewModel);
        public SkillGroupViewModel Soziales => new SkillGroupViewModel(CharacterViewModel.Character.SkillGroups.First(_ => _.Type == SkillGroupModelType.Soziales), CharacterViewModel);

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
        public int SkillExperienceBalance => TotalSkillExperience - CharacterViewModel.Character.SkillGroups.SelectMany(model => model.Skills).Sum(model => model.TotalExperience);
        
        public SkillPageViewModel(CharacterViewModel characterViewModel, IWikiService wikiService, IWikiDataService wikiDataService, IRuleService ruleService)
        {
            _wikiDataService = wikiDataService;
            CharacterViewModel = characterViewModel;
            TotalSkillExperience = 1350;

            foreach (var skill in characterViewModel.Character.SkillGroups.SelectMany(model => model.Skills))
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
                var vm = new SkillDetailViewModel(parameter.Skill, parameter.SkillGroup, characterViewModel, wikiService, wikiDataService, ruleService);
                vm.CloseRequested += (sender, args) => { SkillDetailViewModel = null; };
                vm.OpenWikiPageRequested += (sender, s) => { OpenWikiPageRequested?.Invoke(this, s); };

                SkillDetailViewModel = vm;
            });

            OpenSkillGroupDetailCommand = new Command<SkillGroupModel>(group =>
            {
                var vm = new SkillGroupDetailViewModel(group, characterViewModel, wikiService);
                vm.CloseRequested += (sender, args) => { SkillGroupDetailViewModel = null; };
                vm.OpenWikiPageRequested += (sender, s) => { OpenWikiPageRequested?.Invoke(this, s); };
                SkillGroupDetailViewModel = vm;
            });
        }
    }
}