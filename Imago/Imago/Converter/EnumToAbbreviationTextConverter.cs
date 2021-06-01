using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Imago.Models.Enum;
using Imago.Util;
using Xamarin.Forms;

namespace Imago.Converter
{
    public class EnumToAbbreviationTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Enum enumValue)
            {
                var attr = EnumExtensions.GetAttribute<AbbreviationAttribute>(enumValue);
                if (attr == null || string.IsNullOrEmpty(attr.Abbreviation))
                    return enumValue.ToString();

                return attr.Abbreviation;
            }

            throw new InvalidOperationException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
