namespace ImagoApp.Application.Models.Base
{
    public class CreationExperienceBaseModel : DependentBaseModel
    {
        private int _creationExperience;

        public CreationExperienceBaseModel()
        {
            
        }

        public int CreationExperience
        {
            get => _creationExperience;
            set => SetProperty(ref _creationExperience, value);
        }
    }
}
