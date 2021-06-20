using System;
using System.Collections.Generic;
using System.Text;
using Imago.Util;
using Newtonsoft.Json;

namespace Imago.Models.Base
{
    public abstract class CalculableBase : BindableBase
    {
        private double _finalValue;

        [JsonIgnore]
        public double FinalValue
        {
            get => _finalValue;
            set => SetProperty(ref _finalValue, value);
        }
    }
}
