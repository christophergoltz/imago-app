using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using ImagoApp.Application;
using ImagoApp.Application.Models;
using ImagoApp.Application.Services;
using ImagoApp.Shared.Enums;
using Xamarin.Forms;

namespace ImagoApp.ViewModels.Page
{
    public class EquipmentPageViewModel : BindableBase
    {
        public CharacterViewModel CharacterViewModel { get; private set; }

        private int _selectedTabIndex;

        public EquipmentPageViewModel(CharacterViewModel characterViewModel, IWikiDataService wikiDataService)
        {
            CharacterViewModel = characterViewModel;
            
            KopfViewModel = new BodyPartArmorListViewModel(characterViewModel, wikiDataService, characterViewModel.CharacterModel.BodyParts.First(_ => _.Type == BodyPartType.Kopf));
            TorsoViewModel = new BodyPartArmorListViewModel(characterViewModel, wikiDataService, characterViewModel.CharacterModel.BodyParts.First(_ => _.Type == BodyPartType.Torso));
            ArmLinksViewModel = new BodyPartArmorListViewModel(characterViewModel, wikiDataService, characterViewModel.CharacterModel.BodyParts.First(_ => _.Type == BodyPartType.ArmLinks));
            ArmRechtsViewModel = new BodyPartArmorListViewModel(characterViewModel, wikiDataService, characterViewModel.CharacterModel.BodyParts.First(_ => _.Type == BodyPartType.ArmRechts));
            BeinLinksViewModel = new BodyPartArmorListViewModel(characterViewModel, wikiDataService, characterViewModel.CharacterModel.BodyParts.First(_ => _.Type == BodyPartType.BeinLinks));
            BeinRechtsViewModel = new BodyPartArmorListViewModel(characterViewModel, wikiDataService, characterViewModel.CharacterModel.BodyParts.First(_ => _.Type == BodyPartType.BeinRechts));

            WeaponListViewModel = new WeaponListViewModel(characterViewModel, wikiDataService);
            WeaponListViewModel.OpenWikiPageRequested += (sender, url) =>
            {
                OpenWikiPageRequested?.Invoke(sender, url);
            };

            EquippableItemViewModels = new ObservableCollection<EquippableItemViewModel>(
                characterViewModel.CharacterModel.EquippedItems.Select(item => new EquippableItemViewModel(item, characterViewModel)));
        }

        public int SelectedTabIndex
        {
            get => _selectedTabIndex;
            set => SetProperty(ref _selectedTabIndex, value);
        }
        
        public BodyPartArmorListViewModel KopfViewModel { get; set; }
        public BodyPartArmorListViewModel TorsoViewModel { get; set; }
        public BodyPartArmorListViewModel ArmLinksViewModel { get; set; }
        public BodyPartArmorListViewModel ArmRechtsViewModel { get; set; }
        public BodyPartArmorListViewModel BeinLinksViewModel { get; set; }
        public BodyPartArmorListViewModel BeinRechtsViewModel { get; set; }

        private TreatmentDetailViewModel _treatmentDetailViewModel;
        public TreatmentDetailViewModel TreatmentDetailViewModel
        {
            get => _treatmentDetailViewModel;
            set => SetProperty(ref _treatmentDetailViewModel, value);
        }

        private HealingDetailViewModel _healingDetailViewModel;
        public HealingDetailViewModel HealingDetailViewModel
        {
            get => _healingDetailViewModel;
            set => SetProperty(ref _healingDetailViewModel, value);
        }

        private ICommand _openTreatmentCommand;

        public ICommand OpenTreamentCommand => _openTreatmentCommand ?? (_openTreatmentCommand = new Command(() =>
        {
            try
            {
                var vm = new TreatmentDetailViewModel(CharacterViewModel);
                vm.CloseRequested += (sender, args) => TreatmentDetailViewModel = null;
                TreatmentDetailViewModel = vm;
            }
            catch (Exception exception)
            {
                App.ErrorManager.TrackException(exception, CharacterViewModel.CharacterModel.Name);
            }
        }));

        private ICommand _openHealingCommand;

        public ICommand OpenHealingCommand => _openHealingCommand ?? (_openHealingCommand = new Command(() =>
        {
            try
            {
                var vm = new HealingDetailViewModel(CharacterViewModel);
                vm.CloseRequested += (sender, args) => HealingDetailViewModel = null;
                HealingDetailViewModel = vm;
            }
            catch (Exception exception)
            {
                App.ErrorManager.TrackException(exception, CharacterViewModel.CharacterModel.Name);
            }
        }));

        public WeaponListViewModel WeaponListViewModel { get; set; }

        private ICommand _openWeaponCommand;


        public ICommand OpenWeaponCommand => _openWeaponCommand ?? (_openWeaponCommand = new Command<WeaponModel>(weapon =>
        {
            try
            {
                var vm = new WeaponDetailViewModel(weapon, CharacterViewModel);
                vm.CloseRequested += (sender, args) => WeaponDetailViewModel = null;
                vm.RemoveWeaponRequested += (sender, args) =>
                {
                    CharacterViewModel.CharacterModel.Weapons.Remove(weapon);
                    CharacterViewModel.RecalculateHandicapAttributes();
                    WeaponDetailViewModel = null;
                };
                WeaponDetailViewModel = vm;
            }
            catch (Exception exception)
            {
                App.ErrorManager.TrackException(exception, CharacterViewModel.CharacterModel.Name);
            }
        }));

        public event EventHandler<string> OpenWikiPageRequested;

        private WeaponDetailViewModel _weaponDetailViewModel;
        public WeaponDetailViewModel WeaponDetailViewModel
        {
            get => _weaponDetailViewModel;
            set => SetProperty(ref _weaponDetailViewModel, value);
        }
        
        private ICommand _removeItemCommand;

        public ICommand RemoveItemCommand => _removeItemCommand ?? (_removeItemCommand =
            new Command<EquippableItemViewModel>(item =>
            {
                try
                {
                    EquippableItemViewModels.Remove(item);
                    CharacterViewModel.CharacterModel.EquippedItems.Remove(item.EquipableItemModel);
                    CharacterViewModel.RecalculateHandicapAttributes();
                }
                catch (Exception e)
                {
                    App.ErrorManager.TrackException(e, CharacterViewModel.CharacterModel.Name);
                }
            }));

        private ICommand _openDailyGoodsWikiCommand;

        public ICommand OpenDailyGoodsWikiCommand => _openDailyGoodsWikiCommand ?? (_openDailyGoodsWikiCommand = new Command(() =>
        {
            try
            {
                var url = WikiConstants.DailyGoodsUrl;
                OpenWikiPageRequested?.Invoke(this, url);
            }
            catch (Exception exception)
            {
                App.ErrorManager.TrackException(exception, CharacterViewModel.CharacterModel.Name);
            }
        }));

        private ICommand _openAmmunitionCommand;

        public ICommand OpenAmmunitionCommand => _openAmmunitionCommand ?? (_openAmmunitionCommand = new Command(() =>
        {
            try
            {
                var url = WikiConstants.AmmunitionUrl;
                OpenWikiPageRequested?.Invoke(this, url);
            }
            catch (Exception exception)
            {
                App.ErrorManager.TrackException(exception, CharacterViewModel.CharacterModel.Name);
            }
        }));

        private ICommand _addItemCommand;

        public ICommand AddItemCommand => _addItemCommand ?? (_addItemCommand = new Command(() =>
        {
            try
            {
                var equipableItem = new EquipableItemModel(string.Empty, 0, false, false);
                CharacterViewModel.CharacterModel.EquippedItems.Add(equipableItem);
                EquippableItemViewModels.Add(new EquippableItemViewModel(equipableItem, CharacterViewModel));
            }
            catch (Exception e)
            {
                App.ErrorManager.TrackException(e, CharacterViewModel.CharacterModel.Name);
            }
        }));

        public ObservableCollection<EquippableItemViewModel> EquippableItemViewModels { get; set; }
    }
}
