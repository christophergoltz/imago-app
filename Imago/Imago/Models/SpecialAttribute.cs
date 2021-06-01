using System;
using System.Collections.Generic;
using System.Text;
using Imago.Models.Base;
using Imago.Models.Enum;

namespace Imago.Models
{
    //eg. Initiative
    public class SpecialAttribute : DependentBase
    {
        private SpecialAttributeType _type;
        private string _formula;

        public SpecialAttribute()
        {
            
        }

        public SpecialAttribute(SpecialAttributeType type, string formula)
        {
            Type = type;
            Formula = formula;
        }

        public SpecialAttributeType Type
        {
            get => _type;
            set => SetProperty(ref _type, value);
        }

        public string Formula
        {
            get => _formula;
            set => SetProperty(ref _formula, value);
        }
    }
}
