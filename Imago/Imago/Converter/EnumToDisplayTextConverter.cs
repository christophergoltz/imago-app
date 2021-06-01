using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Imago.Models.Enum;
using Imago.Util;
using Xamarin.Forms;

namespace Imago.Converter
{
    public class EnumToDisplayTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Enum enumValue)
            {
                var attr = EnumExtensions.GetAttribute<DisplayTextAttribute>(enumValue);
                if (attr == null || string.IsNullOrEmpty(attr.Text))
                    return enumValue.ToString();

                return attr.Text;
            }

            throw new InvalidOperationException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
