using ImagoApp.Application;
using ImagoApp.Application.Models;

namespace ImagoApp.ViewModels
{
    public class AttributeViewModel : BindableBase
    {
        public AttributeViewModel(AttributeModel attributeModel, CharacterViewModel characterViewModel)
        {
            AttributeModel = attributeModel;
            CharacterViewModel = characterViewModel;
        }

        public AttributeModel AttributeModel { get; set; }
        public CharacterViewModel CharacterViewModel { get; }

        public int Corrosion
        {
            get => AttributeModel.Corrosion;
            set
            {
                CharacterViewModel.SetCorrosionValue(AttributeModel, value);
                OnPropertyChanged(nameof(Corrosion));
            }
        }

        public int Modification
        {
            get => AttributeModel.ModificationValue;
            set
            {
                CharacterViewModel.SetModificationValue(AttributeModel, value);
                OnPropertyChanged(nameof(Modification));
            }
        }


        public int TotalExperienceValue
        {
            get => AttributeModel.TotalExperience;
            set
            {
                CharacterViewModel.SetExperienceToAttribute(AttributeModel, value);
                OnPropertyChanged(nameof(TotalExperienceValue));
            }
        }

        public int NaturalValue
        {
            get => AttributeModel.NaturalValue;
            set
            {
                CharacterViewModel.SetNaturalValue(AttributeModel, value);
                OnPropertyChanged(nameof(NaturalValue));
            }
        }
    }
}
