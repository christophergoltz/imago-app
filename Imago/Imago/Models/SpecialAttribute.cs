using System;
using System.Collections.Generic;
using System.Text;
using Imago.Models.Base;
using Imago.Models.Enum;

namespace Imago.Models
{
    //eg. Initiative
    public class SpecialAttribute : SkillBase
    {
        private SpecialAttributeType _type;

        public SpecialAttributeType Type
        {
            get => _type;
            set => SetProperty(ref _type, value);
        }
    }
}
