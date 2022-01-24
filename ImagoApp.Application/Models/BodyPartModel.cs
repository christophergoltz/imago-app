using System.Collections.ObjectModel;
using ImagoApp.Shared.Enums;

namespace ImagoApp.Application.Models
{
    public class BodyPartModel : BindableBase
    {
        private int _maxHitpoints;
        private double _currentHitpointspercentage;
        private ObservableCollection<ArmorPartModelModel> _armor;
        private BodyPartType _type;

        public BodyPartModel(BodyPartType type, ObservableCollection<ArmorPartModelModel> armor)
        {
            Type = type;
            Armor = armor;
        }

        public int MaxHitpoints
        {
            get => _maxHitpoints;
            set
            {
                SetProperty(ref _maxHitpoints, value);
                OnPropertyChanged(nameof(CurrentHitpoints));
                OnPropertyChanged(nameof(MissingHitpoints));
            }
        }

        public double CurrentHitpointsPercentage
        {
            get => _currentHitpointspercentage;
            set
            {
                SetProperty(ref _currentHitpointspercentage, value);
                OnPropertyChanged(nameof(CurrentHitpoints));
                OnPropertyChanged(nameof(MissingHitpoints));
            }
        }

        public int MissingHitpoints => MaxHitpoints - CurrentHitpoints;
        public int CurrentHitpoints => (MaxHitpoints * CurrentHitpointsPercentage).GetRoundedValue();

        public ObservableCollection<ArmorPartModelModel> Armor
        {
            get => _armor;
            set => SetProperty(ref _armor, value);
        }

        public BodyPartType Type
        {
            get => _type;
            set => SetProperty(ref _type, value);
        }
    }
}