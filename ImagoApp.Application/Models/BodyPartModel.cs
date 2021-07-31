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
        private int _currentHitpoints;
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
                var diff = value - _maxHitpoints;
                SetProperty(ref _maxHitpoints, value);
                if (diff != 0)
                {
                    CurrentHitpoints += diff;
                }
            }
        }

        public int CurrentHitpoints
        {
            get => _currentHitpoints;
            set
            {
                if (value > MaxHitpoints)
                {
                    //set to cap
                    SetProperty(ref _currentHitpoints, _maxHitpoints);
                }
                else
                {
                    SetProperty(ref _currentHitpoints, value);
                }
            }
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