﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Imago.Models.Enum;
using Xamarin.Forms;

namespace Imago.Converter
{
    public class AttributeSkillSourceToStringConverter : IValueConverter
    {
        private TypeToAbbreviationextConverter x = new TypeToAbbreviationextConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var list = (List<AttributeType>)value;
            return string.Join("+", list.Select(attributeType => x.Convert(attributeType, null, null, CultureInfo.InvariantCulture)));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}