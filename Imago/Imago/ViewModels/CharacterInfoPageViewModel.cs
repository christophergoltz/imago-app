using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Imago.Models;
using Imago.Models.Enum;
using Imago.Repository;
using Imago.Services;
using Imago.Util;
using Xamarin.Forms;
using Attribute = Imago.Models.Attribute;

namespace Imago.ViewModels
{
    public class CharacterInfoPageViewModel : BindableBase
    {
        private readonly IRuleRepository _ruleRepository;
        private string _title;
        private bool _attributeExperienceOpen;

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public bool AttributeExperienceOpen
        {
            get => _attributeExperienceOpen;
            set => SetProperty(ref _attributeExperienceOpen, value);
        }

        public ICommand CloseOpenAttributeExperienceCommand { get; }
        public ICommand AddExperienceToAttributeCommand { get; }

        public CharacterInfoPageViewModel(Character character, ICharacterService characterService,
            IRuleRepository ruleRepository)
        {
            _ruleRepository = ruleRepository;
            Title = character.Name;
            Character = character;
            Character.OpenAttributeIncreases.CollectionChanged += OpenAttributeIncreases_CollectionChanged;

            AttributeViewModels = Character.Attributes
                .Select(attribute => new AttributeViewModel(characterService, attribute, Character)).ToList();
            OpenAttributeExperienceViewModels = new ObservableCollection<OpenAttributeExperienceViewModel>();

            CheckForOpenAttributeExperience();

            CloseOpenAttributeExperienceCommand = new Command(() => AttributeExperienceOpen = false);
         
            AddExperienceToAttributeCommand = new Command<OpenAttributeExperienceViewModel>(viewModel =>
            {
                if (viewModel.SelectedAttribute == null)
                    return; 

                characterService.AddOneExperienceToAttribute(viewModel.SelectedAttribute, Character.Attributes,Character.SkillGroups);
                
                OpenAttributeExperienceViewModels.Remove(viewModel);
                Character.OpenAttributeIncreases.Remove(
                    Character.OpenAttributeIncreases.First(type => type == viewModel.Source));

                if (!OpenAttributeExperienceViewModels.Any())
                    AttributeExperienceOpen = false;
            });
        }

        private void OpenAttributeIncreases_CollectionChanged(object sender,
            System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            CheckForOpenAttributeExperience();
        }

        private void CheckForOpenAttributeExperience()
        {
            OpenAttributeExperienceViewModels.Clear();

            foreach (var attributeIncrease in Character.OpenAttributeIncreases)
            {
                var affectedAttributeTypes = _ruleRepository.GetSkillGroupSources(attributeIncrease).Distinct().ToList();
                var affectedAttributes = Character.Attributes.Where(attribute => affectedAttributeTypes.Contains(attribute.Type)).ToList();
                OpenAttributeExperienceViewModels.Add(new OpenAttributeExperienceViewModel(attributeIncrease, affectedAttributes));
            }

            if (OpenAttributeExperienceViewModels.Any())
                AttributeExperienceOpen = true;
        }

        public Character Character { get; private set; }

        public List<AttributeViewModel> AttributeViewModels { get; set; }

        public ObservableCollection<OpenAttributeExperienceViewModel> OpenAttributeExperienceViewModels { get; set; }
    }
}