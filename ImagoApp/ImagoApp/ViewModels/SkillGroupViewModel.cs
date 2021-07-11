using System.Collections.Generic;
using System.Linq;
using ImagoApp.Application;
using ImagoApp.Application.Models;

namespace ImagoApp.ViewModels
{
    public class SkillGroupViewModel : BindableBase
    {
        private SkillGroupModel _skillGroup;
        private List<SkillViewModel> _skills;

        public SkillGroupModel SkillGroup
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

        public SkillGroupViewModel(SkillGroupModel skillGroup, CharacterViewModel characterViewModel)
        {
            SkillGroup = skillGroup;
            CharacterViewModel = characterViewModel;
            Skills = skillGroup.Skills.Select(model => new SkillViewModel(model, skillGroup, CharacterViewModel)).ToList();
        }
    }
}
