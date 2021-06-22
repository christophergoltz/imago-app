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
        private int _totalAttributeExperience;
        public CharacterViewModel CharacterViewModel { get; private set; }

        public List<AttributeExperienceViewModel> AttributeExperienceViewModel { get; set; }
        
        public List<SkillExperienceViewModel> SkillExperienceViewModelBewegung { get; set; }
        public SkillGroup Bewegung => CharacterViewModel.Character.SkillGroups[SkillGroupType.Bewegung];

        public List<SkillExperienceViewModel> SkillExperienceViewModelNahkampf { get; set; }
        public SkillGroup Nahkampf => CharacterViewModel.Character.SkillGroups[SkillGroupType.Nahkampf];

        public List<SkillExperienceViewModel> SkillExperienceViewModelHeimlichkeit { get; set; }
        public SkillGroup Heimlichkeit => CharacterViewModel.Character.SkillGroups[SkillGroupType.Heimlichkeit];

        public List<SkillExperienceViewModel> SkillExperienceViewModelFernkampf { get; set; }
        public SkillGroup Fernkampf => CharacterViewModel.Character.SkillGroups[SkillGroupType.Fernkampf];

        public List<SkillExperienceViewModel> SkillExperienceViewModelWebkunst { get; set; }
        public SkillGroup Webkunst => CharacterViewModel.Character.SkillGroups[SkillGroupType.Webkunst];

        public List<SkillExperienceViewModel> SkillExperienceViewModelWissenschaft { get; set; }
        public SkillGroup Wissenschaft => CharacterViewModel.Character.SkillGroups[SkillGroupType.Wissenschaft];

        public List<SkillExperienceViewModel> SkillExperienceViewModelHandwerk { get; set; }
        public SkillGroup Handwerk => CharacterViewModel.Character.SkillGroups[SkillGroupType.Handwerk];

        public List<SkillExperienceViewModel> SkillExperienceViewModelSoziales { get; set; }
        public SkillGroup Soziales => CharacterViewModel.Character.SkillGroups[SkillGroupType.Soziales];

        public int TotalAttributeExperience
        {
            get => _totalAttributeExperience;
            set
            {
                SetProperty(ref _totalAttributeExperience, value);
                OnPropertyChanged(nameof(AttributeExperienceBalance));
            }
        }

        public int AttributeExperienceBalance => TotalAttributeExperience - AttributeExperienceViewModel?.Sum(model => model.TotalExperienceValue) ?? 0;

        public CharacterCreationViewModel(ICharacterRepository characterRepository, IRuleRepository ruleRepository)
        {
            var character = characterRepository.CreateNewCharacter();
            var characterViewModel = new CharacterViewModel(character, ruleRepository);

            TotalAttributeExperience = 940;

            CharacterViewModel = characterViewModel;
            AttributeExperienceViewModel = characterViewModel.Character.Attributes.Select(_ => new AttributeExperienceViewModel(_, characterViewModel)).ToList();
            foreach (var vm in AttributeExperienceViewModel)
            {
                vm.PropertyChanged += (sender, args) =>
                {
                    if (args.PropertyName.Equals(nameof(ViewModels.AttributeExperienceViewModel.TotalExperienceValue)))
                    {
                        OnPropertyChanged(nameof(AttributeExperienceBalance));
                    }
                };
            }
            OnPropertyChanged(nameof(AttributeExperienceBalance));

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
