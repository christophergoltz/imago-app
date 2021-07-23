using System;
using System.Globalization;
using System.Linq;
using ImagoApp.Application.Constants;
using ImagoApp.Application.Services;
using ImagoApp.Shared.Enums;
using Xamarin.Forms;

namespace ImagoApp.Converter
{
    public class SkillGroupTypeToAttributeSourceStringConverter : IValueConverter
    {
        private readonly EnumToAbbreviationTextConverter _enumToAbbreviationTextConverter = new EnumToAbbreviationTextConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var skillGroupType = (SkillGroupModelType)value;
            var sources = RuleConstants.GetSkillGroupSources(skillGroupType);

            return string.Join("+", sources.Select(attributeType => _enumToAbbreviationTextConverter.Convert(attributeType, null, null, CultureInfo.InvariantCulture)));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
