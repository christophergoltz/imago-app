using System.Collections.Generic;
using System.Text;
using Imago.Models;
using Imago.Services;
using Imago.Shared.Util;
using Imago.Util;

namespace Imago.ViewModels
{
    public class SpecialAttributeViewModel : BindableBase
    {
        private readonly ICharacterService _characterService;

        public SpecialAttributeViewModel(ICharacterService characterService, SpecialAttribute specialAttribute, Character character)
        {
            _characterService = characterService;
            SpecialAttribute = specialAttribute;
            Character = character;
        }

        public SpecialAttribute SpecialAttribute { get; set; }
        public Character Character { get; set; }
        
        public int Modification
        {
            get => SpecialAttribute.ModificationValue;
            set
            {
                _characterService.SetModificationValue(SpecialAttribute, value, Character);
                OnPropertyChanged(nameof(Modification));
            }
        }
    }
}
