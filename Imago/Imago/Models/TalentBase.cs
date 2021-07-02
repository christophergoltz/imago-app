﻿using System;
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
        private string _phaseValueMod;
        private string _description;

        public TalentBase()
        {

        }

        protected TalentBase(string name, string shortDescription, string description, bool activeUse, int? difficulty,
            string phaseValueMod)
        {
            Name = name;
            ShortDescription = shortDescription;
            Description = description;
            ActiveUse = activeUse;
            Difficulty = difficulty;
            PhaseValueMod = phaseValueMod;
        }

        public string PhaseValueMod
        {
            get => _phaseValueMod;
            set => SetProperty(ref _phaseValueMod, value);
        }

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public string ShortDescription
        {
            get => _shortDescription;
            set => SetProperty(ref _shortDescription, value);
        }

        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        public bool ActiveUse
        {
            get => _activeUse;
            set => SetProperty(ref _activeUse, value);
        }

        public int? Difficulty
        {
            get => _difficulty;
            set => SetProperty(ref _difficulty, value);
        }
    }
}