using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace ImagoApp.Converter
{
    public class FileInfoLenghtToSmartFileSizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var byteCount = (long) value;
            string[] suffix = {"B", "KB", "MB", "GB", "TB", "PB", "EB"};
            if (byteCount == 0)
                return "0" + suffix[0];
            var bytes = Math.Abs(byteCount);
            var place = (int) Math.Floor(Math.Log(bytes, 1024));
            var num = Math.Round(bytes / Math.Pow(1024, place), 1);
            return (Math.Sign(byteCount) * num).ToString(CultureInfo.InvariantCulture) + suffix[place];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new InvalidOperationException(nameof(FileInfoLenghtToSmartFileSizeConverter));
        }
    }
}
