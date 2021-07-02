using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using Xamarin.Forms;

namespace ImagoApp.ViewModels
{
    public class WeaponListViewModel :Util.BindableBase
    {
        private readonly CharacterViewModel _characterViewModel;
        private readonly Repository.WrappingDatabase.IMeleeWeaponRepository _meleeWeaponRepository;
        private readonly Repository.WrappingDatabase.IRangedWeaponRepository _rangedWeaponRepository;
        private readonly Repository.WrappingDatabase.ISpecialWeaponRepository _specialWeaponRepository;
        private readonly Repository.WrappingDatabase.IShieldRepository _shieldRepository;
        public ICommand AddWeaponCommand { get; }

        public event EventHandler<Models.Weapon> OpenWeaponRequested;

        public WeaponListViewModel(CharacterViewModel  characterViewModel, 
            Repository.WrappingDatabase.IMeleeWeaponRepository meleeWeaponRepository,
            Repository.WrappingDatabase.IRangedWeaponRepository rangedWeaponRepository,
            Repository.WrappingDatabase.ISpecialWeaponRepository specialWeaponRepository,
            Repository.WrappingDatabase.IShieldRepository shieldRepository)
        {
            foreach (var weapon in characterViewModel.Character.Weapons)
            {
                weapon.PropertyChanged += OnWeaponLoadValueChanged;
            }
            _characterViewModel = characterViewModel;
            _meleeWeaponRepository = meleeWeaponRepository;
            _rangedWeaponRepository = rangedWeaponRepository;
            _specialWeaponRepository = specialWeaponRepository;
            _shieldRepository = shieldRepository;

            AddWeaponCommand = new Command(() =>
            {
                Task.Run(async () =>
                {
                    Dictionary<string, Models.Weapon> weapons;

                    using (UserDialogs.Instance.Loading("Waffen werden geladen", null, null, true, MaskType.Black))
                    {
                        await Task.Delay(250);

                        var allWeapons = await _meleeWeaponRepository.GetAllItemsAsync();
                        allWeapons.AddRange(await _rangedWeaponRepository.GetAllItemsAsync());
                        allWeapons.AddRange(await _specialWeaponRepository.GetAllItemsAsync());
                        allWeapons.AddRange(await _shieldRepository.GetAllItemsAsync());

                        weapons = allWeapons
                            .ToDictionary(weapon => weapon.Name.ToString(), weapon => weapon);

                        await Task.Delay(250);
                    }

                    string result = null;

                    await Device.InvokeOnMainThreadAsync(async () =>
                    {
                        result = await UserDialogs.Instance.ActionSheetAsync($"Waffe hinzufügen", "Abbrechen", null,
                                CancellationToken.None, weapons.Keys.OrderBy(s => s).ToArray());
                    });

                    if (result == null || result.Equals("Abbrechen"))
                        return;

                    //copy object by value to prevent ref copies
                    var newWeapon = Util.ObjectHelper.DeepCopy(weapons[result]);
                    newWeapon.Fight = true;
                    newWeapon.Adventure = true;
                    await Device.InvokeOnMainThreadAsync(() =>
                    {
                        _characterViewModel.Character.Weapons.Add(newWeapon);
                    });
                    
                    newWeapon.PropertyChanged += OnWeaponLoadValueChanged;
                    _characterViewModel.RecalculateHandicapAttributes();

                    OpenWeaponRequested?.Invoke(this, newWeapon);
                });
            });
        }

        private void OnWeaponLoadValueChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName.Equals(nameof(Models.Weapon.LoadValue)))
            {
                _characterViewModel.RecalculateHandicapAttributes();
            }
        }
    }
}
