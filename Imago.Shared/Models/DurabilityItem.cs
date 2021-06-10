namespace Imago.Shared.Models
{
    public class DurabilityItem : ItemBase
    {
        public DurabilityItem(int durability)
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
