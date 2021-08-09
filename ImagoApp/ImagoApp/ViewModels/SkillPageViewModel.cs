using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using ImagoApp.Application;
using ImagoApp.Application.Models;
using ImagoApp.Application.Services;
using ImagoApp.Shared.Enums;
using Xamarin.Forms;

namespace ImagoApp.ViewModels
{
    public class SkillPageViewModel : BindableBase
    {
        private readonly IWikiService _wikiService;
        private readonly IWikiDataService _wikiDataService;
        public CharacterViewModel CharacterViewModel { get; }
        private SkillGroupDetailViewModel _skillGroupDetailViewModel;
        private SkillDetailViewModel _skillDetailViewModel;
        public event EventHandler<string> OpenWikiPageRequested;
        public SkillGroupViewModel Bewegung => new SkillGroupViewModel(CharacterViewModel.CharacterModel.SkillGroups.First(_=>_.Type == SkillGroupModelType.Bewegung), CharacterViewModel);
        public SkillGroupViewModel Nahkampf => new SkillGroupViewModel(CharacterViewModel.CharacterModel.SkillGroups.First(_ => _.Type == SkillGroupModelType.Nahkampf), CharacterViewModel);
        public SkillGroupViewModel Heimlichkeit => new SkillGroupViewModel(CharacterViewModel.CharacterModel.SkillGroups.First(_ => _.Type == SkillGroupModelType.Heimlichkeit), CharacterViewModel);
        public SkillGroupViewModel Fernkampf => new SkillGroupViewModel(CharacterViewModel.CharacterModel.SkillGroups.First(_ => _.Type == SkillGroupModelType.Fernkampf), CharacterViewModel);
        public SkillGroupViewModel Webkunst => new SkillGroupViewModel(CharacterViewModel.CharacterModel.SkillGroups.First(_ => _.Type == SkillGroupModelType.Webkunst), CharacterViewModel);
        public SkillGroupViewModel Wissenschaft => new SkillGroupViewModel(CharacterViewModel.CharacterModel.SkillGroups.First(_ => _.Type == SkillGroupModelType.Wissenschaft), CharacterViewModel);
        public SkillGroupViewModel Handwerk => new SkillGroupViewModel(CharacterViewModel.CharacterModel.SkillGroups.First(_ => _.Type == SkillGroupModelType.Handwerk), CharacterViewModel);
        public SkillGroupViewModel Soziales => new SkillGroupViewModel(CharacterViewModel.CharacterModel.SkillGroups.First(_ => _.Type == SkillGroupModelType.Soziales), CharacterViewModel);

        private ICommand _openSkillDetailCommandCommand;
        public ICommand OpenSkillDetailCommand => _openSkillDetailCommandCommand ?? (_openSkillDetailCommandCommand = new Command<(SkillModel Skill, SkillGroupModel SkillGroup)>(parameter =>
        {
            try
            {
                var vm = new SkillDetailViewModel(parameter.Skill, parameter.SkillGroup, CharacterViewModel, _wikiService, _wikiDataService);
                vm.CloseRequested += (sender, args) => { SkillDetailViewModel = null; };
                vm.OpenWikiPageRequested += (sender, s) => { OpenWikiPageRequested?.Invoke(this, s); };

                SkillDetailViewModel = vm;
            }
            catch (Exception exception)
            {
                App.ErrorManager.TrackException(exception, CharacterViewModel.CharacterModel.Name, new Dictionary<string, string>()
                {
                    { "Skill", parameter.Skill.Type.ToString()},
                    { "SkillGroup", parameter.SkillGroup.Type.ToString()}
                });
            }
        }));

        private ICommand _openSkillGroupDetailCommand;

        public ICommand OpenSkillGroupDetailCommand => _openSkillGroupDetailCommand ?? (_openSkillGroupDetailCommand =
            new Command<SkillGroupModel>(group =>
            {
                try
                {
                    var vm = new SkillGroupDetailViewModel(group, CharacterViewModel, _wikiService);
                    vm.CloseRequested += (sender, args) => { SkillGroupDetailViewModel = null; };
                    vm.OpenWikiPageRequested += (sender, s) => { OpenWikiPageRequested?.Invoke(this, s); };
                    SkillGroupDetailViewModel = vm;
                }
                catch (Exception exception)
                {
                    App.ErrorManager.TrackException(exception, CharacterViewModel.CharacterModel.Name,
                        new Dictionary<string, string>()
                        {
                            {"SkillGroup", group.Type.ToString()}
                        });
                }
            }));
        
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
        
        public int TotalSkillExperience
        {
            get => CharacterViewModel.CharacterModel.CharacterCreationSkillPoints;
            set
            {
                CharacterViewModel.CharacterModel.CharacterCreationSkillPoints = value;
                OnPropertyChanged(nameof(SkillExperienceBalance));
            }
        }

        public int SkillExperienceBalance => TotalSkillExperience - CharacterViewModel.CharacterModel.SkillGroups
                                                 .SelectMany(model => model.Skills)
                                                 .Sum(model => model.CreationExperience);
        
        public SkillPageViewModel(CharacterViewModel characterViewModel, IWikiService wikiService, IWikiDataService wikiDataService)
        {
            _wikiService = wikiService;
            _wikiDataService = wikiDataService;
            CharacterViewModel = characterViewModel;

            foreach (var skill in characterViewModel.CharacterModel.SkillGroups.SelectMany(model => model.Skills))
            {
                skill.PropertyChanged += (sender, args) =>
                {
                    if (args.PropertyName.Equals(nameof(SkillModel.CreationExperience)))
                    {
                        OnPropertyChanged(nameof(SkillExperienceBalance));
                    }
                };
            }
        }
    }
}