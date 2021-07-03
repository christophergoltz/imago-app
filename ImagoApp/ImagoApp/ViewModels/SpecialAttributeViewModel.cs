﻿using ImagoApp.Application.Models;

namespace ImagoApp.ViewModels
{
    public class SpecialAttributeViewModel : Util.BindableBase
    {
        private readonly CharacterViewModel _characterViewModel;

        public SpecialAttributeViewModel(CharacterViewModel characterViewModel, SpecialAttribute specialAttribute)
        {
            _characterViewModel = characterViewModel;
            SpecialAttribute = specialAttribute;
        }

        public SpecialAttribute SpecialAttribute { get; set; }
        public Character Character { get; set; }
        
        public int Modification
        {
            get => SpecialAttribute.ModificationValue;
            set
            {
                _characterViewModel.SetModificationValue(SpecialAttribute, value);
                OnPropertyChanged(nameof(Modification));
            }
        }
    }
}
