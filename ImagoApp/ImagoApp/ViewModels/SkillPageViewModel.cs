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

        public SkillGroupViewModel Bewegung { get; set; }
        public SkillGroupViewModel Nahkampf { get; set; }
        public SkillGroupViewModel Heimlichkeit { get; set; }
        public SkillGroupViewModel Fernkampf { get; set; }
        public SkillGroupViewModel Webkunst { get; set; }
        public SkillGroupViewModel Wissenschaft { get; set; }
        public SkillGroupViewModel Handwerk { get; set; }
        public SkillGroupViewModel Soziales { get; set; }

        public void OpenSkill(SkillModelType skillModelType)
        {
            SkillModel mSkill = null;
            SkillGroupModel mSkillGroup = null;

            var found = false;

            foreach (var skillGroup in CharacterViewModel.CharacterModel.SkillGroups)
            {
                foreach (var skill in skillGroup.Skills)
                {
                    if (skill.Type == skillModelType)
                    {
                        found = true;
                        mSkill = skill;
                        break;
                    }
                }

                if (found)
                {
                    mSkillGroup = skillGroup;
                    break;
                }

            }

            Device.BeginInvokeOnMainThread(() =>
            {
                OpenSkillDetailCommand?.Execute((mSkill, mSkillGroup));
            });
        }

        private ICommand _openSkillDetailCommandCommand;
        public ICommand OpenSkillDetailCommand => _openSkillDetailCommandCommand ?? (_openSkillDetailCommandCommand = new Command<(SkillModel Skill, SkillGroupModel SkillGroup)>(parameter =>
        {
            try
            {
                var vm = new SkillDetailViewModel(parameter.Skill, parameter.SkillGroup, CharacterViewModel, _wikiService, _wikiDataService);
                vm.CloseRequested += (sender, args) => { SkillDetailViewModel = null; };
                vm.OpenWikiPageRequested += (sender, s) => { OpenWikiPageRequested?.Invoke(this, s); };

                //close possible old dialog
                if (SkillGroupDetailViewModel != null)
                    SkillGroupDetailViewModel = null;

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

            Bewegung = new SkillGroupViewModel(_wikiService, CharacterViewModel.CharacterModel.SkillGroups.First(_ => _.Type == SkillGroupModelType.Bewegung), CharacterViewModel);
            Bewegung.OpenWikiPageRequested += (sender, s) => OpenWikiPageRequested?.Invoke(sender, s);
            Nahkampf = new SkillGroupViewModel(_wikiService, CharacterViewModel.CharacterModel.SkillGroups.First(_ => _.Type == SkillGroupModelType.Nahkampf), CharacterViewModel);
            Nahkampf.OpenWikiPageRequested += (sender, s) => OpenWikiPageRequested?.Invoke(sender, s);
            Heimlichkeit = new SkillGroupViewModel(_wikiService, CharacterViewModel.CharacterModel.SkillGroups.First(_ => _.Type == SkillGroupModelType.Heimlichkeit), CharacterViewModel);
            Heimlichkeit.OpenWikiPageRequested += (sender, s) => OpenWikiPageRequested?.Invoke(sender, s);
            Fernkampf = new SkillGroupViewModel(_wikiService, CharacterViewModel.CharacterModel.SkillGroups.First(_ => _.Type == SkillGroupModelType.Fernkampf), CharacterViewModel);
            Fernkampf.OpenWikiPageRequested += (sender, s) => OpenWikiPageRequested?.Invoke(sender, s);
            Webkunst = new SkillGroupViewModel(_wikiService, CharacterViewModel.CharacterModel.SkillGroups.First(_ => _.Type == SkillGroupModelType.Webkunst), CharacterViewModel);
            Webkunst.OpenWikiPageRequested += (sender, s) => OpenWikiPageRequested?.Invoke(sender, s);
            Wissenschaft = new SkillGroupViewModel(_wikiService, CharacterViewModel.CharacterModel.SkillGroups.First(_ => _.Type == SkillGroupModelType.Wissenschaft), CharacterViewModel);
            Wissenschaft.OpenWikiPageRequested += (sender, s) => OpenWikiPageRequested?.Invoke(sender, s);
            Handwerk = new SkillGroupViewModel(_wikiService, CharacterViewModel.CharacterModel.SkillGroups.First(_ => _.Type == SkillGroupModelType.Handwerk), CharacterViewModel);
            Handwerk.OpenWikiPageRequested += (sender, s) => OpenWikiPageRequested?.Invoke(sender, s);
            Soziales = new SkillGroupViewModel(_wikiService, CharacterViewModel.CharacterModel.SkillGroups.First(_ => _.Type == SkillGroupModelType.Soziales), CharacterViewModel);
            Soziales.OpenWikiPageRequested += (sender, s) => OpenWikiPageRequested?.Invoke(sender, s);
        }
    }
}