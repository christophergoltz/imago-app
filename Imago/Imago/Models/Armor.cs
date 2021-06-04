﻿using System;
using System.Collections.Generic;
using System.Text;
using Imago.Models.Enum;

namespace Imago.Models
{
    public class Armor : DurabilityItem
    {
        public Armor(ArmorType type, int physicalDefense, int energyDefense, int loadValue = 0)
        {
            Type = type;
            PhysicalDefense = physicalDefense;
            EnergyDefense = energyDefense;
            LoadValue = loadValue;
        }

        private int _physicalDefense;
        private int _energyDefense;
        private ArmorType _type;

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

        public ArmorType Type
        {
            get => _type;
            set => SetProperty(ref _type, value);
        }
    }
}