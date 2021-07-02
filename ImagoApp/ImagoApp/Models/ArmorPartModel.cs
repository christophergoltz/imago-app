namespace ImagoApp.Models
{
    public class ArmorPartModel : DurabilityItem
    {
        public ArmorPartModel()
        {
            
        }

        public ArmorPartModel(Enum.ArmorPartType armorPartType, string name, int loadValue, bool fight, bool adventure, int durability, int energyDefense, int physicalDefense) 
            : base(name, loadValue, fight, adventure, durability)
        {
            ArmorPartType = armorPartType;
            PhysicalDefense = physicalDefense;
            EnergyDefense = energyDefense;
        }
        
        private int _physicalDefense;
        private int _energyDefense;
        private Enum.ArmorPartType _armorPartType;

        public int PhysicalDefense
        {
            get => _physicalDefense;
            set => SetProperty(ref _physicalDefense, value);
        }

        public int EnergyDefense
        {
            get => _energyDefense;
            set => SetProperty(ref _energyDefense, value);
        }

        public Enum.ArmorPartType ArmorPartType
        {
            get => _armorPartType;
            set => SetProperty(ref _armorPartType, value);
        }
    }
}
