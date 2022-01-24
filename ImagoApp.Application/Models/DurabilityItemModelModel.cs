namespace ImagoApp.Application.Models
{
    public abstract class DurabilityItemModelModel : EquippableItemModel
    {
        public DurabilityItemModelModel(string name,int load, bool fight, bool adventure, int durability) : base(name, load, adventure, fight)
        {
            DurabilityValue = durability;
        }

        public DurabilityItemModelModel()
        {
            
        }

        private int _durabilityValue;

        public int DurabilityValue
        {
            get => _durabilityValue;
            set => SetProperty(ref _durabilityValue, value);
        }
    }
}
