using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imago.Models;
using Imago.Models.Enum;
using Imago.Repository;
using Imago.Util;

namespace Imago.ViewModels
{
    public class CharacterCreationViewModel : BindableBase
    {
        public CharacterViewModel CharacterViewModel { get; private set; }
        
        public List<SkillExperienceViewModel> SkillExperienceViewModelBewegung { get; set; }
        public SkillGroupModel Bewegung => CharacterViewModel.Character.SkillGroups[SkillGroupModelType.Bewegung];

        public List<SkillExperienceViewModel> SkillExperienceViewModelNahkampf { get; set; }
        public SkillGroupModel Nahkampf => CharacterViewModel.Character.SkillGroups[SkillGroupModelType.Nahkampf];

        public List<SkillExperienceViewModel> SkillExperienceViewModelHeimlichkeit { get; set; }
        public SkillGroupModel Heimlichkeit => CharacterViewModel.Character.SkillGroups[SkillGroupModelType.Heimlichkeit];

        public List<SkillExperienceViewModel> SkillExperienceViewModelFernkampf { get; set; }
        public SkillGroupModel Fernkampf => CharacterViewModel.Character.SkillGroups[SkillGroupModelType.Fernkampf];

        public List<SkillExperienceViewModel> SkillExperienceViewModelWebkunst { get; set; }
        public SkillGroupModel Webkunst => CharacterViewModel.Character.SkillGroups[SkillGroupModelType.Webkunst];

        public List<SkillExperienceViewModel> SkillExperienceViewModelWissenschaft { get; set; }
        public SkillGroupModel Wissenschaft => CharacterViewModel.Character.SkillGroups[SkillGroupModelType.Wissenschaft];

        public List<SkillExperienceViewModel> SkillExperienceViewModelHandwerk { get; set; }
        public SkillGroupModel Handwerk => CharacterViewModel.Character.SkillGroups[SkillGroupModelType.Handwerk];

        public List<SkillExperienceViewModel> SkillExperienceViewModelSoziales { get; set; }
        public SkillGroupModel Soziales => CharacterViewModel.Character.SkillGroups[SkillGroupModelType.Soziales];
        
        public CharacterCreationViewModel(ICharacterRepository characterRepository, IRuleRepository ruleRepository)
        {
            var character = characterRepository.CreateNewCharacter();
            var characterViewModel = new CharacterViewModel(character, ruleRepository);
            
            CharacterViewModel = characterViewModel;
            SkillExperienceViewModelBewegung = Bewegung.Skills.Select(skill => new SkillExperienceViewModel(skill, Bewegung, characterViewModel)).ToList();
            SkillExperienceViewModelFernkampf = Fernkampf.Skills.Select(skill => new SkillExperienceViewModel(skill, Fernkampf, characterViewModel)).ToList();
            SkillExperienceViewModelHandwerk = Handwerk.Skills.Select(skill => new SkillExperienceViewModel(skill, Handwerk, characterViewModel)).ToList();
            SkillExperienceViewModelHeimlichkeit = Heimlichkeit.Skills.Select(skill => new SkillExperienceViewModel(skill, Heimlichkeit, characterViewModel)).ToList();
            SkillExperienceViewModelNahkampf = Nahkampf.Skills.Select(skill => new SkillExperienceViewModel(skill, Nahkampf, characterViewModel)).ToList();
            SkillExperienceViewModelWebkunst = Webkunst.Skills.Select(skill => new SkillExperienceViewModel(skill, Webkunst, characterViewModel)).ToList();
            SkillExperienceViewModelSoziales = Soziales.Skills.Select(skill => new SkillExperienceViewModel(skill, Soziales, characterViewModel)).ToList();
            SkillExperienceViewModelWissenschaft = Wissenschaft.Skills.Select(skill => new SkillExperienceViewModel(skill, Wissenschaft, characterViewModel)).ToList();
        }
    }
}
