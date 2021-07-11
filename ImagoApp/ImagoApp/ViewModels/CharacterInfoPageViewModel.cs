﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using ImagoApp.Application;
using ImagoApp.Application.Models;
using ImagoApp.Application.Services;
using ImagoApp.Shared.Enums;
using Xamarin.Forms;

namespace ImagoApp.ViewModels
{
    public class CharacterInfoPageViewModel : BindableBase
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
            //apply distributed to attributes
            CharacterViewModel.Character.Attributes.First(attribute => attribute.Type == AttributeType.Staerke).ExperienceBySkillGroup += AttributeExperienceDialogViewModel.Staerke.Count;
            CharacterViewModel.Character.Attributes.First(attribute => attribute.Type == AttributeType.Geschicklichkeit).ExperienceBySkillGroup += AttributeExperienceDialogViewModel.Geschicklichkeit.Count;
            CharacterViewModel.Character.Attributes.First(attribute => attribute.Type == AttributeType.Intelligenz).ExperienceBySkillGroup += AttributeExperienceDialogViewModel.Intelligenz.Count;
            CharacterViewModel.Character.Attributes.First(attribute => attribute.Type == AttributeType.Konstitution).ExperienceBySkillGroup += AttributeExperienceDialogViewModel.Konstitution.Count;
            CharacterViewModel.Character.Attributes.First(attribute => attribute.Type == AttributeType.Wahrnehmung).ExperienceBySkillGroup += AttributeExperienceDialogViewModel.Wahrnehmung.Count;
            CharacterViewModel.Character.Attributes.First(attribute => attribute.Type == AttributeType.Willenskraft).ExperienceBySkillGroup += AttributeExperienceDialogViewModel.Willenskraft.Count;
            CharacterViewModel.Character.Attributes.First(attribute => attribute.Type == AttributeType.Charisma).ExperienceBySkillGroup += AttributeExperienceDialogViewModel.Charisma.Count;
            
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

            CharacterViewModel.Character.Attributes.First(attribute => attribute.Type == AttributeType.Staerke).TotalExperience = CharacterViewModel.Character.Attributes
                .First(attribute => attribute.Type == AttributeType.Staerke).TotalExperience;
            CharacterViewModel.Character.Attributes.First(attribute => attribute.Type == AttributeType.Geschicklichkeit).TotalExperience = CharacterViewModel.Character.Attributes.First(attribute => attribute.Type == AttributeType.Geschicklichkeit).TotalExperience ;
            CharacterViewModel.Character.Attributes.First(attribute => attribute.Type == AttributeType.Intelligenz).TotalExperience = CharacterViewModel.Character.Attributes.First(attribute => attribute.Type == AttributeType.Intelligenz).TotalExperience           ;
            CharacterViewModel.Character.Attributes.First(attribute => attribute.Type == AttributeType.Konstitution).TotalExperience = CharacterViewModel.Character.Attributes.First(attribute => attribute.Type == AttributeType.Konstitution).TotalExperience         ;
            CharacterViewModel.Character.Attributes.First(attribute => attribute.Type == AttributeType.Wahrnehmung).TotalExperience = CharacterViewModel.Character.Attributes.First(attribute => attribute.Type == AttributeType.Wahrnehmung).TotalExperience           ;
            CharacterViewModel.Character.Attributes.First(attribute => attribute.Type == AttributeType.Willenskraft).TotalExperience = CharacterViewModel.Character.Attributes.First(attribute => attribute.Type == AttributeType.Willenskraft).TotalExperience         ;
            CharacterViewModel.Character.Attributes.First(attribute => attribute.Type == AttributeType.Charisma).TotalExperience = CharacterViewModel.Character.Attributes.First(attribute => attribute.Type == AttributeType.Charisma).TotalExperience;

            AttributeExperienceDialogViewModel = null;
        }));

        private ICommand _cancelOpenAttributeExperienceCommand;
        public ICommand CancelOpenAttributeExperienceCommand => _cancelOpenAttributeExperienceCommand ?? (_cancelOpenAttributeExperienceCommand = new Command(() =>
        {
            AttributeExperienceDialogViewModel = null;
        }));

        private ICommand _addNewBloodCarrierCommand;
        public ICommand AddNewBloodCarrierCommand => _addNewBloodCarrierCommand ?? (_addNewBloodCarrierCommand = new Command(() =>
        {
            //todo move to service
            CharacterViewModel.Character.BloodCarrier.Add(new BloodCarrierModel("", 0, 0, 0));
        }));

        private ICommand _removeBloodCarrierCommand;
        public ICommand RemoveBloodCarrierCommand => _removeBloodCarrierCommand ?? (_removeBloodCarrierCommand = new Command<BloodCarrierModel>(model =>
        {
            CharacterViewModel.Character.BloodCarrier.Remove(model);
        }));
        
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