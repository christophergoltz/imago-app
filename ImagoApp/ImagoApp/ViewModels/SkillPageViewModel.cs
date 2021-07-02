using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace ImagoApp.ViewModels
{
    public class SkillPageViewModel : Util.BindableBase
    {
        public CharacterViewModel CharacterViewModel { get; }
        private SkillGroupDetailViewModel _skillGroupDetailViewModel;
        private SkillDetailViewModel _skillDetailViewModel;
        
        public SkillGroupViewModel Bewegung => new SkillGroupViewModel(CharacterViewModel.Character.SkillGroups[Models.Enum.SkillGroupModelType.Bewegung], CharacterViewModel);
        public SkillGroupViewModel Nahkampf => new SkillGroupViewModel(CharacterViewModel.Character.SkillGroups[Models.Enum.SkillGroupModelType.Nahkampf], CharacterViewModel);
        public SkillGroupViewModel Heimlichkeit => new SkillGroupViewModel(CharacterViewModel.Character.SkillGroups[Models.Enum.SkillGroupModelType.Heimlichkeit], CharacterViewModel);
        public SkillGroupViewModel Fernkampf => new SkillGroupViewModel(CharacterViewModel.Character.SkillGroups[Models.Enum.SkillGroupModelType.Fernkampf], CharacterViewModel);
        public SkillGroupViewModel Webkunst => new SkillGroupViewModel(CharacterViewModel.Character.SkillGroups[Models.Enum.SkillGroupModelType.Webkunst], CharacterViewModel);
        public SkillGroupViewModel Wissenschaft => new SkillGroupViewModel(CharacterViewModel.Character.SkillGroups[Models.Enum.SkillGroupModelType.Wissenschaft], CharacterViewModel);
        public SkillGroupViewModel Handwerk => new SkillGroupViewModel(CharacterViewModel.Character.SkillGroups[Models.Enum.SkillGroupModelType.Handwerk], CharacterViewModel);
        public SkillGroupViewModel Soziales => new SkillGroupViewModel(CharacterViewModel.Character.SkillGroups[Models.Enum.SkillGroupModelType.Soziales], CharacterViewModel);

        public ICommand OpenSkillDetailCommand { get; set; }
        public ICommand OpenSkillGroupDetailCommand { get; set; }

        public SkillDetailViewModel SkillDetailViewModel
        {
            get => _skillDetailViewModel;
            set => SetProperty(ref _skillDetailViewModel, value);
        }

        public SkillGroupDetailViewModel SkillGroupDetailViewModel
        {
            get => _skillGroupDetailViewModel;
            set => SetProperty(ref _skillGroupDetailViewModel, value);
        }

        private int _totalSkillExperience;
        public int TotalSkillExperience
        {
            get => _totalSkillExperience;
            set
            {
                SetProperty(ref _totalSkillExperience, value);
                OnPropertyChanged(nameof(SkillExperienceBalance));
            }
        }
        public int SkillExperienceBalance => TotalSkillExperience - CharacterViewModel.Character.SkillGroups.Values.SelectMany(model => model.Skills).Sum(model => model.TotalExperience);
        
        public SkillPageViewModel(CharacterViewModel characterViewModel, Services.IWikiService wikiService, 
            Repository.WrappingDatabase.IMasteryRepository masteryRepository,
            Repository.WrappingDatabase.ITalentRepository talentRepository,
            Repository.IRuleRepository ruleRepository)
        {
            CharacterViewModel = characterViewModel;
            TotalSkillExperience = 1350;

            foreach (var skill in characterViewModel.Character.SkillGroups.Values.SelectMany(model => model.Skills))
            {
                skill.PropertyChanged += (sender, args) =>
                {
                    if (args.PropertyName.Equals(nameof(Models.SkillModel.TotalExperience)))
                    {
                        OnPropertyChanged(nameof(SkillExperienceBalance));
                    }
                };
            }

            OpenSkillDetailCommand = new Command<(Models.SkillModel Skill, Models.SkillGroupModel SkillGroup)>(parameter =>
            {
                var vm = new SkillDetailViewModel(parameter.Skill, parameter.SkillGroup, characterViewModel,
                    wikiService, masteryRepository, talentRepository, ruleRepository);
                vm.CloseRequested += (sender, args) => { SkillDetailViewModel = null; };

                SkillDetailViewModel = vm;
            });

            OpenSkillGroupDetailCommand = new Command<Models.SkillGroupModel>(group =>
            {
                var vm = new SkillGroupDetailViewModel(group, characterViewModel, wikiService);
                vm.CloseRequested += (sender, args) => { SkillGroupDetailViewModel = null; };

                SkillGroupDetailViewModel = vm;
            });
        }
    }
}