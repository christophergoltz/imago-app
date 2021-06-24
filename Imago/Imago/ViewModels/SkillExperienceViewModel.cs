using System.Collections.Generic;
using System.Text;
using Imago.Models;
using Imago.Services;
using Imago.Util;

namespace Imago.ViewModels
{
    public class SkillExperienceViewModel : BindableBase
    {
        private readonly SkillGroup _skillGroup;
        private readonly CharacterViewModel _characterViewModel;
       
        public Skill Skill { get; private set; }

        public SkillExperienceViewModel(Skill skill, SkillGroup skillGroup,  CharacterViewModel characterViewModel)
        {
            _skillGroup = skillGroup;
            _characterViewModel = characterViewModel;
            Skill = skill;
        }

        public int TotalExperienceValue
        {
            get => Skill.TotalExperience;
            set
            {
                _characterViewModel.SetExperienceToSkill(Skill, _skillGroup, value);
                OnPropertyChanged(nameof(TotalExperienceValue));
                OnPropertyChanged(nameof(IncreaseValue));
            }
        }

        public int IncreaseValue
        {
            get => Skill.IncreaseValue;
            set
            {
                var experienceRequired = IncreaseServices.GetExperienceRequiredForLevel(IncreaseType.Skill, value);
                TotalExperienceValue = experienceRequired;
            }
        }
    }
}
