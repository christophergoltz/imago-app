using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Imago.Models;
using Imago.Models.Enum;
using Xamarin.Forms;

namespace Imago.Converter
{
    public class DicionaryToBodyPartConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var bodyPartType = (BodyPartType) parameter;
            var bodyParts = (Dictionary<BodyPartType, BodyPart>) value;

            return bodyParts[bodyPartType];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
