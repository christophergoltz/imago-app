using ImagoApp.Application.Models;

namespace ImagoApp.ViewModels
{
    public class EquippableItemViewModel : Util.BindableBase
    {
        private EquipableItem _equipableItem;
        private readonly CharacterViewModel _characterViewModel;

        public EquippableItemViewModel(EquipableItem equipableItem, CharacterViewModel characterViewModel)
        {
            _equipableItem = equipableItem;
            _characterViewModel = characterViewModel;
        }

        public EquipableItem EquipableItem
        {
            get => _equipableItem;
            set => SetProperty(ref _equipableItem, value);
        }

        public int LoadValue
        {
            get => _equipableItem.LoadValue;
            set
            {
                _equipableItem.LoadValue = value;
                OnPropertyChanged(nameof(LoadValue));
                _characterViewModel.RecalculateHandicapAttributes();
            }
        }

        public bool Fight
        {
            get => _equipableItem.Fight;
            set
            {
                _equipableItem.Fight = value;
                OnPropertyChanged(nameof(Fight));
                _characterViewModel.RecalculateHandicapAttributes();
            }
        }

        public bool Adventure
        {
            get => _equipableItem.Adventure;
            set
            {
                _equipableItem.Adventure = value;
                OnPropertyChanged(nameof(Adventure));
                _characterViewModel.RecalculateHandicapAttributes();
            }
        }
    }
}
