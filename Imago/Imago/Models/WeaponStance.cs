using System;
using System.Collections.Generic;
using System.Text;
using Imago.Models.Enum;
using Imago.Util;

namespace Imago.Models
{
    public class WeaponStance : BindableBase
    {
        private WeaponStanceType _type;
        private string _phaseValue;
        private string _damageFormula;
        private int? _parryModifier;
        private string _range;

        public WeaponStance(WeaponStanceType type, string phaseValue, string damageFormula, int? parryModifier, string range, int loadValue)
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

        public string PhaseValue
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

        public string Range
        {
            get => _range;
            set => SetProperty(ref _range, value);
        }
    }
}