using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Imago.Models;
using Imago.Repository;
using Imago.Services;
using Imago.Shared.Util;
using Imago.Util;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Imago.ViewModels
{
    public class WeaponListViewModel :BindableBase
    {
        private readonly Character _character;
        private readonly ICharacterService _characterService;
        private readonly IItemRepository _itemRepository;
        public ICommand AddWeaponCommand { get; }
        public ICommand RemoveWeaponCommand { get; set; }

        public WeaponListViewModel(Character character, ICharacterService characterService, IItemRepository itemRepository)
        {
            _character = character;
            foreach (var weapon in _character.Weapons)
            {
                weapon.PropertyChanged += OnWeaponLoadValueChanged;
            }
            _characterService = characterService;
            _itemRepository = itemRepository;
            AddWeaponCommand = new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    //todo use converter
                    var x = _itemRepository.GetAllWeapons()
                        .ToDictionary(s => s.Type.ToString(), s => s);

                    var result =
                        await Shell.Current.DisplayActionSheet($"Waffe hinzufügen", "Abbrechen", null, x.Keys.ToArray());

                    if (result == null || result.Equals("Abbrechen"))
                        return;

                    var newWeapon = x[result];
                    _character.Weapons.Add(newWeapon);
                    newWeapon.PropertyChanged += OnWeaponLoadValueChanged;
                    _characterService.RecalculateHandicapAttributes(_character);
                });
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
