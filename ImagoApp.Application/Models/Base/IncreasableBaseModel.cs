namespace ImagoApp.Application.Models.Base
{
    public abstract class IncreasableBaseModel : ModifiableBaseModel
    {
        private int _experienceValue;
        private int _increaseValueCache;
        private int _experienceForNextIncreasedRequiredCache;
        private int _leftoverExperienceCache;

        public int ExperienceValue
        {
            get => _experienceValue;
            set => SetProperty(ref _experienceValue, value);
        }
        
        public int IncreaseValueCache
        {
            get => _increaseValueCache;
            set => SetProperty(ref _increaseValueCache, value);
        }
        
        public int ExperienceForNextIncreasedRequiredCache
        {
            get => _experienceForNextIncreasedRequiredCache;
            set => SetProperty(ref _experienceForNextIncreasedRequiredCache , value);
        }
        
        public int LeftoverExperienceCache
        {
            get => _leftoverExperienceCache;
            set => SetProperty(ref _leftoverExperienceCache , value);
        }
    }
}