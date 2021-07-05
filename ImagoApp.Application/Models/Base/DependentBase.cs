﻿using Newtonsoft.Json;

namespace ImagoApp.Application.Models.Base
{
    public abstract class DependentBase : IncreasableBase
    {
        private int _baseValue;
        
        public int BaseValue
        {
            get => _baseValue;
            set => SetProperty(ref _baseValue, value);
        }
    }
}
