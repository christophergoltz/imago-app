using System;
using System.Collections.Generic;
using System.Text;
using Imago.Models.Base;

namespace Imago.Models
{
    public class DurabilityItem : ItemBase
    {
        private int _durabilityValue;

        public int DurabilityValue
        {
            get => _durabilityValue;
            set => SetProperty(ref _durabilityValue, value);
        }
    }
}
