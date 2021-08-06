using System;
using System.Collections.ObjectModel;
using System.Drawing;
using ImagoApp.Shared.Enums;
using Newtonsoft.Json;

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
            set => SetProperty(ref _maxHitpoints, value);
        }

        public double CurrentHitpointsPercentage
        {
            get => _currentHitpointspercentage;
            set => SetProperty(ref _currentHitpointspercentage, value);
        }

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