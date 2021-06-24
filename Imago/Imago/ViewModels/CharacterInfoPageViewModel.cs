﻿using System;
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

        public ICommand AddNewBloodCarrierCommand { get; set; }
        public ICommand RemoveBloodCarrierCommand { get; set; }

        private int _totalAttributeExperience;
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

        public CharacterInfoPageViewModel(CharacterViewModel characterViewModel, IRuleRepository ruleRepository)
        {
             TotalAttributeExperience = 940;
            _ruleRepository = ruleRepository;
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
            OpenAttributeExperienceViewModels = new ObservableCollection<OpenAttributeExperienceViewModel>();

            OpenAttributeExperienceDialogIfNeeded();

            CloseOpenAttributeExperienceCommand = new Command(() => AttributeExperienceOpen = false);
         
            AddExperienceToAttributeCommand = new Command<OpenAttributeExperienceViewModel>(viewModel =>
            {
                if (viewModel.SelectedAttribute == null)
                    return;

                CharacterViewModel.AddOneExperienceToAttributeBySkillGroup(viewModel.SelectedAttribute);
             
                OpenAttributeExperienceViewModels.Remove(viewModel);
                characterViewModel.Character.OpenAttributeIncreases.Remove(
                    characterViewModel.Character.OpenAttributeIncreases.First(type => type == viewModel.Source));

                if (!OpenAttributeExperienceViewModels.Any())
                    AttributeExperienceOpen = false;
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
            OpenAttributeExperienceViewModels.Clear();

            foreach (var attributeIncrease in CharacterViewModel.Character.OpenAttributeIncreases)
            {
                var affectedAttributeTypes = _ruleRepository.GetSkillGroupSources(attributeIncrease).Distinct().ToList();
                var affectedAttributes = CharacterViewModel.Character.Attributes.Where(attribute => affectedAttributeTypes.Contains(attribute.Type)).ToList();
                OpenAttributeExperienceViewModels.Add(new OpenAttributeExperienceViewModel(attributeIncrease, affectedAttributes));
            }

            if (OpenAttributeExperienceViewModels.Any())
                AttributeExperienceOpen = true;
        }

        public CharacterViewModel CharacterViewModel { get; private set; }

        public List<AttributeViewModel> AttributeViewModels { get; set; }
        public List<SpecialAttributeViewModel> SpecialAttributeViewModels { get; set; }

        public ObservableCollection<OpenAttributeExperienceViewModel> OpenAttributeExperienceViewModels { get; set; }
    }
}