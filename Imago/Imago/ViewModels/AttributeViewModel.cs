using System;
using System.Collections.Generic;
using System.Text;
using Imago.Models;
using Imago.Services;
using Imago.Util;
using Imago.ViewModels;
using Attribute = Imago.Models.Attribute;

namespace Imago.ViewModels
{
    public class AttributeViewModel : BindableBase
    {
        private readonly ICharacterService _characterService;

        public AttributeViewModel(ICharacterService characterService, Attribute attribute, Character character)
        {
            _characterService = characterService;
            Attribute = attribute;
            Character = character;
        }

        public Attribute Attribute { get; set; }
        public Character Character { get; set; }

        public int Corrosion
        {
            get => Attribute.Corrosion;
            set
            {
                _characterService.SetCorrosionValue(Attribute, value, Character);
                OnPropertyChanged(nameof(Corrosion));
            }
        }

        public int Modification
        {
            get => Attribute.ModificationValue;
            set
            {
                _characterService.SetModificationValue(Attribute, value, Character);
                OnPropertyChanged(nameof(Modification));
            }
        }
    }
}
