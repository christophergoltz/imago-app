using Imago.Util;

namespace Imago.Models
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
