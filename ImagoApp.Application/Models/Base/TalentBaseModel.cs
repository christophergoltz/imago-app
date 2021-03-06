namespace ImagoApp.Application.Models.Base
{
    public abstract class TalentBaseModel : BindableBase
    {
        private string _name;
        private bool _activeUse;
        private string _shortDescription;
        private string _description;

        public TalentBaseModel()
        {

        }

        protected TalentBaseModel(string name, string shortDescription, string description, bool activeUse)
        {
            Name = name;
            ShortDescription = shortDescription;
            Description = description;
            ActiveUse = activeUse;
        }
        
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }
        public string ShortDescription
        {
            get => _shortDescription;
            set => SetProperty(ref _shortDescription, value);
        }
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        public bool ActiveUse
        {
            get => _activeUse;
            set => SetProperty(ref _activeUse, value);
        }
    }
}