using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Imago.Models.Enum;
using Imago.Repository;
using Xamarin.Forms;

namespace Imago.Converter
{
    public class SkillGroupTypeToAttributeSourceStringConverter : IValueConverter
    {
        private readonly EnumToAbbreviationTextConverter _enumToAbbreviationTextConverter = new EnumToAbbreviationTextConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var skillGroupType = (SkillGroupType)value;

            //todo meh, remove direct access to repo
            var sources = new RuleRepository().GetSkillGroupSources(skillGroupType);

            return string.Join("+", sources.Select(attributeType => _enumToAbbreviationTextConverter.Convert(attributeType, null, null, CultureInfo.InvariantCulture)));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
