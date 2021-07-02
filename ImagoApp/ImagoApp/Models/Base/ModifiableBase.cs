namespace ImagoApp.Models.Base
{
    public abstract class ModifiableBase : CalculableBase
    {
        private int _modificationValue;

        public int ModificationValue
        {
            get => _modificationValue;
            set => SetProperty(ref _modificationValue, value);
        }
    }
}
