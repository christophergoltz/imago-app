using System;
using System.Collections.Generic;
using System.Text;

namespace Imago.Shared.Models
{
    public class ArmorModel : DurabilityItem
    {
        public ArmorModel()
        {
            
        }

        public ArmorModel(ArmorModelType type, int physicalDefense, int energyDefense, int loadValue, int durability) : base(durability)
        {
            Type = type;
            PhysicalDefense = physicalDefense;
            EnergyDefense = energyDefense;
            LoadValue = loadValue;
        }

        private bool _fight;
        private bool _adventure;
        private int _physicalDefense;
        private int _energyDefense;
        private ArmorModelType _type;

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

        public ArmorModelType Type
        {
            get => _type;
            set => SetProperty(ref _type, value);
        }

        public bool Fight
        {
            get => _fight;
            set => SetProperty(ref _fight, value);
        }

        public bool Adventure
        {
            get => _adventure;
            set => SetProperty(ref _adventure, value);
        }
    }
}
