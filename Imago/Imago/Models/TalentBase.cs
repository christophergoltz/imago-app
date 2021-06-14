using System;
using System.Collections.Generic;
using System.Text;
using Imago.Util;

namespace Imago.Models
{
    public abstract class TalentBase : BindableBase
    {
        private string _name;
        private bool _activeUse;
        private int? _difficulty;
        private string _shortDescription;

        public TalentBase()
        {
            
        }

        protected TalentBase(string name, string shortDescription, bool activeUse, int? difficulty)
        {
            Name = name;
            ShortDescription = shortDescription;
            ActiveUse = activeUse;
            Difficulty = difficulty;
        }

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name , value);
        }

        public string ShortDescription
        {
            get => _shortDescription;
            set => SetProperty(ref _shortDescription, value);
        }

        public bool ActiveUse
        {
            get => _activeUse;
            set => SetProperty(ref _activeUse , value);
        }

        public int? Difficulty
        {
            get => _difficulty;
            set => SetProperty(ref _difficulty, value);
        }
    }
}
