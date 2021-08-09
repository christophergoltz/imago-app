using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using ImagoApp.Application;
using ImagoApp.Application.Models;
using ImagoApp.Application.Models.Template;
using ImagoApp.Application.Services;
using ImagoApp.Shared.Enums;
using Xamarin.Forms;

namespace ImagoApp.ViewModels
{
    public class BodyPartArmorListViewModel : BindableBase
    {
        private readonly CharacterViewModel _characterViewModel;
        private readonly IWikiDataService _wikiDataService;
        public BodyPartModel BodyPartModel { get; }

        private ICommand _removeArmorCommand;
        public ICommand RemoveArmorCommand => _removeArmorCommand ?? (_removeArmorCommand = new Command<ArmorPartModelModel>(armor =>
        {
            try
            {
                BodyPartModel.Armor.Remove(armor);
                _characterViewModel.RecalculateHandicapAttributes();
            }
            catch (Exception exception)
            {
                App.ErrorManager.TrackException(exception, _characterViewModel.CharacterModel.Name);
            }
        }));

        public int CurrentHitpoints
        {
            get
            {
                var currentHitpointsFloat = BodyPartModel.MaxHitpoints * BodyPartModel.CurrentHitpointsPercentage;
                var currentHitpoints = currentHitpointsFloat.GetRoundedValue();
                return currentHitpoints;
            }
            set
            {
                var newPercentage = value / (double)BodyPartModel.MaxHitpoints;
                BodyPartModel.CurrentHitpointsPercentage = newPercentage;
            }
        }


        private ICommand _addArmorCommand;
        public ICommand AddArmorCommand => _addArmorCommand ?? (_addArmorCommand = new Command(() =>
        {
            try
            {
                Task.Run(async () =>
                {
                    Dictionary<string, ArmorPartTemplateModel> armor;

                    using (UserDialogs.Instance.Loading("Rüstungen werden geladen", null, null, true, MaskType.Black))
                    {
                        await Task.Delay(250);

                        var currentBodyPart = BodyPartModel.Type.MapBodyPartTypeToArmorPartType();
                        var allArmor = _wikiDataService.GetAllArmor();
                        armor = allArmor
                            .Where(pair => pair.ArmorPartType == currentBodyPart)
                            .Select(pair => pair)
                            .ToDictionary(_ => _.Name, _ => _);

                        armor.Add("Natürlich", new ArmorPartTemplateModel()
                        {
                            ArmorPartType = currentBodyPart,
                            Name = "Natürlich"
                        });

                        await Task.Delay(250);
                    }

                    string result = null;

                    await Device.InvokeOnMainThreadAsync(async () =>
                    {
                        result = await UserDialogs.Instance.ActionSheetAsync($"Rüstung hinzufügen", "Abbrechen", null,
                            null,
                            armor.Keys.OrderBy(s => s).ToArray());
                    });

                    if (result == null || result.Equals("Abbrechen"))
                        return;

                    var selectedArmor = armor[result];
                    var newArmor = _wikiDataService.GetArmorFromTemplate(selectedArmor);
                    await Device.InvokeOnMainThreadAsync(() =>
                    {
                        BodyPartModel.Armor.Add(newArmor);
                    });

                    newArmor.PropertyChanged += OnArmorPropertyChanged;
                    _characterViewModel.RecalculateHandicapAttributes();
                });
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }));
        
        public BodyPartArmorListViewModel(CharacterViewModel characterViewModel, IWikiDataService wikiDataService, BodyPartModel bodyPartModel)
        {
            _characterViewModel = characterViewModel;
            _wikiDataService = wikiDataService;
            BodyPartModel = bodyPartModel;
            BodyPartModel.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName.Equals(nameof(BodyPartModel.CurrentHitpointsPercentage)) || args.PropertyName.Equals(nameof(BodyPartModel.MaxHitpoints)))
                {
                    OnPropertyChanged(nameof(BodyPartModel));
                    OnPropertyChanged(nameof(CurrentHitpoints));
                }
            };
            
            foreach (var armor in bodyPartModel.Armor)
            {
                armor.PropertyChanged += OnArmorPropertyChanged;
            }
        }

        private void OnArmorPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName.Equals(nameof(ArmorPartModelModel.LoadValue))
                || args.PropertyName.Equals(nameof(ArmorPartModelModel.Fight))
                || args.PropertyName.Equals(nameof(ArmorPartModelModel.Adventure)))
            {
                _characterViewModel.RecalculateHandicapAttributes();
            }
        }
    }
}