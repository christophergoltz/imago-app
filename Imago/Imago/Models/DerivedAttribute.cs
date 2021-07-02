using System;
using System.Collections.Generic;
using System.Text;
using Imago.Models.Base;
using Imago.Models.Enum;
using Imago.Util;

namespace Imago.Models
{
    //eg. schadensmod, traglast
    public class DerivedAttribute : CalculableBase
    {
        private DerivedAttributeType _type;

        public DerivedAttribute(DerivedAttributeType type)
        {
            Type = type;
        }

        public DerivedAttributeType Type
        {
            get => _type;
            set => SetProperty(ref _type, value);
        }
    }
}