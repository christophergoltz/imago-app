namespace ImagoApp.Models
{
    public abstract class DurabilityItem : EquipableItem
    {
        public DurabilityItem(string name,int load, bool fight, bool adventure, int durability) : base(name, load, adventure, fight)
        {
            DurabilityValue = durability;
        }

        public DurabilityItem()
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
