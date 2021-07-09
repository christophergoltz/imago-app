using System;
using System.Globalization;
using ImagoApp.Shared.Attributes;
using Xamarin.Forms;

namespace ImagoApp.Converter
{
    public class EnumToAbbreviationTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Enum enumValue)
            {
                var attr = Util.EnumExtensions.GetAttribute<AbbreviationAttribute>(enumValue);
                if (attr == null || string.IsNullOrEmpty(attr.Abbreviation))
                    return enumValue.ToString();

                return attr.Abbreviation;
            }

            throw new InvalidOperationException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new InvalidOperationException(nameof(EnumToAbbreviationTextConverter));
        }
    }
}
