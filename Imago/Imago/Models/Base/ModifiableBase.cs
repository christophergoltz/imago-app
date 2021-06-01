using System;
using System.Collections.Generic;
using System.Text;

namespace Imago.Models.Base
{
    public abstract class ModifiableBase : CalculableBase
    {
        private int _modificationValue;

        public int ModificationValue
        {
            get => _modificationValue;
            set => SetProperty(ref _modificationValue, value);
        }
    }
}
