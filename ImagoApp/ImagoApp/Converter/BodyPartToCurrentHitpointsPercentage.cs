using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using ImagoApp.Application.Models;
using ImagoApp.Shared.Enums;
using Xamarin.Forms;

namespace ImagoApp.Converter
{
    public class BodyPartToCurrentHitpointsPercentage : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is BodyPartModel bodyPart)
            {
                var currentHitpointsPercentage = ((double)bodyPart.CurrentHitpoints / bodyPart.MaxHitpoints);
                if (currentHitpointsPercentage > 100)
                    currentHitpointsPercentage = 100;

                if (currentHitpointsPercentage < 0)
                    currentHitpointsPercentage = 0;
                return currentHitpointsPercentage;
            }

            throw new InvalidOperationException(nameof(BodyPartToCurrentHitpointsPercentage));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException(nameof(DicionaryToBodyPartConverter));
        }
    }
}
