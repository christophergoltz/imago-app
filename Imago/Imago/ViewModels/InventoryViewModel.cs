using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Imago.Models;
using Xamarin.Forms;

namespace Imago.ViewModels
{
    public class InventoryViewModel
    {
        public Character Character { get; }

        public ICommand DeleteSelectedEquippedItem { get; }

        public InventoryViewModel(Character character)
        {
            Character = character;

            DeleteSelectedEquippedItem = new Command<EquipableItem>(item =>
            {
                character.EquippedItems.Remove(item);
            });
        }
    }
}
