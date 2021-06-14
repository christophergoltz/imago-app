namespace Imago.Models
{
    public class ArmorModel : DurabilityItem
    {
        public ArmorModel()
        {
            
        }

        public ArmorModel(string name, int physicalDefense, int energyDefense, int loadValue, int durability) : base(durability)
        {
            Name = name;
            PhysicalDefense = physicalDefense;
            EnergyDefense = energyDefense;
            LoadValue = loadValue;
        }

        private bool _fight;
        private bool _adventure;
        private int _physicalDefense;
        private int _energyDefense;
        private string _name;

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

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
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
