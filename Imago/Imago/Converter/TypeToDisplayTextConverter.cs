﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Imago.Attributes;
using Imago.Models.Enum;
using Xamarin.Forms;

namespace Imago.Converter
{
    public class TypeToDisplayTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is AttributeType attributeType)
            {
                var attr = EnumExtensions.GetAttribute<DisplayTextAttribute>(attributeType);
                if (attr == null || string.IsNullOrEmpty(attr.Text))
                    return attributeType.ToString();

                return attr.Text;
            }
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}