namespace ImagoApp.Models
{
    public abstract class ItemBase : Util.BindableBase
    {
        public ItemBase()
        {

            }

        public ItemBase(string name, int load)
        {
            Name = name;
            LoadValue = load;
        }

        private int _loadValue;
        private string _name;

        public int LoadValue
        {
            get => _loadValue;
            set => SetProperty(ref _loadValue, value);
        }

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }
    }
}
