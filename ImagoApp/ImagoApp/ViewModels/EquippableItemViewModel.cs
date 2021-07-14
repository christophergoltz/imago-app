using ImagoApp.Application;
using ImagoApp.Application.Models;

namespace ImagoApp.ViewModels
{
    public class EquippableItemViewModel : BindableBase
    {
        private EquipableItemModel _equipableItemModel;
        private readonly CharacterViewModel _characterViewModel;

        public EquippableItemViewModel(EquipableItemModel equipableItemModel, CharacterViewModel characterViewModel)
        {
            _equipableItemModel = equipableItemModel;
            _characterViewModel = characterViewModel;
        }

        public EquipableItemModel EquipableItemModel
        {
            get => _equipableItemModel;
            set => SetProperty(ref _equipableItemModel, value);
        }

        public int LoadValue
        {
            get => _equipableItemModel.LoadValue;
            set
            {
                _equipableItemModel.LoadValue = value;
                OnPropertyChanged(nameof(LoadValue));
                _characterViewModel.RecalculateHandicapAttributes();
            }
        }

        public bool Fight
        {
            get => _equipableItemModel.Fight;
            set
            {
                _equipableItemModel.Fight = value;
                OnPropertyChanged(nameof(Fight));
                _characterViewModel.RecalculateHandicapAttributes();
            }
        }

        public bool Adventure
        {
            get => _equipableItemModel.Adventure;
            set
            {
                _equipableItemModel.Adventure = value;
                OnPropertyChanged(nameof(Adventure));
                _characterViewModel.RecalculateHandicapAttributes();
            }
        }
    }
}
