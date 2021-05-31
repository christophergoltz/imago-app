using System;
using System.Collections.Generic;
using System.Text;
using Imago.Models.Base;
using Imago.Models.Enum;

namespace Imago.Models
{
    public class Attribute : UpgradeableSkillBase
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

        public override string ToString()
        {
            return Type.ToString();
        }
    }
}
