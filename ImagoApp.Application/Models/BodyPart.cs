using System;
using System.Collections.ObjectModel;
using System.Drawing;
using ImagoApp.Shared.Enums;
using ImagoApp.Util;
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

        //private Color GetBlendedColor(int percentage)
        //{
        //    var redHex = (Color)Application.Current.Resources["RotesUmbra1"];
        //    var yellowHex = (Color)Application.Current.Resources["GelbesUmbra2"];
        //    var greenHex = (Color)Application.Current.Resources["HellGruenesUmbra2"];
           
        //    if (percentage < 50)
        //        return Interpolate(redHex, yellowHex, percentage / 50.0);
        //    return Interpolate(yellowHex, greenHex, (percentage - 50) / 50.0);
        //}

        //private Color Interpolate(Color color1, Color color2, double fraction)
        //{
        //    var r = Interpolate(color1.R, color2.R, fraction);
        //    var g = Interpolate(color1.G, color2.G, fraction);
        //    var b = Interpolate(color1.B, color2.B, fraction);
        //    return new Color(r, g, b);
        //}

        //private double Interpolate(double d1, double d2, double f)
        //{
        //    return d1 + (d2 - d1) * f;
        //}
        
        //todo move to vm
        public Color HitpointsColor => Color.BlueViolet;//todo GetBlendedColor((int)(CurrentHitpointsPercentage*100));

        //todo move to vm
        public double CurrentHitpointsPercentage
        {
            get
            {
                var f = (double) CurrentHitpoints / MaxHitpoints;
                return f;
            }
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