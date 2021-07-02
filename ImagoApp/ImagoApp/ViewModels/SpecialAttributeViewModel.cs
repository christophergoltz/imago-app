namespace ImagoApp.ViewModels
{
    public class SpecialAttributeViewModel : Util.BindableBase
    {
        private readonly CharacterViewModel _characterViewModel;

        public SpecialAttributeViewModel(CharacterViewModel characterViewModel, Models.SpecialAttribute specialAttribute)
        {
            _characterViewModel = characterViewModel;
            SpecialAttribute = specialAttribute;
        }

        public Models.SpecialAttribute SpecialAttribute { get; set; }
        public Models.Character Character { get; set; }
        
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
