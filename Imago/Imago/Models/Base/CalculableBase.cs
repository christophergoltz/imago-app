using System;
using System.Collections.Generic;
using System.Text;
using Imago.Util;

namespace Imago.Models.Base
{
    public abstract class CalculableBase : BindableBase
    {
        private int _finalValue;

        public int FinalValue
        {
            get => _finalValue;
            set => SetProperty(ref _finalValue, value);
        }
    }
}
