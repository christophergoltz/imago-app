using System;
using System.Collections.Generic;
using System.Text;
using Imago.Models.Enum;
using Imago.Shared.Models;

namespace Imago.Models
{
    public class Weapon : DurabilityItem
    {
        public Weapon()
        {
            
        }

        public WeaponType Type
        {
            get => _type;
            set => SetProperty(ref _type, value);
        }

        private Dictionary<WeaponStanceType, WeaponStance> _weaponStances;
        private WeaponType _type;

        public Dictionary<WeaponStanceType, WeaponStance> WeaponStances
        {
            get => _weaponStances;
            set => SetProperty(ref _weaponStances, value);
        }

        public Weapon(WeaponType type ,Dictionary<WeaponStanceType, WeaponStance> weaponStances )
        {
            WeaponStances = weaponStances;
            Type = type;
        }
    }
}