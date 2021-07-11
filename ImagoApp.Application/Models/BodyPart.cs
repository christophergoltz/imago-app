using System;
using System.Collections.ObjectModel;
using System.Drawing;
using ImagoApp.Shared.Enums;
using Newtonsoft.Json;

namespace ImagoApp.Application.Models
{
    public class BodyPart : BindableBase
    {
        private int _maxHitpoints;
        private int _currentHitpoints;
        private ObservableCollection<ArmorPartModel> _armor;
        private BodyPartType _type;

        public BodyPart(BodyPartType type, int currentHitpoints, ObservableCollection<ArmorPartModel> armor)
        {
            Type = type;
            CurrentHitpoints = currentHitpoints;
            Armor = armor;
        }

        public int MaxHitpoints
        {
            get => _maxHitpoints;
            set => SetProperty(ref _maxHitpoints, value);
        }

        public int CurrentHitpoints
        {
            get => _currentHitpoints;
            set => SetProperty(ref _currentHitpoints, value);
        }
        
        public ObservableCollection<ArmorPartModel> Armor
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