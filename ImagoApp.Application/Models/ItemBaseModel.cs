namespace ImagoApp.Application.Models
{
    public abstract class ItemBaseModel : BindableBase
    {
        public ItemBaseModel()
        {

        }

        public ItemBaseModel(string name)
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
