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
        private string _formula;

        public DerivedAttribute(DerivedAttributeType type, string formula)
        {
            Type = type;
            Formula = formula;
        }

        public DerivedAttributeType Type
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