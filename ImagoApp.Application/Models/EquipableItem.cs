using AutoMapper.Configuration.Conventions;

namespace ImagoApp.Application.Models
{
    public class EquipableItem : ItemBase
    {
        private bool _fight;
        private bool _adventure;
        private int _loadValue;

        public EquipableItem()
        {
        }

        public EquipableItem(string name, int loadValue, bool adventure, bool fight) : base(name)
        {
            LoadValue = loadValue;
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

        public int LoadValue
        {
            get => _loadValue;
            set => SetProperty(ref _loadValue, value);
        }
    }
}