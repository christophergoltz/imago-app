using System;
using System.Collections.Generic;
using System.Text;
using Imago.Services;
using Imago.Util;
using Imago.ViewModels;
using Attribute = Imago.Models.Attribute;

namespace Imago.ViewModels
{
    public class AttributeViewModel : BindableBase
    {
        private readonly ICharacterService _characterService;

        public AttributeViewModel(ICharacterService characterService, Attribute attribute)
        {
            _characterService = characterService;
            Attribute = attribute;
        }

        public Attribute Attribute { get; set; }

        public int Corrosion
        {
            get => Attribute.Corrosion;
            set
            {
                _characterService.AddCorrosion(Attribute, value - Attribute.Corrosion);
                OnPropertyChanged(nameof(Corrosion));
            }
        }
    }
}
