﻿namespace ImagoApp.Application.Models.Base
{
    public abstract class DependentBaseModel : IncreasableBaseModel
    {
        private int _baseValue;
        
        public int BaseValue
        {
            get => _baseValue;
            set => SetProperty(ref _baseValue, value);
        }
    }
}
