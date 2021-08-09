using System;
using System.Linq;
using System.Windows.Input;
using ImagoApp.Application;
using ImagoApp.Application.Models;
using ImagoApp.Application.Services;
using ImagoApp.Shared.Enums;
using Xamarin.Forms;

namespace ImagoApp.ViewModels
{
    public class StatusPageViewModel : BindableBase
    {
        private WeaponDetailViewModel _weaponDetailViewModel;
        public CharacterViewModel CharacterViewModel { get; }
        public event EventHandler<string> OpenWikiPageRequested;

        public WeaponDetailViewModel WeaponDetailViewModel
        {
            get => _weaponDetailViewModel;
            set => SetProperty(ref _weaponDetailViewModel, value);
        }

        private ICommand _openHealingWikiCommand;

        public ICommand OpenHealingWikiCommand => _openHealingWikiCommand ?? (_openHealingWikiCommand = new Command(() =>
        {
            var url = WikiConstants.HealingUrl;
            OpenWikiPageRequested?.Invoke(this, url);
        }));

        public WeaponListViewModel WeaponListViewModel { get; set; }

        private ICommand _openWeaponCommand;
        public ICommand OpenWeaponCommand => _openWeaponCommand ?? (_openWeaponCommand = new Command<WeaponModel>(weapon=>
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

        public StatusPageViewModel(CharacterViewModel characterViewModel, IWikiDataService wikiDataService)
        {
            CharacterViewModel = characterViewModel;
            
            KopfViewModel = new BodyPartArmorListViewModel(characterViewModel, wikiDataService, CharacterViewModel.CharacterModel.BodyParts.First(_=> _.Type == BodyPartType.Kopf));
            TorsoViewModel = new BodyPartArmorListViewModel(characterViewModel, wikiDataService, CharacterViewModel.CharacterModel.BodyParts.First(_ => _.Type == BodyPartType.Torso));
            ArmLinksViewModel = new BodyPartArmorListViewModel(characterViewModel, wikiDataService, CharacterViewModel.CharacterModel.BodyParts.First(_ => _.Type == BodyPartType.ArmLinks));
            ArmRechtsViewModel = new BodyPartArmorListViewModel(characterViewModel, wikiDataService, CharacterViewModel.CharacterModel.BodyParts.First(_ => _.Type == BodyPartType.ArmRechts));
            BeinLinksViewModel = new BodyPartArmorListViewModel(characterViewModel, wikiDataService, CharacterViewModel.CharacterModel.BodyParts.First(_ => _.Type == BodyPartType.BeinLinks));
            BeinRechtsViewModel = new BodyPartArmorListViewModel(characterViewModel, wikiDataService, CharacterViewModel.CharacterModel.BodyParts.First(_ => _.Type == BodyPartType.BeinRechts));

            WeaponListViewModel = new WeaponListViewModel(characterViewModel, wikiDataService);
            WeaponListViewModel.OpenWeaponRequested += (sender, weapon) => OpenWeaponCommand?.Execute(weapon);
            WeaponListViewModel.OpenWikiPageRequested += (sender, url) => { OpenWikiPageRequested?.Invoke(sender, url); };
        }

        public BodyPartArmorListViewModel KopfViewModel { get; set; }
        public BodyPartArmorListViewModel TorsoViewModel { get; set; }
        public BodyPartArmorListViewModel ArmLinksViewModel { get; set; }
        public BodyPartArmorListViewModel ArmRechtsViewModel { get; set; }
        public BodyPartArmorListViewModel BeinLinksViewModel { get; set; }
        public BodyPartArmorListViewModel BeinRechtsViewModel { get; set; }
    }
}
