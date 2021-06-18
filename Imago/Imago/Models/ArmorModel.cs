namespace Imago.Models
{
    public class ArmorModel : DurabilityItem
    {
        public ArmorModel()
        {
            
        }

        public ArmorModel(string name, int loadValue, bool fight, bool adventure, int durability, int energyDefense, int physicalDefense) 
            : base(name, loadValue, fight, adventure, durability)
        {
            PhysicalDefense = physicalDefense;
            EnergyDefense = energyDefense;
        }
        
        private int _physicalDefense;
        private int _energyDefense;

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
    }
}
