using ImagoApp.Application;
using ImagoApp.Application.Models;

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
                _characterViewModel.SetCreationExperience(Skill, _skillGroup, value);
                OnPropertyChanged(nameof(CreationExperience));
            }
        }
    }
}