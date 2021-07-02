using System.Collections.ObjectModel;

namespace ImagoApp.ViewModels
{
    public class AttributeExperienceDialogViewModel : Util.BindableBase
    {
        private ObservableCollection<OpenAttributeExperienceViewModel> _openAttributeExperience;

        public ObservableCollection<OpenAttributeExperienceViewModel> OpenAttributeExperience
        {
            get => _openAttributeExperience;
            set => SetProperty(ref _openAttributeExperience, value);
        }

        public ObservableCollection<OpenAttributeExperienceViewModel> Staerke { get; set; }
        public ObservableCollection<OpenAttributeExperienceViewModel> Charisma { get; set; }
        public ObservableCollection<OpenAttributeExperienceViewModel> Geschicklichkeit { get; set; }
        public ObservableCollection<OpenAttributeExperienceViewModel> Intelligenz { get; set; }
        public ObservableCollection<OpenAttributeExperienceViewModel> Konstitution { get; set; }
        public ObservableCollection<OpenAttributeExperienceViewModel> Willenskraft { get; set; }
        public ObservableCollection<OpenAttributeExperienceViewModel> Wahrnehmung { get; set; }
        
        public AttributeExperienceDialogViewModel(ObservableCollection<OpenAttributeExperienceViewModel> openAttributeExperience)
        {
            OpenAttributeExperience = openAttributeExperience;
            Staerke = new ObservableCollection<OpenAttributeExperienceViewModel>();
            Charisma = new ObservableCollection<OpenAttributeExperienceViewModel>();
            Geschicklichkeit = new ObservableCollection<OpenAttributeExperienceViewModel>();
            Intelligenz = new ObservableCollection<OpenAttributeExperienceViewModel>();
            Konstitution = new ObservableCollection<OpenAttributeExperienceViewModel>();
            Willenskraft = new ObservableCollection<OpenAttributeExperienceViewModel>();
            Wahrnehmung = new ObservableCollection<OpenAttributeExperienceViewModel>();
        }
    }
}
