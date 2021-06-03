using System;
using System.Collections.Generic;
using System.Text;
using Imago.Models.Enum;
using Imago.Util;

namespace Imago.Models
{
    public class WeaponStance : DurabilityItem
    {
        private WeaponStanceType _type;
        private int _phaseValue;
        private string _damageFormula;
        private int? _parryModifier;
        private int? _range;

        public WeaponStance(WeaponStanceType type, int phaseValue, string damageFormula, int? parryModifier, int? range)
        {
            Type = type;
            ParryModifier = parryModifier;
            DamageFormula = damageFormula;
            PhaseValue = phaseValue;
            Range = range;
        }

        public WeaponStanceType Type
        {
            get => _type;
            set => SetProperty(ref _type, value);
        }

        public int PhaseValue
        {
            get => _phaseValue;
            set => SetProperty(ref _phaseValue, value);
        }

        public string DamageFormula
        {
            get => _damageFormula;
            set => SetProperty(ref _damageFormula, value);
        }

        public int? ParryModifier
        {
            get => _parryModifier;
            set => SetProperty(ref _parryModifier, value);
        }

        public int? Range
        {
            get => _range;
            set => SetProperty(ref _range, value);
        }
    }
}