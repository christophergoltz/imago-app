using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Imago.Util;
using Xamarin.Forms;

namespace Imago.Converter
{
    public class PositiveIntegerToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int intValue)
            {
                if (intValue >= 0)
                    return true;

                return false;
            }

            throw new InvalidOperationException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
