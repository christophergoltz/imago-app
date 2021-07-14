using System;
using ImagoApp.Shared.Enums;

namespace ImagoApp.Application.Models
{
    public class ArmorPartModelModel : DurabilityItemModelModel
    {
        public ArmorPartModelModel()
        {
            
        }

        public ArmorPartModelModel(ArmorPartType armorPartType, string name, int loadValue, bool fight, bool adventure, int durability, int energyDefense, int physicalDefense) 
            : base(name, loadValue, fight, adventure, durability)
        {
            ArmorPartType = armorPartType;
            PhysicalDefense = physicalDefense;
            EnergyDefense = energyDefense;
        }
        
        private int _physicalDefense;
        private int _energyDefense;
        private ArmorPartType _armorPartType;

        public int PhysicalDefense
        {
            get => _physicalDefense;
            set => SetProperty(ref _physicalDefense, value);
        }

        public int EnergyDefense
        {
            get => _energyDefense;
            set => SetProperty(ref _energyDefense, value);
        }

        public ArmorPartType ArmorPartType
        {
            get => _armorPartType;
            set => SetProperty(ref _armorPartType, value);
        }
    }
}
