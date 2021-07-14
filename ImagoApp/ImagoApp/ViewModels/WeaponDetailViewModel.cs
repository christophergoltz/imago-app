using System;
using System.Windows.Input;
using ImagoApp.Application;
using ImagoApp.Application.Models;
using Xamarin.Forms;

namespace ImagoApp.ViewModels
{
    public class WeaponDetailViewModel : BindableBase
    {
        private readonly CharacterViewModel _characterViewModel;
        private WeaponModel _weaponModel;

        public WeaponModel WeaponModel
        {
            get => _weaponModel;
            set => SetProperty(ref _weaponModel, value);
        }

        public ICommand CloseCommand { get; set; }
        public ICommand RemoveWeaponCommand { get; set; }

        public event EventHandler CloseRequested;
        public event EventHandler RemoveWeaponRequested;

        public bool FightValue
        {
            get => WeaponModel.Fight;
            set
            {
                WeaponModel.Fight = value;
                _characterViewModel.RecalculateHandicapAttributes();
                OnPropertyChanged(nameof(FightValue));
            }
        }

        public bool AdventureValue
        {
            get => WeaponModel.Adventure;
            set
            {
                WeaponModel.Adventure = value;
                _characterViewModel.RecalculateHandicapAttributes();
                OnPropertyChanged(nameof(AdventureValue));
            }
        }

        public int LoadValue
        {
            get => WeaponModel.LoadValue;
            set
            {
                WeaponModel.LoadValue = value;
                _characterViewModel.RecalculateHandicapAttributes();
                OnPropertyChanged(nameof(LoadValue));
            }
        }

        public WeaponDetailViewModel(WeaponModel weaponModel, CharacterViewModel characterViewModel)
        {
            _characterViewModel = characterViewModel;
            WeaponModel = weaponModel;
            
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
