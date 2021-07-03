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
                characterViewModel.Character.EquippedItems.Remove(item.EquipableItem);
                characterViewModel.RecalculateHandicapAttributes();
            });

            AddNewEquippedItem = new Command(() =>
            {
                var equipableItem = new EquipableItem(string.Empty,0, false, false);
                CharacterViewModel.Character.EquippedItems.Add(equipableItem);
                EquippableItemViewModels.Add(new EquippableItemViewModel(equipableItem, characterViewModel));
            });

            EquippableItemViewModels = new ObservableCollection<EquippableItemViewModel>(
                characterViewModel.Character.EquippedItems.Select(item => new EquippableItemViewModel(item, characterViewModel)));
        }
    }
}
