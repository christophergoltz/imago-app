using System;
using System.Collections.Generic;
using System.Text;
using Imago.Shared.Util;
using Imago.Util;

namespace Imago.Models.Base
{
    public abstract class CalculableBase : BindableBase
    {
        private double _finalValue;

        public double FinalValue
        {
            get => _finalValue;
            set => SetProperty(ref _finalValue, value);
        }
    }
}
