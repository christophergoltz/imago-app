using System;
using System.Collections.Generic;
using System.Globalization;
using Xamarin.Forms;

namespace ImagoApp.Converter
{
    public class DicionaryToBodyPartConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var bodyPartType = (Models.Enum.BodyPartType) parameter;
            var bodyParts = (Dictionary<Models.Enum.BodyPartType, Models.BodyPart>) value;

            return bodyParts[bodyPartType];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
