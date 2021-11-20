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
using Xamarin.Forms;

namespace ImagoApp.ViewModels
{
    public class WeaponListViewModel : BindableBase
    {
        private readonly CharacterViewModel _characterViewModel;
        private readonly IWikiDataService _wikiDataService;
        private ICommand _addWeaponCommand;
        public event EventHandler<string> OpenWikiPageRequested;

        private ICommand _openMeleeWeaponWikiCommand;

        public ICommand OpenMeleeWeaponWikiCommand => _openMeleeWeaponWikiCommand ?? (_openMeleeWeaponWikiCommand = new Command(() =>
        {
            try
            {
                var url = WikiConstants.MeleeWeaponUrl;
                OpenWikiPageRequested?.Invoke(this, url);
            }
            catch (Exception exception)
            {
                App.ErrorManager.TrackException(exception, _characterViewModel.CharacterModel.Name);
            }
        }));

        private ICommand _openRangedWeaponWikiCommand;
        public ICommand OpenRangedWeaponWikiCommand => _openRangedWeaponWikiCommand ?? (_openRangedWeaponWikiCommand = new Command(() =>
        {
            try
            {
                var url = WikiConstants.RangedWeaponUrl;
                OpenWikiPageRequested?.Invoke(this, url);
            }
            catch (Exception exception)
            {
                App.ErrorManager.TrackException(exception, _characterViewModel.CharacterModel.Name);
            }
        }));

        private ICommand _openShieldWikiCommand;
        public ICommand OpenShieldWikiCommand => _openShieldWikiCommand ?? (_openShieldWikiCommand = new Command(() =>
        {
            try
            {
                var url = WikiConstants.ShieldsUrl;
                OpenWikiPageRequested?.Invoke(this, url);
            }
            catch (Exception exception)
            {
                App.ErrorManager.TrackException(exception, _characterViewModel.CharacterModel.Name);
            }
        }));

        private ICommand _openSpecialWeaponWikiCommand;
        public ICommand OpenSpecialWeaponWikiCommand => _openSpecialWeaponWikiCommand ?? (_openSpecialWeaponWikiCommand = new Command(() =>
        {
            try
            {
                var url = WikiConstants.SpecialWeaponUrl;
                OpenWikiPageRequested?.Invoke(this, url);
            }
            catch (Exception exception)
            {
                App.ErrorManager.TrackException(exception, _characterViewModel.CharacterModel.Name);
            }
        }));


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
                }
                catch (Exception e)
                {
                    App.ErrorManager.TrackException(e, _characterViewModel.CharacterModel.Name);
                }
            });
        }));
        
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
