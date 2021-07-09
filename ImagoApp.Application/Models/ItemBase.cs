using ImagoApp.Util;

namespace ImagoApp.Application.Models
{
    public abstract class ItemBase : BindableBase
    {
        public ItemBase()
        {

        }

        public ItemBase(string name)
        {
            Name = name;
        }

        private string _name;

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }
    }
}
