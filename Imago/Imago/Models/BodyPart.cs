using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Imago.Models.Enum;
using Imago.Util;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace Imago.Models
{
    public class BodyPart : BindableBase
    {
        private int _maxHitpoints;
        private int _currentHitpoints;
        private ObservableCollection<ArmorModel> _armor;
        private BodyPartType _type;
        private string _formula;

        public BodyPart(BodyPartType type, string formula, int currentHitpoints, ObservableCollection<ArmorModel> armor)
        {
            Type = type;
            Formula = formula;
            CurrentHitpoints = currentHitpoints;
            Armor = armor;
        }

        public int MaxHitpoints
        {
            get => _maxHitpoints;
            set
            {
                SetProperty(ref _maxHitpoints, value);
                OnPropertyChanged(nameof(CurrentHitpointsPercentage));
                OnPropertyChanged(nameof(HitpointsColor));
            }
        }

        public int CurrentHitpoints
        {
            get => _currentHitpoints;
            set
            {
                SetProperty(ref _currentHitpoints, value);
                OnPropertyChanged(nameof(CurrentHitpointsPercentage));
                OnPropertyChanged(nameof(HitpointsColor));
            }
        }

        private Color GetBlendedColor(int percentage)
        {
            if (percentage < 50)
                return Interpolate(Color.Red, Color.Yellow, percentage / 50.0);
            return Interpolate(Color.Yellow, Color.Lime, (percentage - 50) / 50.0);
        }

        private Color Interpolate(Color color1, Color color2, double fraction)
        {
            var r = Interpolate(color1.R, color2.R, fraction);
            var g = Interpolate(color1.G, color2.G, fraction);
            var b = Interpolate(color1.B, color2.B, fraction);
            return new Color(r, g, b);
        }

        private double Interpolate(double d1, double d2, double f)
        {
            return d1 + (d2 - d1) * f;
        }

        [JsonIgnore]
        public Color HitpointsColor => GetBlendedColor((int)(CurrentHitpointsPercentage*100));

        [JsonIgnore]
        public double CurrentHitpointsPercentage
        {
            get
            {
                var f = (double) CurrentHitpoints / MaxHitpoints;
                return f;
            }
        }
        
        public ObservableCollection<ArmorModel> Armor
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