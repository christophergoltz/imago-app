using System;
using System.Collections.Generic;
using System.Text;
using Imago.Util;

namespace Imago.Models.Base
{
    public abstract class SkillBase : BindableBase
    {
        private int _naturalValue;
        private int _modificationValue;
        private int _finalValue;
        
        public int NaturalValue
        {
            get => _naturalValue;
            set => SetProperty(ref _naturalValue ,value);
        }

        public int FinalValue
        {
            get => _finalValue;
            set => SetProperty(ref _finalValue ,value);
        }

        public int ModificationValue
        {
            get => _modificationValue;
            set => SetProperty(ref _modificationValue , value);
        }
    }
}
