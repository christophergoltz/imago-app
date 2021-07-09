using ImagoApp.Application.Models;

namespace ImagoApp.ViewModels
{
    public class SkillViewModel : Util.BindableBase
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

        public int TotalExperienceValue
        {
            get => Skill.TotalExperience;
            set
            {
                _characterViewModel.SetExperienceToSkill(Skill, _skillGroup, value);
                OnPropertyChanged(nameof(TotalExperienceValue));
            }
        }
    }
}