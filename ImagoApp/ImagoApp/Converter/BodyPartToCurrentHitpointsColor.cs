using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using ImagoApp.Application.Models;
using Xamarin.Forms;

namespace ImagoApp.Converter
{
    public class BodyPartToCurrentHitpointsColor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is BodyPart bodyPart)
            {
                var currentHitpointsPercentage = (int)((double)bodyPart.CurrentHitpoints / bodyPart.MaxHitpoints * 100);
                if (currentHitpointsPercentage > 100)
                    currentHitpointsPercentage = 100;

                if (currentHitpointsPercentage < 0)
                    currentHitpointsPercentage = 0;

                return GetBlendedColor((int) currentHitpointsPercentage);
            }

            throw new InvalidOperationException(nameof(BodyPartToCurrentHitpointsPercentage));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException(nameof(DicionaryToBodyPartConverter));
        }

        private Color GetBlendedColor(int percentage)
        {
            var redHex = (Color)Xamarin.Forms.Application.Current.Resources["RotesUmbra1"];
            var yellowHex = (Color)Xamarin.Forms.Application.Current.Resources["GelbesUmbra2"];
            var greenHex = (Color)Xamarin.Forms.Application.Current.Resources["HellGruenesUmbra2"];

            if (percentage < 50)
                return Interpolate(redHex, yellowHex, percentage / 50.0);
            return Interpolate(yellowHex, greenHex, (percentage - 50) / 50.0);
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
    }
}
