namespace ImagoApp.ViewModels
{
    public class SkillViewModel : Util.BindableBase
    {
        private readonly Models.SkillGroupModel _skillGroup;
        private readonly CharacterViewModel _characterViewModel;
        private Models.SkillModel _skill;

        public Models.SkillModel Skill
        {
            get => _skill;
            set => SetProperty(ref _skill, value);
        }

        public SkillViewModel(Models.SkillModel skill, Models.SkillGroupModel skillGroup, CharacterViewModel characterViewModel)
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