using System.Collections.Generic;
using System.Linq;

namespace ImagoApp.ViewModels
{
    public class SkillGroupViewModel : Util.BindableBase
    {
        private Models.SkillGroupModel _skillGroup;
        private List<SkillViewModel> _skills;

        public Models.SkillGroupModel SkillGroup
        {
            get => _skillGroup;
            private set => SetProperty(ref _skillGroup ,value);
        }

        public CharacterViewModel CharacterViewModel { get; }

        public List<SkillViewModel> Skills
        {
            get => _skills;
            private set => SetProperty(ref _skills , value);
        }

        public SkillGroupViewModel(Models.SkillGroupModel skillGroup, CharacterViewModel characterViewModel)
        {
            SkillGroup = skillGroup;
            CharacterViewModel = characterViewModel;
            Skills = skillGroup.Skills.Select(model => new SkillViewModel(model, skillGroup, CharacterViewModel)).ToList();
        }
    }
}
