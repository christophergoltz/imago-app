namespace ImagoApp.Application.Models.Base
{
    public abstract class ModifiableBaseModel : CalculableBaseModel
    {
        private int _modificationValue;

        public int ModificationValue
        {
            get => _modificationValue;
            set => SetProperty(ref _modificationValue, value);
        }
    }
}
