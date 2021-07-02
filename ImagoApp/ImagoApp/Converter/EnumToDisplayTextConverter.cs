using System;
using System.Globalization;
using Xamarin.Forms;

namespace ImagoApp.Converter
{
    public class EnumToDisplayTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Enum enumValue)
            {
                var attr = Util.EnumExtensions.GetAttribute<Util.DisplayTextAttribute>(enumValue);
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
