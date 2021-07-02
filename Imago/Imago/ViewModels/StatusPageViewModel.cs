using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Imago.Models;
using Imago.Models.Enum;
using Imago.Repository;
using Imago.Repository.WrappingDatabase;
using Imago.Services;
using Imago.Util;
using Imago.Views.CustomControls;
using Xamarin.Forms;

namespace Imago.ViewModels
{
    public class StatusPageViewModel : BindableBase
    {
        private readonly IMeleeWeaponRepository _meleeWeaponRepository;
        private readonly IRangedWeaponRepository _rangedWeaponRepository;
        private readonly ISpecialWeaponRepository _specialWeaponRepository;
        private readonly IShieldRepository _shieldRepository;
        private WeaponDetailViewModel _weaponDetailViewModel;
        public CharacterViewModel CharacterViewModel { get; }
        
        public WeaponDetailViewModel WeaponDetailViewModel
        {
            get => _weaponDetailViewModel;
            set => SetProperty(ref _weaponDetailViewModel, value);
        }

        public WeaponListViewModel WeaponListViewModel { get; set; }

        public ICommand OpenWeaponCommand { get; set; }

        public StatusPageViewModel(CharacterViewModel characterViewModel, IArmorRepository armorRepository,
            IMeleeWeaponRepository meleeWeaponRepository, IRangedWeaponRepository rangedWeaponRepository, ISpecialWeaponRepository specialWeaponRepository,
            IShieldRepository shieldRepository)
        {
            _meleeWeaponRepository = meleeWeaponRepository;
            _rangedWeaponRepository = rangedWeaponRepository;
            _specialWeaponRepository = specialWeaponRepository;
            _shieldRepository = shieldRepository;
            CharacterViewModel = characterViewModel;
            

            KopfViewModel = new BodyPartArmorListViewModel(characterViewModel, armorRepository, CharacterViewModel.Character.BodyParts[BodyPartType.Kopf]);
            TorsoViewModel = new BodyPartArmorListViewModel(characterViewModel, armorRepository, CharacterViewModel.Character.BodyParts[BodyPartType.Torso]);
            ArmLinksViewModel = new BodyPartArmorListViewModel(characterViewModel, armorRepository, CharacterViewModel.Character.BodyParts[BodyPartType.ArmLinks]);
            ArmRechtsViewModel = new BodyPartArmorListViewModel(characterViewModel, armorRepository, CharacterViewModel.Character.BodyParts[BodyPartType.ArmRechts]);
            BeinLinksViewModel = new BodyPartArmorListViewModel(characterViewModel, armorRepository, CharacterViewModel.Character.BodyParts[BodyPartType.BeinLinks]);
            BeinRechtsViewModel = new BodyPartArmorListViewModel(characterViewModel, armorRepository, CharacterViewModel.Character.BodyParts[BodyPartType.BeinRechts]);

            WeaponListViewModel = new WeaponListViewModel(characterViewModel, _meleeWeaponRepository,
                _rangedWeaponRepository, _specialWeaponRepository, _shieldRepository);
            WeaponListViewModel.OpenWeaponRequested += (sender, weapon) => OpenWeaponCommand?.Execute(weapon);

            OpenWeaponCommand = new Command<Weapon>(weapon =>
            {
                var vm = new WeaponDetailViewModel(weapon, CharacterViewModel);
                vm.CloseRequested += (sender, args) => WeaponDetailViewModel = null;
                vm.RemoveWeaponRequested += (sender, args) =>
                {
                    CharacterViewModel.Character.Weapons.Remove(weapon);
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
