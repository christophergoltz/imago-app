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
        public CharacterViewModel CharacterViewModel { get; }
        public event EventHandler<string> OpenWikiPageRequested;
        public event EventHandler<(DiceSearchModelType type, object value)> DiceRollRequested;

        public SkillGroupViewModel Bewegung { get; set; }
        public SkillGroupViewModel Nahkampf { get; set; }
        public SkillGroupViewModel Heimlichkeit { get; set; }
        public SkillGroupViewModel Fernkampf { get; set; }
        public SkillGroupViewModel Webkunst { get; set; }
        public SkillGroupViewModel Wissenschaft { get; set; }
        public SkillGroupViewModel Handwerk { get; set; }
        public SkillGroupViewModel Soziales { get; set; }

        public int SelectedTabIndex
        {
            get => _selectedTabIndex;
            set => SetProperty(ref _selectedTabIndex, value);
        }

        private ICommand _switchTabCommand;
        public ICommand SwitchTabCommand => _switchTabCommand ?? (_switchTabCommand = new Command<int>(parameter =>
        {
            SelectedTabIndex = parameter;
        }));
        
        private int _selectedTabIndex;
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

        public SkillPageViewModel(CharacterViewModel characterViewModel, IWikiService wikiService)
        {
            var wikiService1 = wikiService;
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

            Bewegung = new SkillGroupViewModel(wikiService1, CharacterViewModel.CharacterModel.SkillGroups.First(_ => _.Type == SkillGroupModelType.Bewegung), CharacterViewModel);
            Bewegung.OpenWikiPageRequested += (sender, s) => OpenWikiPageRequested?.Invoke(sender, s);
            Bewegung.DiceRollRequested += (sender, s) => DiceRollRequested?.Invoke(sender, s);
           
            Nahkampf = new SkillGroupViewModel(wikiService1, CharacterViewModel.CharacterModel.SkillGroups.First(_ => _.Type == SkillGroupModelType.Nahkampf), CharacterViewModel);
            Nahkampf.OpenWikiPageRequested += (sender, s) => OpenWikiPageRequested?.Invoke(sender, s);
            Nahkampf.DiceRollRequested += (sender, s) => DiceRollRequested?.Invoke(sender, s);
          
            Heimlichkeit = new SkillGroupViewModel(wikiService1, CharacterViewModel.CharacterModel.SkillGroups.First(_ => _.Type == SkillGroupModelType.Heimlichkeit), CharacterViewModel);
            Heimlichkeit.OpenWikiPageRequested += (sender, s) => OpenWikiPageRequested?.Invoke(sender, s);
            Heimlichkeit.DiceRollRequested += (sender, s) => DiceRollRequested?.Invoke(sender, s);
          
            Fernkampf = new SkillGroupViewModel(wikiService1, CharacterViewModel.CharacterModel.SkillGroups.First(_ => _.Type == SkillGroupModelType.Fernkampf), CharacterViewModel);
            Fernkampf.OpenWikiPageRequested += (sender, s) => OpenWikiPageRequested?.Invoke(sender, s);
            Fernkampf.DiceRollRequested += (sender, s) => DiceRollRequested?.Invoke(sender, s);
           
            Webkunst = new SkillGroupViewModel(wikiService1, CharacterViewModel.CharacterModel.SkillGroups.First(_ => _.Type == SkillGroupModelType.Webkunst), CharacterViewModel);
            Webkunst.OpenWikiPageRequested += (sender, s) => OpenWikiPageRequested?.Invoke(sender, s);
            Webkunst.DiceRollRequested += (sender, s) => DiceRollRequested?.Invoke(sender, s);
          
            Wissenschaft = new SkillGroupViewModel(wikiService1, CharacterViewModel.CharacterModel.SkillGroups.First(_ => _.Type == SkillGroupModelType.Wissenschaft), CharacterViewModel);
            Wissenschaft.OpenWikiPageRequested += (sender, s) => OpenWikiPageRequested?.Invoke(sender, s);
            Wissenschaft.DiceRollRequested += (sender, s) => DiceRollRequested?.Invoke(sender, s);
           
            Handwerk = new SkillGroupViewModel(wikiService1, CharacterViewModel.CharacterModel.SkillGroups.First(_ => _.Type == SkillGroupModelType.Handwerk), CharacterViewModel);
            Handwerk.OpenWikiPageRequested += (sender, s) => OpenWikiPageRequested?.Invoke(sender, s);
            Handwerk.DiceRollRequested += (sender, s) => DiceRollRequested?.Invoke(sender, s);
          
            Soziales = new SkillGroupViewModel(wikiService1, CharacterViewModel.CharacterModel.SkillGroups.First(_ => _.Type == SkillGroupModelType.Soziales), CharacterViewModel);
            Soziales.OpenWikiPageRequested += (sender, s) => OpenWikiPageRequested?.Invoke(sender, s);
            Soziales.DiceRollRequested += (sender, s) => DiceRollRequested?.Invoke(sender, s);
        }
    }
}