using System;
using System.Collections.Generic;
using System.Windows.Input;
using ImagoApp.Application;
using ImagoApp.Application.Models;
using Xamarin.Forms;

namespace ImagoApp.ViewModels
{
    public class SkillViewModel : BindableBase
    {
        private readonly SkillGroupModel _skillGroup;
        private readonly CharacterViewModel _characterViewModel;
        private SkillModel _skill;

        public SkillModel Skill
        {
            get => _skill;
            set => SetProperty(ref _skill, value);
        }

        public SkillViewModel(SkillModel skill, SkillGroupModel skillGroup, CharacterViewModel characterViewModel)
        {
            _skillGroup = skillGroup;
            _characterViewModel = characterViewModel;
            Skill = skill;
        }

        public int CreationExperience
        {
            get => Skill.CreationExperience;
            set
            {
                if (value != Skill.CreationExperience)
                {
                    _characterViewModel.SetCreationExperience(Skill, _skillGroup, value);
                    OnPropertyChanged(nameof(CreationExperience));
                }
            }
        }

        public int ModificationValue
        {
            get => Skill?.ModificationValue ?? 0;
            set
            {
                _characterViewModel.SetModification(Skill, value);
                OnPropertyChanged(nameof(ModificationValue));
            }
        }

        private ICommand _increaseExperienceCommand;

        public ICommand IncreaseExperienceCommand => _increaseExperienceCommand ?? (_increaseExperienceCommand = new Command<int>(experienceValue =>
        {
            try
            {
                _characterViewModel.AddExperienceToSkill(Skill, _skillGroup, experienceValue);
            }
            catch (Exception exception)
            {
                App.ErrorManager.TrackException(exception, _characterViewModel.CharacterModel.Name, new Dictionary<string, string>()
                {
                    { "Experience Value", experienceValue.ToString()}
                });
            }
        }));

        private ICommand _decreaseExperienceCommand;
        public ICommand DecreaseExperienceCommand => _decreaseExperienceCommand ?? (_decreaseExperienceCommand = new Command(() =>
        {
            try
            {
                //todo -1 by parameter; 
                _characterViewModel.AddExperienceToSkill(Skill, _skillGroup, -1);
            }
            catch (Exception exception)
            {
                App.ErrorManager.TrackException(exception, _characterViewModel.CharacterModel.Name);
            }
        }));
    }
}