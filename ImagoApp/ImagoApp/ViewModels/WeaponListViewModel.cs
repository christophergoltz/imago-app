using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using ImagoApp.Application;
using ImagoApp.Application.Models;
using ImagoApp.Application.Models.Template;
using ImagoApp.Application.Services;
using ImagoApp.Util;
using Xamarin.Forms;

namespace ImagoApp.ViewModels
{
    public class WeaponListViewModel : BindableBase
    {
        private readonly CharacterViewModel _characterViewModel;
        private readonly IWikiDataService _wikiDataService;
        private ICommand _addWeaponCommand;
        public ICommand AddWeaponCommand => _addWeaponCommand ?? (_addWeaponCommand = new Command(() =>
        {
            Task.Run(async () =>
            {
                try
                {
                    Dictionary<string, WeaponTemplateModel> weapons;

                    using (UserDialogs.Instance.Loading("Waffen werden geladen", null, null, true, MaskType.Black))
                    {
                        await Task.Delay(250);

                        var allWeapons = _wikiDataService.GetAllWeapons();
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

                    var selectedWeapon = weapons[result];
                    var newWeapon = _wikiDataService.GetWeaponFromTemplate(selectedWeapon);

                    await Device.InvokeOnMainThreadAsync(() =>
                    {
                        _characterViewModel.CharacterModel.Weapons.Add(newWeapon);
                    });

                    newWeapon.PropertyChanged += OnWeaponLoadValueChanged;
                    _characterViewModel.RecalculateHandicapAttributes();

                    OpenWeaponRequested?.Invoke(this, newWeapon);
                }
                catch (Exception e)
                {
                    App.ErrorManager.TrackException(e, _characterViewModel.CharacterModel.Name);
                }
            });
        }));

        public event EventHandler<WeaponModel> OpenWeaponRequested;

        public WeaponListViewModel(CharacterViewModel characterViewModel, IWikiDataService wikiDataService)
        {
            foreach (var weapon in characterViewModel.CharacterModel.Weapons)
            {
                weapon.PropertyChanged += OnWeaponLoadValueChanged;
            }
            _characterViewModel = characterViewModel;
            _wikiDataService = wikiDataService;
        }

        private void OnWeaponLoadValueChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName.Equals(nameof(WeaponModel.LoadValue)))
            {
                _characterViewModel.RecalculateHandicapAttributes();
            }
        }
    }
}
