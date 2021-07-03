using System;
using System.Windows.Input;
using ImagoApp.Application.Models;
using Xamarin.Forms;

namespace ImagoApp.ViewModels
{
    public class WeaponDetailViewModel : Util.BindableBase
    {
        private readonly CharacterViewModel _characterViewModel;
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
                _characterViewModel.RecalculateHandicapAttributes();
                OnPropertyChanged(nameof(FightValue));
            }
        }

        public bool AdventureValue
        {
            get => Weapon.Adventure;
            set
            {
                Weapon.Adventure = value;
                _characterViewModel.RecalculateHandicapAttributes();
                OnPropertyChanged(nameof(AdventureValue));
            }
        }

        public int LoadValue
        {
            get => Weapon.LoadValue;
            set
            {
                Weapon.LoadValue = value;
                _characterViewModel.RecalculateHandicapAttributes();
                OnPropertyChanged(nameof(LoadValue));
            }
        }

        public WeaponDetailViewModel(Weapon weapon, CharacterViewModel characterViewModel)
        {
            _characterViewModel = characterViewModel;
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
