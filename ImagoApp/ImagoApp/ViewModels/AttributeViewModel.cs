using ImagoApp.Application.Models;

namespace ImagoApp.ViewModels
{
    public class AttributeViewModel : Util.BindableBase
    {
        public AttributeViewModel(Attribute attribute, CharacterViewModel characterViewModel)
        {
            Attribute = attribute;
            CharacterViewModel = characterViewModel;
        }

        public Attribute Attribute { get; set; }
        public CharacterViewModel CharacterViewModel { get; }

        public int Corrosion
        {
            get => Attribute.Corrosion;
            set
            {
                CharacterViewModel.SetCorrosionValue(Attribute, value);
                OnPropertyChanged(nameof(Corrosion));
            }
        }

        public int Modification
        {
            get => Attribute.ModificationValue;
            set
            {
                CharacterViewModel.SetModificationValue(Attribute, value);
                OnPropertyChanged(nameof(Modification));
            }
        }


        public int TotalExperienceValue
        {
            get => Attribute.TotalExperience;
            set
            {
                CharacterViewModel.SetExperienceToAttribute(Attribute, value);
                OnPropertyChanged(nameof(TotalExperienceValue));
            }
        }
    }
}
