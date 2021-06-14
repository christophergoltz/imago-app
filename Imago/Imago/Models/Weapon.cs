using System;
using System.Collections.Generic;
using System.Text;
using Imago.Models.Enum;

namespace Imago.Models
{
    public class Weapon : DurabilityItem
    {
        private List<WeaponStance> _weaponStances;
        private string _name;

        public Weapon()
        {
            
        }

        public Weapon(string name, List<WeaponStance> weaponStances, int loadValue, int durability)
        {
            WeaponStances = weaponStances;
            Name = name;
            LoadValue = loadValue;
            DurabilityValue = durability;
        }

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public List<WeaponStance> WeaponStances
        {
            get => _weaponStances;
            set => SetProperty(ref _weaponStances, value);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}