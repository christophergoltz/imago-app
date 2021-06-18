using System;
using System.Collections.Generic;
using System.Text;
using Imago.Models.Enum;

namespace Imago.Models
{
    public class Weapon : DurabilityItem
    {
        private List<WeaponStance> _weaponStances;

        public Weapon()
        {
            
        }

        public Weapon(string name, List<WeaponStance> weaponStances, bool fight, bool adventure, int loadValue, int durability)
        : base(name, loadValue, fight, adventure, durability )
        {
            WeaponStances = weaponStances;
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