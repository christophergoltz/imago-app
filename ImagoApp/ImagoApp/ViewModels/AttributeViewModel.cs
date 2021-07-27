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
                if (AttributeModel.Corrosion != value)
                {
                    CharacterViewModel.SetCorrosion(AttributeModel, value);
                    OnPropertyChanged(nameof(Corrosion));
                }
            }
        }

        public int Modification
        {
            get => AttributeModel.ModificationValue;
            set
            {
                if (AttributeModel.ModificationValue != value)
                {
                    CharacterViewModel.SetModification(AttributeModel, value);
                    OnPropertyChanged(nameof(Modification));
                }
            }
        }

        public int SpecialExperience
        {
            get => AttributeModel.ExperienceValue;
            set
            {
                if (AttributeModel.ExperienceValue != value)
                {
                    CharacterViewModel.SetExperienceToAttribute(AttributeModel, value);
                    OnPropertyChanged(nameof(SpecialExperience));
                }
            }
        }

        public int CreationExperience
        {
            get => AttributeModel.CreationExperience;
            set
            {
                if (AttributeModel.CreationExperience != value)
                {
                    CharacterViewModel.SetCreationExperienceToAttribute(AttributeModel, value);
                    OnPropertyChanged(nameof(CreationExperience));
                }
            }
        }

        public int BaseValue
        {
            get => AttributeModel.BaseValue;
            set
            {
                if (AttributeModel.BaseValue != value)
                {
                    CharacterViewModel.SetBaseValue(AttributeModel, value);
                    OnPropertyChanged(nameof(BaseValue));
                }
            }
        }
    }
}
