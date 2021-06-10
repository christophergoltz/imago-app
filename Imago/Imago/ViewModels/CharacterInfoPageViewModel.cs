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

        public List<DerivedAttribute> DerivedAttributes { get; set; }

        public ICommand CloseOpenAttributeExperienceCommand { get; }
        public ICommand AddExperienceToAttributeCommand { get; }

        public CharacterInfoPageViewModel(Character character, ICharacterService characterService,
            IRuleRepository ruleRepository)
        {
            _ruleRepository = ruleRepository;
            Title = character.Name;
            Character = character;

            DerivedAttributes = character.DerivedAttributes
                .Where(_ => _.Type == DerivedAttributeType.Egoregenration ||
                            _.Type == DerivedAttributeType.Schadensmod ||
                            _.Type == DerivedAttributeType.Traglast)
                .ToList();

            AttributeViewModels = Character.Attributes.Select(_ => new AttributeViewModel(characterService, _, Character)).ToList();
            SpecialAttributeViewModels = Character.SpecialAttributes.Select(_ => new SpecialAttributeViewModel(characterService, _, character)).ToList();
            OpenAttributeExperienceViewModels = new ObservableCollection<OpenAttributeExperienceViewModel>();

            OpenAttributeExperienceDialogIfNeeded();

            CloseOpenAttributeExperienceCommand = new Command(() => AttributeExperienceOpen = false);
         
            AddExperienceToAttributeCommand = new Command<OpenAttributeExperienceViewModel>(viewModel =>
            {
                if (viewModel.SelectedAttribute == null)
                    return; 

                characterService.AddOneExperienceToAttribute(viewModel.SelectedAttribute, Character);
                
                OpenAttributeExperienceViewModels.Remove(viewModel);
                Character.OpenAttributeIncreases.Remove(
                    Character.OpenAttributeIncreases.First(type => type == viewModel.Source));

                if (!OpenAttributeExperienceViewModels.Any())
                    AttributeExperienceOpen = false;
            });
        }

        public void OpenAttributeExperienceDialogIfNeeded()
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
        public List<SpecialAttributeViewModel> SpecialAttributeViewModels { get; set; }

        public ObservableCollection<OpenAttributeExperienceViewModel> OpenAttributeExperienceViewModels { get; set; }
    }
}