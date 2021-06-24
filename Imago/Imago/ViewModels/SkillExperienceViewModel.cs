using System.Collections.Generic;
using System.Text;
using Imago.Models;
using Imago.Services;
using Imago.Util;

namespace Imago.ViewModels
{
    public class SkillExperienceViewModel : BindableBase
    {
        private readonly SkillGroupModel _skillGroupModel;
        private readonly CharacterViewModel _characterViewModel;
       
        public SkillModel SkillModel { get; private set; }

        public SkillExperienceViewModel(SkillModel skillModel, SkillGroupModel skillGroupModel,  CharacterViewModel characterViewModel)
        {
            _skillGroupModel = skillGroupModel;
            _characterViewModel = characterViewModel;
            SkillModel = skillModel;
        }

        public int TotalExperienceValue
        {
            get => SkillModel.TotalExperience;
            set
            {
                _characterViewModel.SetExperienceToSkill(SkillModel, _skillGroupModel, value);
                OnPropertyChanged(nameof(TotalExperienceValue));
                OnPropertyChanged(nameof(IncreaseValue));
            }
        }

        public int IncreaseValue
        {
            get => SkillModel.IncreaseValue;
            set
            {
                var experienceRequired = IncreaseServices.GetExperienceRequiredForLevel(IncreaseType.Skill, value);
                TotalExperienceValue = experienceRequired;
            }
        }
    }
}
