namespace ImagoApp.Application.Models
{
    public class BloodCarrierModel : ItemBase
    {
        private int _currentCapacity;
        private int _maximumCapacity;
        private int _regeneration;

        public BloodCarrierModel(string name, int currentCapacity, int maximumCapacity, int regeneration) : base(name)
        {
            CurrentCapacity = currentCapacity;
            MaximumCapacity = maximumCapacity;
            Regeneration = regeneration;
        }
        
        public int CurrentCapacity
        {
            get => _currentCapacity;
            set => SetProperty(ref _currentCapacity, value);
        }

        public int MaximumCapacity
        {
            get => _maximumCapacity;
            set => SetProperty(ref _maximumCapacity, value);
        }

        public int Regeneration
        {
            get => _regeneration;
            set => SetProperty(ref _regeneration, value);
        }
    }
}