using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Imago.Models;
using Imago.Models.Enum;
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

        public List<DerivedAttribute> DerivedAttributes { get; set; }

        public ObservableCollection<EquippableItemViewModel> EquippableItemViewModels { get; set; }

        public InventoryViewModel(Character character, ICharacterService characterService)
        {
            _characterService = characterService;
            Character = character;

            DerivedAttributes = character.DerivedAttributes
                .Where(_ => _.Type == DerivedAttributeType.SprungreichweiteAbenteuer ||
                            _.Type == DerivedAttributeType.SprunghoeheAbenteuer ||
                            _.Type == DerivedAttributeType.SprungreichweiteGesamt ||
                            _.Type == DerivedAttributeType.SprunghoeheGesamt)
                .ToList();

            DeleteSelectedEquippedItem = new Command<EquippableItemViewModel>(item =>
            {
                EquippableItemViewModels.Remove(item);
                character.EquippedItems.Remove(item.EquipableItem);
                _characterService.RecalculateHandicapAttributes(Character);
            });

            AddNewEquippedItem = new Command(() =>
            {
                var equipableItem = new EquipableItem(string.Empty,0, false, false);
                Character.EquippedItems.Add(equipableItem);
                EquippableItemViewModels.Add(new EquippableItemViewModel(equipableItem, _characterService, Character));
            });

            EquippableItemViewModels = new ObservableCollection<EquippableItemViewModel>(
                character.EquippedItems.Select(item => new EquippableItemViewModel(item, _characterService, character)));
        }
    }
}
