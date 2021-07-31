using System;
using System.Collections.Generic;
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
        private string _title;

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }
        
        private ICommand _cancelOpenAttributeExperienceCommand;
        public ICommand CancelOpenAttributeExperienceCommand => _cancelOpenAttributeExperienceCommand ?? (_cancelOpenAttributeExperienceCommand = new Command(() =>
        {
            IsAttributeExperienceDialogOpen = false;
        }));

        private ICommand _addNewBloodCarrierCommand;
        public ICommand AddNewBloodCarrierCommand => _addNewBloodCarrierCommand ?? (_addNewBloodCarrierCommand = new Command(() =>
        {
            try
            {
                //todo move to service
                CharacterViewModel.CharacterModel.BloodCarrier.Add(new BloodCarrierModel("", 0, 0, 0));
            }
            catch (Exception e)
            {
                App.ErrorManager.TrackException(e, CharacterViewModel.CharacterModel.Name);
            }
        }));

        private ICommand _removeBloodCarrierCommand;
        public ICommand RemoveBloodCarrierCommand => _removeBloodCarrierCommand ?? (_removeBloodCarrierCommand = new Command<BloodCarrierModel>(model =>
        {
            try
            {
                CharacterViewModel.CharacterModel.BloodCarrier.Remove(model);
            }
            catch (Exception e)
            {
                App.ErrorManager.TrackException(e, CharacterViewModel.CharacterModel.Name);
            }
        }));
        
        private bool _isAttributeExperienceDialogOpen;
        public int TotalAttributeExperience
        {
            get => CharacterViewModel.CharacterModel.CharacterCreationAttributePoints;
            set
            {
                CharacterViewModel.CharacterModel.CharacterCreationAttributePoints = value;
                OnPropertyChanged(nameof(AttributeExperienceBalance));
            }
        }

        public int AttributeExperienceBalance => TotalAttributeExperience - AttributeViewModels?.Sum(model => model.TotalExperienceValue) ?? 0;

        public CharacterInfoPageViewModel(CharacterViewModel characterViewModel)
        {
            Title = characterViewModel.CharacterModel.Name;
            CharacterViewModel = characterViewModel;

            AttributeViewModels = characterViewModel.CharacterModel.Attributes.Select(_ => new AttributeViewModel(_, characterViewModel)).ToList();
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

        public bool IsAttributeExperienceDialogOpen
        {
            get => _isAttributeExperienceDialogOpen;
            set => SetProperty(ref _isAttributeExperienceDialogOpen, value);
        }

        public void OpenAttributeExperienceDialogIfNeeded()
        {
            if (CharacterViewModel.CharacterModel.OpenAttributeIncreases == null)
                return;
            
            if (!CharacterViewModel.CharacterModel.OpenAttributeIncreases.Any())
                return;

            IsAttributeExperienceDialogOpen = true;
        }

        public CharacterViewModel CharacterViewModel { get; private set; }

        public List<AttributeViewModel> AttributeViewModels { get; set; }
        public List<SpecialAttributeViewModel> SpecialAttributeViewModels { get; set; }
    }
}