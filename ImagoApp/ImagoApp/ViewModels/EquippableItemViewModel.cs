using ImagoApp.Application;
using ImagoApp.Application.Models;

namespace ImagoApp.ViewModels
{
    public class EquippableItemViewModel : BindableBase
    {
        private EquippableItemModel _equippableItemModel;
        private readonly CharacterViewModel _characterViewModel;

        public EquippableItemViewModel(EquippableItemModel equippableItemModel, CharacterViewModel characterViewModel)
        {
            _equippableItemModel = equippableItemModel;
            _characterViewModel = characterViewModel;
        }

        public EquippableItemModel EquippableItemModel
        {
            get => _equippableItemModel;
            set => SetProperty(ref _equippableItemModel, value);
        }

        public int LoadValue
        {
            get => _equippableItemModel.LoadValue;
            set
            {
                _equippableItemModel.LoadValue = value;
                OnPropertyChanged(nameof(LoadValue));
                _characterViewModel.RecalculateHandicapAttributes();
            }
        }

        public bool Fight
        {
            get => _equippableItemModel.Fight;
            set
            {
                _equippableItemModel.Fight = value;
                OnPropertyChanged(nameof(Fight));
                _characterViewModel.RecalculateHandicapAttributes();
            }
        }

        public bool Adventure
        {
            get => _equippableItemModel.Adventure;
            set
            {
                _equippableItemModel.Adventure = value;
                OnPropertyChanged(nameof(Adventure));
                _characterViewModel.RecalculateHandicapAttributes();
            }
        }
    }
}
