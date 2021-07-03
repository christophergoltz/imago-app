using System;
using System.Globalization;
using ImagoApp.Shared.Attributes;
using Xamarin.Forms;

namespace ImagoApp.Converter
{
    public class EnumToFormulaConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Enum enumValue)
            {
                var attr = Util.EnumExtensions.GetAttribute<FormulaAttribute>(enumValue);
                if (attr == null || string.IsNullOrEmpty(attr.Formula))
                    return enumValue.ToString();

                return attr.Formula;
            }

            throw new InvalidOperationException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
