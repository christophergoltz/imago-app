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
using ImagoApp.Util;
using Xamarin.Forms;

namespace ImagoApp.ViewModels
{
    public class BodyPartArmorListViewModel : BindableBase
    {
        private readonly CharacterViewModel _characterViewModel;
        public BodyPartModel BodyPartModel { get; }

        public ICommand RemoveArmorCommand { get; set; }
        public ICommand AddArmorCommand { get; set; }

        public BodyPartArmorListViewModel(CharacterViewModel characterViewModel, IWikiDataService wikiDataService, BodyPartModel bodyPartModel)
        {
            _characterViewModel = characterViewModel;
            BodyPartModel = bodyPartModel;
            BodyPartModel.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName.Equals(nameof(BodyPartModel.CurrentHitpoints)) || args.PropertyName.Equals(nameof(BodyPartModel.MaxHitpoints)))
                {
                    OnPropertyChanged(nameof(BodyPartModel));
                }
            };
            
            foreach (var armor in bodyPartModel.Armor)
            {
                armor.PropertyChanged += OnArmorPropertyChanged;
            }
            
            RemoveArmorCommand = new Command<ArmorPartModelModel>(armor =>
            {
                BodyPartModel.Armor.Remove(armor);
                characterViewModel.RecalculateHandicapAttributes();
            });

            AddArmorCommand = new Command(() =>
            {
                Task.Run(async () =>
                {
                    Dictionary<string, ArmorPartTemplateModel> armor;

                    using (UserDialogs.Instance.Loading("Rüstungen werden geladen", null, null, true, MaskType.Black))
                    {
                        await Task.Delay(250);

                        var currentBodyPart = bodyPartModel.Type.MapBodyPartTypeToArmorPartType();
                        var allArmor = wikiDataService.GetAllArmor();
                        armor = allArmor
                            .Where(pair => pair.ArmorPartType == currentBodyPart)
                            .Select(pair => pair)
                            .ToDictionary(_ => _.Name, _ => _);

                        await Task.Delay(250);
                    }

                    string result = null;
                    
                    await Device.InvokeOnMainThreadAsync(async () =>
                    {
                        result = await UserDialogs.Instance.ActionSheetAsync($"Rüstung hinzufügen", "Abbrechen", null, null,
                            armor.Keys.OrderBy(s => s).ToArray());
                    });

                    if (result == null || result.Equals("Abbrechen"))
                        return;

                    var selectedArmor = armor[result];
                    var newArmor = wikiDataService.GetArmorFromTemplate(selectedArmor);
                    await Device.InvokeOnMainThreadAsync(() =>
                    {
                        BodyPartModel.Armor.Add(newArmor);
                    });

                    newArmor.PropertyChanged += OnArmorPropertyChanged;
                    characterViewModel.RecalculateHandicapAttributes();
                });
            });
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