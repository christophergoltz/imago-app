using System;
using System.Collections.Generic;
using System.Text;
using Imago.Util;

namespace Imago.Models.Base
{
    public abstract class SkillBase : BindableBase
    {
        private int _naturalValue;
        private int _modificationValue;
       // protected int _faktischerWert;
        
        public int NaturalValue
        {
            get => _naturalValue;
            set => SetProperty(ref _naturalValue ,value);
        }


        //public int FaktischerWert
        //{
        //    get
        //    {
        //        return _faktischerWert;
        //    }
        //    set
        //    {
        //        _faktischerWert = value;
        //        OnPropertyChanged();
        //    }
        //}

        public int ModificationValue
        {
            get => _modificationValue;
            set => SetProperty(ref _modificationValue , value);
        }
    }
}
