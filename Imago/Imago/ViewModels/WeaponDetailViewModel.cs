using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Imago.Models;
using Imago.Util;
using Xamarin.Forms;

namespace Imago.ViewModels
{
    public class WeaponDetailViewModel : BindableBase
    {
        private Weapon _weapon;

        public Weapon Weapon
        {
            get => _weapon;
            set => SetProperty(ref _weapon, value);
        }

        public ICommand CloseCommand { get; set; }

        public event EventHandler CloseRequested;

        public WeaponDetailViewModel(Weapon weapon)
        {
            Weapon = weapon;
            
            CloseCommand = new Command(() =>
            {
                CloseRequested?.Invoke(this, EventArgs.Empty);
            });
        }
    }
}
