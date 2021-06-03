using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using Imago.Models.Base;

namespace Imago.Models
{
    public class EquipableItem : ItemBase
    {
        private bool _fight;
        private bool _adventure;
        private string _name;
        private int _quantity;

        public EquipableItem(string name, bool adventure, bool fight, int quantity)
        {
            Name = name;
            Adventure = adventure;
            Fight = fight;
            Quantity = quantity;
        }

        public bool Fight
        {
            get => _fight;
            set => SetProperty(ref _fight ,value);
        }

        public bool Adventure
        {
            get => _adventure;
            set => SetProperty(ref _adventure, value);
        }
        
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name ,value);
        }

        public int Quantity
        {
            get => _quantity;
            set => SetProperty(ref _quantity, value);
        }
    }
}
