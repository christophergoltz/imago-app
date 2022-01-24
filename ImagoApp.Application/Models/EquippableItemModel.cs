namespace ImagoApp.Application.Models
{
    public class EquippableItemModel : WeightItemModel
    {
        private bool _fight;
        private bool _adventure;

        public EquippableItemModel()
        {
        }

        public EquippableItemModel(string name, int loadValue, bool adventure, bool fight) : base(loadValue, name)
        {
            Adventure = adventure;
            Fight = fight;
        }

        public bool Fight
        {
            get => _fight;
            set => SetProperty(ref _fight, value);
        }

        public bool Adventure
        {
            get => _adventure;
            set => SetProperty(ref _adventure, value);
        }
    }
}