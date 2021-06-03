using System;
using System.Collections.Generic;
using System.Text;
using Imago.Models;

namespace Imago.ViewModels
{
    public class InventoryViewModel
    {
        private readonly Character _character;

        public InventoryViewModel(Character character)
        {
            _character = character;
        }
    }
}
