using System;
using System.Collections.Generic;
using System.Text;
using Imago.Util;

namespace Imago.Models
{
    public class BloodCarrierModel : BindableBase
    {
        private string _name;
        private int _currentCapacity;
        private int _maximumCapacity;
        private int _regeneration;

        public BloodCarrierModel(string name, int currentCapacity, int maximumCapacity, int regeneration)
        {
            Name = name;
            CurrentCapacity = currentCapacity;
            MaximumCapacity = maximumCapacity;
            Regeneration = regeneration;
        }

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public int CurrentCapacity
        {
            get => _currentCapacity;
            set => SetProperty(ref _currentCapacity, value);
        }

        public int MaximumCapacity
        {
            get => _maximumCapacity;
            set => SetProperty(ref _maximumCapacity, value);
        }

        public int Regeneration
        {
            get => _regeneration;
            set => SetProperty(ref _regeneration, value);
        }
    }
}