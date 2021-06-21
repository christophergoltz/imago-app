using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imago.Models;
using Imago.Repository;
using Imago.Util;

namespace Imago.ViewModels
{
    public class CharacterCreationViewModel : BindableBase
    {
        private int _totalAttributeExperience;
        public CharacterViewModel CharacterViewModel { get; private set; }

        public List<AttributeExperienceViewModel> AttributeExperienceViewModel { get; set; }

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
        }
    }
}
