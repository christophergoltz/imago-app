using ImagoApp.Shared.Attributes;

namespace ImagoApp.Application.Models
{
    public enum WeaveTalentSettingModelType
    {
        Unknown = 0,
        [DisplayText("Dauer")]
        Duration,
        [DisplayText("Stabilität der Welt")]
        StrengthOfWorld,
        [DisplayText("Stärke der Kunst")]
        StrengthOfTalent,
        [DisplayText("Reichweite (Radius)")]
        RadiusRange,
        [DisplayText("Reichweite (Durchmesser)")]
        DiameterRange,
        [DisplayText("Objekt Volumen")]
        ObjectVolume,
        [DisplayText("Produkt Volumen")]
        ProductVolume,
        [DisplayText("Volumen des Bildes")]
        ImageVolume,
    }

    public class WeaveTalentSettingModel : BindableBase
    {
        private string _abbreviation;
        private int _quantity;
        private string _unit;
        private double _stepValue;
        private WeaveTalentSettingModelType _type;

        public string Abbreviation
        {
            get => _abbreviation;
            set => SetProperty(ref _abbreviation, value);
        }

        public WeaveTalentSettingModelType Type
        {
            get => _type;
            set => SetProperty(ref _type ,value);
        }
        
        public int Quantity
        {
            get => _quantity;
            set
            {
                SetProperty(ref _quantity, value);
                OnPropertyChanged(nameof(FinalValue));
            }
        }

        public string Unit
        {
            get => _unit;
            set => SetProperty(ref _unit, value);
        }

        public double StepValue
        {
            get => _stepValue;
            set
            {
                SetProperty(ref _stepValue, value);
                OnPropertyChanged(nameof(FinalValue));
            }
        }

        public double FinalValue => Quantity * StepValue;
    }
}
