using System;
using System.Collections.Generic;
using System.Globalization;
using ImagoApp.Application.Models;
using ImagoApp.Shared.Enums;
using Xamarin.Forms;

namespace ImagoApp.Converter
{
    public class DicionaryToBodyPartConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var bodyPartType = (BodyPartType) parameter;
            var bodyParts = (Dictionary<BodyPartType, BodyPartModel>) value;

            return bodyParts[bodyPartType];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new InvalidOperationException(nameof(DicionaryToBodyPartConverter));
        }
    }
}
