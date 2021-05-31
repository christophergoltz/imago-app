using System;
using System.Collections.Generic;
using System.Text;
using Imago.Models.Base;
using Imago.Models.Enum;
using Imago.Util;

namespace Imago.Models
{
    //eg. schadensmod, traglast
    public class DerivedAttribute : BindableBase
    {
        private DerivedAttributeType _type;
        private int _finalValue;
        private string _formula;

        public DerivedAttributeType Type
        {
            get => _type;
            set => SetProperty(ref _type, value);
        }

        public int FinalValue
        {
            get => _finalValue;
            set => SetProperty(ref _finalValue, value);
        }

        public string Formula
        {
            get => _formula;
            set => SetProperty(ref _formula, value);
        }
    }
}