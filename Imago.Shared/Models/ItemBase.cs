using Imago.Shared.Util;

namespace Imago.Shared.Models
{
    public abstract class ItemBase : BindableBase
    {
        public ItemBase()
        {
            
        }

        private int _loadValue;

        public int LoadValue
        {
            get => _loadValue;
            set => SetProperty(ref _loadValue, value);
        }
    }
}
