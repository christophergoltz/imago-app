using System;
using System.Collections.Generic;
using System.Text;
using Imago.Models.Enum;
using Imago.Util;

namespace Imago.Models
{
    public class BodyPart : BindableBase
    {
        private int _maxHitpoints;
        private int _currentHitpoints;
        private List<ArmorType> _armor;
        private BodyPartType _type;
        private string _formula;

        public BodyPart(BodyPartType type, string formula, int currentHitpoints, List<ArmorType> armor)
        {
            Type = type;
            Formula = formula;
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

        public List<ArmorType> Armor
        {
            get => _armor;
            set => SetProperty(ref _armor, value);
        }

        public BodyPartType Type
        {
            get => _type;
            set => SetProperty(ref _type, value);
        }

        public string Formula
        {
            get => _formula;
            set => SetProperty(ref _formula, value);
        }
    }
}