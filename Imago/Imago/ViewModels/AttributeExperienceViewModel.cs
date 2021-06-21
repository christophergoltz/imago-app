﻿
using System.Collections.Generic;
using System.Text;
using Imago.Models;
using Imago.Util;

namespace Imago.ViewModels
{
    public class AttributeExperienceViewModel : BindableBase
    {
        private readonly CharacterViewModel _characterViewModel;
        public Attribute Attribute { get; private set; }

        public AttributeExperienceViewModel(Attribute attribute, CharacterViewModel characterViewModel)
        {
            _characterViewModel = characterViewModel;
            Attribute = attribute;
        }

        public int TotalExperienceValue
        {
            get => Attribute.TotalExperience;
            set
            {
                Attribute.TotalExperience = value;
                OnPropertyChanged(nameof(TotalExperienceValue));
                _characterViewModel.UpdateNewFinalValueOfAttribute(Attribute);
            }
        }
    }
}
