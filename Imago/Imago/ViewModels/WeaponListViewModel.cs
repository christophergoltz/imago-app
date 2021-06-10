using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Imago.Models;
using Imago.Repository;
using Imago.Repository.WrappingDatabase;
using Imago.Services;
using Imago.Util;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Imago.ViewModels
{
    public class WeaponListViewModel :BindableBase
    {
        private readonly Character _character;
        private readonly ICharacterService _characterService;
        private readonly IMeleeWeaponRepository _meleeWeaponRepository;
        private readonly IRangedWeaponRepository _rangedWeaponRepository;
        public ICommand AddWeaponCommand { get; }
        public ICommand RemoveWeaponCommand { get; set; }

        public WeaponListViewModel(Character character, ICharacterService characterService, IMeleeWeaponRepository meleeWeaponRepository, IRangedWeaponRepository rangedWeaponRepository)
        {
            _character = character;
            foreach (var weapon in _character.Weapons)
            {
                weapon.PropertyChanged += OnWeaponLoadValueChanged;
            }
            _characterService = characterService;
            _meleeWeaponRepository = meleeWeaponRepository;
            _rangedWeaponRepository = rangedWeaponRepository;

            AddWeaponCommand = new Command(async () =>
            {
                var allWeapons = await _meleeWeaponRepository.GetAllItemsAsync();
                allWeapons.AddRange(await _rangedWeaponRepository.GetAllItemsAsync());

                var weapons = allWeapons
                    .ToDictionary(weapon => weapon.Name.ToString(), weapon => weapon);

                var result =
                    await Shell.Current.DisplayActionSheet($"Waffe hinzufügen", "Abbrechen", null,
                        weapons.Keys.ToArray());

                if (result == null || result.Equals("Abbrechen"))
                    return;

                //copy object by value to prevent ref copies
                var newWeapon = weapons[result].DeepCopy();
                _character.Weapons.Add(newWeapon);
                newWeapon.PropertyChanged += OnWeaponLoadValueChanged;
                _characterService.RecalculateHandicapAttributes(_character);
            });

            RemoveWeaponCommand = new Command<Weapon>(weapon =>
            {
                _character.Weapons.Remove(weapon);
                _characterService.RecalculateHandicapAttributes(_character);
            });
        }

        private void OnWeaponLoadValueChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName.Equals(nameof(Weapon.LoadValue)))
            {
                _characterService.RecalculateHandicapAttributes(_character);
            }
        }
    }
}
