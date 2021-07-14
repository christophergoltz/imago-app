using System.Linq;
using System.Windows.Input;
using ImagoApp.Application;
using ImagoApp.Application.Models;
using ImagoApp.Application.Services;
using ImagoApp.Infrastructure.Repositories;
using ImagoApp.Shared.Enums;
using Xamarin.Forms;

namespace ImagoApp.ViewModels
{
    public class StatusPageViewModel : BindableBase
    {
        private readonly IWikiDataService _wikiDataService;
        private WeaponDetailViewModel _weaponDetailViewModel;
        public CharacterViewModel CharacterViewModel { get; }
        
        public WeaponDetailViewModel WeaponDetailViewModel
        {
            get => _weaponDetailViewModel;
            set => SetProperty(ref _weaponDetailViewModel, value);
        }

        public WeaponListViewModel WeaponListViewModel { get; set; }

        public ICommand OpenWeaponCommand { get; set; }

        public StatusPageViewModel(CharacterViewModel characterViewModel, IWikiDataService wikiDataService)
        {
            _wikiDataService = wikiDataService;
            CharacterViewModel = characterViewModel;
            

            KopfViewModel = new BodyPartArmorListViewModel(characterViewModel, wikiDataService, CharacterViewModel.CharacterModel.BodyParts.First(_=> _.Type == BodyPartType.Kopf));
            TorsoViewModel = new BodyPartArmorListViewModel(characterViewModel, wikiDataService, CharacterViewModel.CharacterModel.BodyParts.First(_ => _.Type == BodyPartType.Torso));
            ArmLinksViewModel = new BodyPartArmorListViewModel(characterViewModel, wikiDataService, CharacterViewModel.CharacterModel.BodyParts.First(_ => _.Type == BodyPartType.ArmLinks));
            ArmRechtsViewModel = new BodyPartArmorListViewModel(characterViewModel, wikiDataService, CharacterViewModel.CharacterModel.BodyParts.First(_ => _.Type == BodyPartType.ArmRechts));
            BeinLinksViewModel = new BodyPartArmorListViewModel(characterViewModel, wikiDataService, CharacterViewModel.CharacterModel.BodyParts.First(_ => _.Type == BodyPartType.BeinLinks));
            BeinRechtsViewModel = new BodyPartArmorListViewModel(characterViewModel, wikiDataService, CharacterViewModel.CharacterModel.BodyParts.First(_ => _.Type == BodyPartType.BeinRechts));

            WeaponListViewModel = new WeaponListViewModel(characterViewModel, wikiDataService);
            WeaponListViewModel.OpenWeaponRequested += (sender, weapon) => OpenWeaponCommand?.Execute(weapon);

            OpenWeaponCommand = new Command<WeaponModel>(weapon =>
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
            });
        }

        public BodyPartArmorListViewModel KopfViewModel { get; set; }
        public BodyPartArmorListViewModel TorsoViewModel { get; set; }
        public BodyPartArmorListViewModel ArmLinksViewModel { get; set; }
        public BodyPartArmorListViewModel ArmRechtsViewModel { get; set; }
        public BodyPartArmorListViewModel BeinLinksViewModel { get; set; }
        public BodyPartArmorListViewModel BeinRechtsViewModel { get; set; }
    }
}
