using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ImagoApp.Converter
{
    public class EnumToValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            return (int) value;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
