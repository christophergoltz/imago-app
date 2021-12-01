using ImagoApp.Application.Services;
using ImagoApp.Shared.Enums;

namespace ImagoApp.Application.Models.Base
{
    public abstract class CreationExperienceBaseModel : DependentBaseModel
    {
        private int _creationExperience;

        public CreationExperienceBaseModel(IncreaseType increaseType) : base(increaseType)
        {
            
        }
        
        public int CreationExperience
        {
            get => _creationExperience;
            set
            {
                SetProperty(ref _creationExperience, value);
                OnPropertyChanged(nameof(IncreaseValueCache));
                OnPropertyChanged(nameof(LeftoverExperienceCache));
                OnPropertyChanged(nameof(ExperienceForNextIncreasedRequiredCache));
            }
        }
    }
}
