using ImagoApp.Application.Constants;
using Newtonsoft.Json;

namespace ImagoApp.Application.Models.Base
{
    public abstract class IncreasableBaseModel : ModifiableBaseModel
    {
        private int _experienceValue;
        private int _increaseValue;
        private int _experienceForNextIncreasedRequired;
        private int _leftoverExperience;

        public int ExperienceValue
        {
            get => _experienceValue;
            set => SetProperty(ref _experienceValue, value);
        }

        //todo rename to cache
        public int IncreaseValue
        {
            get => _increaseValue;
            set => SetProperty(ref _increaseValue, value);
        }

        //todo rename to cache
        public int ExperienceForNextIncreasedRequired
        {
            get => _experienceForNextIncreasedRequired;
            set => SetProperty(ref _experienceForNextIncreasedRequired , value);
        }

        //todo rename to cache
        public int LeftoverExperience
        {
            get => _leftoverExperience;
            set => SetProperty(ref _leftoverExperience , value);
        }
    }
}