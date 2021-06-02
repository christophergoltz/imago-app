using System;
using System.Collections.Generic;
using System.Text;

namespace Imago.Models.Base
{
    public abstract class DependentBase : ModifiableBase
    {
        private int _baseValue;

        public int BaseValue
        {
            get => _baseValue;
            set => SetProperty(ref _baseValue, value);
        }
    }
}
