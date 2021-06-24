using System.Collections.Generic;
using System.Text;
using Imago.Models;
using Imago.Services;
using Imago.Util;

namespace Imago.ViewModels
{
    public class AttributeExperienceViewModel : BindableBase
    {
        private readonly CharacterViewModel _characterViewModel;
        public Attribute Attribute { get; private set; }

        public AttributeExperienceViewModel(Attribute attribute, CharacterViewModel characterViewModel)
        {
            _characterViewModel = characterViewModel;
            Attribute = attribute;
        }

        public int TotalExperienceValue
        {
            get => Attribute.TotalExperience;
            set
            {
                _characterViewModel.SetExperienceToAttribute(Attribute, value);
                OnPropertyChanged(nameof(TotalExperienceValue));
                OnPropertyChanged(nameof(IncreaseValue));
            }
        }

        public int IncreaseValue
        {
            get => Attribute.IncreaseValue;
            set
            {
                var experienceRequired = IncreaseServices.GetExperienceRequiredForLevel(IncreaseType.Attribute, value);
                TotalExperienceValue = experienceRequired;
            }
        }
    }
}
