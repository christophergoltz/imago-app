using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imago.Models;
using Imago.Services;
using Imago.Util;
using Attribute = Imago.Models.Attribute;

namespace Imago.ViewModels
{
    public class CharacterInfoPageViewModel : BindableBase
    {
        private string _title;

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title , value);
        }

        public CharacterInfoPageViewModel(Character character, IAttributeService attributeService)
        {
            Title = character.Name;
            Character = character;
            AttributeViewModels = Character.Attributes.Select(attribute => new AttributeViewModel(attributeService, attribute)).ToList();
        }

        public Character Character { get; private set; }

        public List<AttributeViewModel> AttributeViewModels { get; set; }
    }
}
