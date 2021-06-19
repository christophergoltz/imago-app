using System.Collections.Generic;
using System.Text;
using Imago.Models;
using Imago.Services;
using Imago.Util;

namespace Imago.ViewModels
{
    public class SpecialAttributeViewModel : BindableBase
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
