using ImagoApp.Application;
using ImagoApp.Application.Models;

namespace ImagoApp.ViewModels
{
    public class SpecialAttributeViewModel : BindableBase
    {
        private readonly CharacterViewModel _characterViewModel;

        public SpecialAttributeViewModel(CharacterViewModel characterViewModel, SpecialAttributeModel specialAttributeModel)
        {
            _characterViewModel = characterViewModel;
            SpecialAttributeModel = specialAttributeModel;
        }

        public SpecialAttributeModel SpecialAttributeModel { get; set; }
        public CharacterModel CharacterModel { get; set; }
        
        public int Modification
        {
            get => SpecialAttributeModel.ModificationValue;
            set
            {
                _characterViewModel.SetModificationValue(SpecialAttributeModel, value);
                OnPropertyChanged(nameof(Modification));
            }
        }
    }
}
