﻿using System;
using System.Globalization;
using Xamarin.Forms;

namespace ImagoApp.Converter
{
    public class ObjectNotNullToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new InvalidOperationException(nameof(ObjectNotNullToBoolConverter));
        }
    }
}