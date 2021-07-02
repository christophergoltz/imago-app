namespace ImagoApp.Models
{
    public class WeaponStance : Util.BindableBase
    {
        private string _type;
        private string _phaseValue;
        private string _damageFormula;
        private string _parryModifier;
        private string _range;


        public WeaponStance(string type, string phaseValue, string damageFormula, string parryModifier, string range)
        {
            Type = type;
            ParryModifier = parryModifier;
            DamageFormula = damageFormula;
            PhaseValue = phaseValue;
            Range = range;
        }
        
        public string Type
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

        public string ParryModifier
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