using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Imago.Models.Base
{
    public abstract class DependentBase : IncreasableBase
    {
        private int _baseValue;

        [JsonIgnore]
        public int BaseValue
        {
            get => _baseValue;
            set => SetProperty(ref _baseValue, value);
        }
    }
}
