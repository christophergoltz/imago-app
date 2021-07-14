using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using ImagoApp.Application.Models;
using Xamarin.Forms;

namespace ImagoApp.ViewModels
{
    public class InventoryViewModel
    {
        public CharacterViewModel CharacterViewModel { get; }

        public ICommand DeleteSelectedEquippedItem { get; }
        public ICommand AddNewEquippedItem { get; }
        
        public ObservableCollection<EquippableItemViewModel> EquippableItemViewModels { get; set; }

        public InventoryViewModel(CharacterViewModel characterViewModel)
        {
            CharacterViewModel = characterViewModel;

            DeleteSelectedEquippedItem = new Command<EquippableItemViewModel>(item =>
            {
                EquippableItemViewModels.Remove(item);
                characterViewModel.CharacterModel.EquippedItems.Remove(item.EquipableItemModel);
                characterViewModel.RecalculateHandicapAttributes();
            });

            AddNewEquippedItem = new Command(() =>
            {
                var equipableItem = new EquipableItemModel(string.Empty,0, false, false);
                CharacterViewModel.CharacterModel.EquippedItems.Add(equipableItem);
                EquippableItemViewModels.Add(new EquippableItemViewModel(equipableItem, characterViewModel));
            });

            EquippableItemViewModels = new ObservableCollection<EquippableItemViewModel>(
                characterViewModel.CharacterModel.EquippedItems.Select(item => new EquippableItemViewModel(item, characterViewModel)));
        }
    }
}
