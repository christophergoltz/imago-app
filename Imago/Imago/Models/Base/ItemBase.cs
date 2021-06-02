using System;
using System.Collections.Generic;
using System.Text;
using Imago.Util;

namespace Imago.Models.Base
{
    public abstract class ItemBase : BindableBase
    {
        private int _loadValue;

        public int LoadValue
        {
            get => _loadValue;
            set => SetProperty(ref _loadValue, value);
        }
    }
}
