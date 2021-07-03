using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using ImagoApp.Application.Models;
using ImagoApp.Application.Services;
using Xamarin.Forms;

namespace ImagoApp.ViewModels
{
    public class CharacterInfoPageViewModel : Util.BindableBase
    {
        private readonly IRuleService _ruleService;
        private string _title;

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private ICommand _saveOpenAttributeExperienceCommand;
        public ICommand SaveOpenAttributeExperienceCommand => _saveOpenAttributeExperienceCommand ?? (_saveOpenAttributeExperienceCommand = new Command(() =>
        {
            var resolvedAttributeExperience = AttributeExperienceDialogViewModel.Charisma.ToList();
            resolvedAttributeExperience.AddRange(AttributeExperienceDialogViewModel.Staerke);
            resolvedAttributeExperience.AddRange(AttributeExperienceDialogViewModel.Geschicklichkeit);
            resolvedAttributeExperience.AddRange(AttributeExperienceDialogViewModel.Intelligenz);
            resolvedAttributeExperience.AddRange(AttributeExperienceDialogViewModel.Konstitution);
            resolvedAttributeExperience.AddRange(AttributeExperienceDialogViewModel.Wahrnehmung);
            resolvedAttributeExperience.AddRange(AttributeExperienceDialogViewModel.Willenskraft);

            foreach (var experienceViewModel in resolvedAttributeExperience)
            {
                CharacterViewModel.Character.OpenAttributeIncreases.Remove(experienceViewModel.SourceType);
            }

            AttributeExperienceDialogViewModel = null;
        }));

        private ICommand _cancelOpenAttributeExperienceCommand;
        public ICommand CancelOpenAttributeExperienceCommand => _cancelOpenAttributeExperienceCommand ?? (_cancelOpenAttributeExperienceCommand = new Command(() =>
        {
            AttributeExperienceDialogViewModel = null;
        }));
      
        public ICommand AddExperienceToAttributeCommand { get; }

        public ICommand AddNewBloodCarrierCommand { get; set; }
        public ICommand RemoveBloodCarrierCommand { get; set; }

        private int _totalAttributeExperience;
        private AttributeExperienceDialogViewModel _attributeExperienceDialogViewModel;

        public int TotalAttributeExperience
        {
            get => _totalAttributeExperience;
            set
            {
                SetProperty(ref _totalAttributeExperience, value);
                OnPropertyChanged(nameof(AttributeExperienceBalance));
            }
        }

        public int AttributeExperienceBalance => TotalAttributeExperience - AttributeViewModels?.Sum(model => model.TotalExperienceValue) ?? 0;

        public CharacterInfoPageViewModel(CharacterViewModel characterViewModel, IRuleService ruleService)
        {
             TotalAttributeExperience = 940;
            _ruleService = ruleService;
            Title = characterViewModel.Character.Name;
            CharacterViewModel = characterViewModel;

            AttributeViewModels = characterViewModel.Character.Attributes.Select(_ => new AttributeViewModel(_, characterViewModel)).ToList();
            foreach (var vm in AttributeViewModels)
            {
                vm.PropertyChanged += (sender, args) =>
                {
                    if (args.PropertyName.Equals(nameof(AttributeViewModel.TotalExperienceValue)))
                    {
                        OnPropertyChanged(nameof(AttributeExperienceBalance));
                    }
                };
            }
            OnPropertyChanged(nameof(AttributeExperienceBalance));
            
            SpecialAttributeViewModels = characterViewModel.SpecialAttributes.Select(_ => new SpecialAttributeViewModel(characterViewModel, _)).ToList();
          
            OpenAttributeExperienceDialogIfNeeded();
            
            AddExperienceToAttributeCommand = new Command<OpenAttributeExperienceViewModel>(viewModel =>
            {
                //if (viewModel.SelectedAttribute == null)
                //    return;

                //CharacterViewModel.AddOneExperienceToAttributeBySkillGroup(viewModel.SelectedAttribute);

#warning  todo

                //OpenAttributeExperienceViewModels.Remove(viewModel);
                //characterViewModel.Character.AttributeIncreases.Remove(
                //    characterViewModel.Character.AttributeIncreases.First(type => type == viewModel.Source));

                //if (!OpenAttributeExperienceViewModels.Any())
                //    AttributeExperienceOpen = false;
            });

            AddNewBloodCarrierCommand = new Command(() =>
            {
                characterViewModel.Character.BloodCarrier.Add(new BloodCarrierModel("", 0,0,0));
            });

            RemoveBloodCarrierCommand = new Command<BloodCarrierModel>(model =>
            {
                characterViewModel.Character.BloodCarrier.Remove(model);
            });
        }

        public void OpenAttributeExperienceDialogIfNeeded()
        {
            if (CharacterViewModel.Character.OpenAttributeIncreases == null)
                return;
            
            if (!CharacterViewModel.Character.OpenAttributeIncreases.Any())
                return;

            var list = new ObservableCollection<OpenAttributeExperienceViewModel>();

            foreach (var attributeIncrease in CharacterViewModel.Character.OpenAttributeIncreases)
            {
                var affectedAttributeTypes = _ruleService.GetSkillGroupSources(attributeIncrease).Distinct().ToList();
                var affectedAttributes = CharacterViewModel.Character.Attributes.Where(attribute => affectedAttributeTypes.Contains(attribute.Type)).ToList();
                list.Add(new OpenAttributeExperienceViewModel(attributeIncrease, affectedAttributes));
            }

            AttributeExperienceDialogViewModel = new AttributeExperienceDialogViewModel(list);
        }

        public CharacterViewModel CharacterViewModel { get; private set; }

        public List<AttributeViewModel> AttributeViewModels { get; set; }
        public List<SpecialAttributeViewModel> SpecialAttributeViewModels { get; set; }

        public AttributeExperienceDialogViewModel AttributeExperienceDialogViewModel
        {
            get => _attributeExperienceDialogViewModel;
            set => SetProperty(ref _attributeExperienceDialogViewModel,value);
        }
    }
}