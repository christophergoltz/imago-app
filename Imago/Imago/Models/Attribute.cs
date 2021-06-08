using System;
using System.Collections.Generic;
using System.Text;
using Imago.Models.Base;
using Imago.Models.Enum;
using Imago.Util;

namespace Imago.Models
{
    public class Attribute : ModifiableBase
    {
        //required for deserialization
        public Attribute()
        {
            
        }

        public Attribute(AttributeType type)
        {
            Type = type;
        }

        public AttributeType Type { get; set; } 

        private int _corrosion;
        public int Corrosion
        {
            get => _corrosion;
            set => SetProperty(ref _corrosion , value);
        }

        private int _experience;
        private int _increaseValue;

        public int Experience
        {
            get => _experience;
            set => SetProperty(ref _experience, value);
        }

        public int IncreaseValue
        {
            get => _increaseValue;
            set
            {
                SetProperty(ref _increaseValue, value);
                OnPropertyChanged(nameof(ExperienceForNextIncrease));
            }
        }


        public int ExperienceForNextIncrease => SkillIncreaseHelper.GetExperienceForNextAttributeLevel(IncreaseValue);

        public override string ToString()
        {
            return Type.ToString();
        }
    }
}
