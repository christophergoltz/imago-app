using System;
using System.Globalization;
using System.Linq;
using Xamarin.Forms;

namespace ImagoApp.Converter
{
    public class SkillGroupTypeToAttributeSourceStringConverter : IValueConverter
    {
        private readonly EnumToAbbreviationTextConverter _enumToAbbreviationTextConverter = new EnumToAbbreviationTextConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var skillGroupType = (Models.Enum.SkillGroupModelType)value;

            //todo meh, remove direct access to repo
            var sources = new Repository.RuleRepository().GetSkillGroupSources(skillGroupType);

            return string.Join("+", sources.Select(attributeType => _enumToAbbreviationTextConverter.Convert(attributeType, null, null, CultureInfo.InvariantCulture)));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
