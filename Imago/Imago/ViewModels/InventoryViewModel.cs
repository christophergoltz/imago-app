using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Imago.Models;
using Imago.Services;
using Xamarin.Forms;

namespace Imago.ViewModels
{
    public class InventoryViewModel
    {
        private readonly ICharacterService _characterService;
        public Character Character { get; }

        public ICommand DeleteSelectedEquippedItem { get; }
        public ICommand AddNewEquippedItem { get; }

        public ObservableCollection<EquippableItemViewModel> EquippableItemViewModels { get; set; }

        public InventoryViewModel(Character character, ICharacterService characterService)
        {
            _characterService = characterService;
            Character = character;

            DeleteSelectedEquippedItem = new Command<EquippableItemViewModel>(item =>
            {
                EquippableItemViewModels.Remove(item);
                character.EquippedItems.Remove(item.EquipableItem);
                _characterService.RecalculateHandicapAttributes(Character);
            });

            AddNewEquippedItem = new Command(() =>
            {
                var equipableItem = new EquipableItem(string.Empty, false, false, 1, 0);
                Character.EquippedItems.Add(equipableItem);
                EquippableItemViewModels.Add(new EquippableItemViewModel(equipableItem, _characterService, Character));
            });

            EquippableItemViewModels = new ObservableCollection<EquippableItemViewModel>(
                character.EquippedItems.Select(item => new EquippableItemViewModel(item, _characterService, character)));
        }
    }
}
