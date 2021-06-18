﻿using System;
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
        private readonly ICharacterService _characterService;
        private readonly ISpecialWeaponRepository _specialWeaponRepository;
        private readonly IShieldRepository _shieldRepository;
        private WeaponDetailViewModel _weaponDetailViewModel;
        public Character Character { get; }

        public List<DerivedAttribute> DerivedAttributes { get; set; }

        public WeaponDetailViewModel WeaponDetailViewModel
        {
            get => _weaponDetailViewModel;
            set => SetProperty(ref _weaponDetailViewModel, value);
        }

        public WeaponListViewModel WeaponListViewModel { get; set; }

        public ICommand OpenWeaponCommand { get; set; }

        public StatusPageViewModel(Character character, IArmorRepository armorRepository,
            IMeleeWeaponRepository meleeWeaponRepository, IRangedWeaponRepository rangedWeaponRepository,
            ICharacterService characterService, ISpecialWeaponRepository specialWeaponRepository,
            IShieldRepository shieldRepository)
        {
            _meleeWeaponRepository = meleeWeaponRepository;
            _rangedWeaponRepository = rangedWeaponRepository;
            _characterService = characterService;
            _specialWeaponRepository = specialWeaponRepository;
            _shieldRepository = shieldRepository;
            Character = character;

            DerivedAttributes = character.DerivedAttributes
                .Where(_ => _.Type == DerivedAttributeType.SprungreichweiteKampf ||
                            _.Type == DerivedAttributeType.SprunghoeheKampf ||
                            _.Type == DerivedAttributeType.Sprintreichweite ||
                            _.Type == DerivedAttributeType.TaktischeBewegung)
                .ToList();

            KopfViewModel = new BodyPartArmorListViewModel(_characterService, armorRepository,
                character.BodyParts[BodyPartType.Kopf], Character);
            TorsoViewModel = new BodyPartArmorListViewModel(_characterService, armorRepository,
                character.BodyParts[BodyPartType.Torso], Character);
            ArmLinksViewModel = new BodyPartArmorListViewModel(_characterService, armorRepository,
                character.BodyParts[BodyPartType.ArmLinks], Character);
            ArmRechtsViewModel = new BodyPartArmorListViewModel(_characterService, armorRepository,
                character.BodyParts[BodyPartType.ArmRechts], Character);
            BeinLinksViewModel = new BodyPartArmorListViewModel(_characterService, armorRepository,
                character.BodyParts[BodyPartType.BeinLinks], Character);
            BeinRechtsViewModel = new BodyPartArmorListViewModel(_characterService, armorRepository,
                character.BodyParts[BodyPartType.BeinRechts], Character);

            WeaponListViewModel = new WeaponListViewModel(character, _characterService, _meleeWeaponRepository,
                _rangedWeaponRepository, _specialWeaponRepository, _shieldRepository);
            WeaponListViewModel.OpenWeaponRequested += (sender, weapon) => OpenWeaponCommand?.Execute(weapon);

            OpenWeaponCommand = new Command<Weapon>(weapon =>
            {
                var vm = new WeaponDetailViewModel(weapon, character, characterService);
                vm.CloseRequested += (sender, args) => WeaponDetailViewModel = null;
                vm.RemoveWeaponRequested += (sender, args) =>
                {
                    character.Weapons.Remove(weapon);
                    _characterService.RecalculateHandicapAttributes(character);
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
