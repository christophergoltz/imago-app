using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Imago.Models;
using Imago.Services;
using Imago.Util;
using Xamarin.Forms;

namespace Imago.ViewModels
{
    public class WeaponDetailViewModel : BindableBase
    {
        private readonly Character _character;
        private readonly ICharacterService _characterService;
        private Weapon _weapon;

        public Weapon Weapon
        {
            get => _weapon;
            set => SetProperty(ref _weapon, value);
        }

        public ICommand CloseCommand { get; set; }
        public ICommand RemoveWeaponCommand { get; set; }

        public event EventHandler CloseRequested;
        public event EventHandler RemoveWeaponRequested;

        public bool FightValue
        {
            get => Weapon.Fight;
            set
            {
                Weapon.Fight = value;
                _characterService.RecalculateHandicapAttributes(_character);
                OnPropertyChanged(nameof(FightValue));
            }
        }

        public bool AdventureValue
        {
            get => Weapon.Adventure;
            set
            {
                Weapon.Adventure = value;
                _characterService.RecalculateHandicapAttributes(_character);
                OnPropertyChanged(nameof(AdventureValue));
            }
        }

        public int LoadValue
        {
            get => Weapon.LoadValue;
            set
            {
                Weapon.LoadValue = value;
                _characterService.RecalculateHandicapAttributes(_character);
                OnPropertyChanged(nameof(LoadValue));
            }
        }

        public WeaponDetailViewModel(Weapon weapon, Character character, ICharacterService characterService)
        {
            _character = character;
            _characterService = characterService;
            Weapon = weapon;
            
            CloseCommand = new Command(() =>
            {
                CloseRequested?.Invoke(this, EventArgs.Empty);
            });

            RemoveWeaponCommand = new Command(() =>
            {
                RemoveWeaponRequested?.Invoke(this, EventArgs.Empty);
            });
        }
    }
}
