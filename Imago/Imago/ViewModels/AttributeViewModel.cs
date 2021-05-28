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
        private readonly IAttributeService _attributeService;

        public AttributeViewModel(IAttributeService attributeService, Attribute attribute)
        {
            _attributeService = attributeService;
            Attribute = attribute;
        }

        public Models.Attribute Attribute { get; set; }

        public int Corrosion
        {
            get => Attribute.Corrosion;
            set
            {
                _attributeService.AddCorrosion(Attribute.Type, value - Attribute.Corrosion);
                OnPropertyChanged(nameof(Corrosion));
            }
        }
    }
}
