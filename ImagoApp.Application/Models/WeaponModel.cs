using System.Collections.Generic;

namespace ImagoApp.Application.Models
{
    public class WeaponModel : DurabilityItemModelModel
    {
        private List<WeaponStanceModel> _weaponStances;

        public WeaponModel()
        {
            
        }

        public WeaponModel(string name, List<WeaponStanceModel> weaponStances, bool fight, bool adventure, int loadValue, int durability)
        : base(name, loadValue, fight, adventure, durability )
        {
            WeaponStances = weaponStances;
        }
        
        public List<WeaponStanceModel> WeaponStances
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