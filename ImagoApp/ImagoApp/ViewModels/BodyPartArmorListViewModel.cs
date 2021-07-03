using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using ImagoApp.Application.Models;
using ImagoApp.Application.Services;
using ImagoApp.Shared.Enums;
using Xamarin.Forms;

namespace ImagoApp.ViewModels
{
    public class BodyPartArmorListViewModel
    {
        private readonly CharacterViewModel _characterViewModel;
        public BodyPart BodyPart { get; }

        public ICommand RemoveArmorCommand { get; set; }
        public ICommand AddArmorCommand { get; set; }

        public BodyPartArmorListViewModel(CharacterViewModel characterViewModel, IWikiDataService wikiDataService, BodyPart bodyPart)
        {
            _characterViewModel = characterViewModel;
            BodyPart = bodyPart;
            foreach (var armor in bodyPart.Armor)
            {
                armor.PropertyChanged += OnArmorPropertyChanged;
            }
            
            RemoveArmorCommand = new Command<ArmorPartModel>(armor =>
            {
                BodyPart.Armor.Remove(armor);
                characterViewModel.RecalculateHandicapAttributes();
            });

            AddArmorCommand = new Command(() =>
            {
                Task.Run(async () =>
                {
                    Dictionary<string, ArmorPartModel> armor;

                    using (UserDialogs.Instance.Loading("Rüstungen werden geladen", null, null, true, MaskType.Black))
                    {
                        await Task.Delay(250);

                        var currentBodyPart = bodyPart.Type.MapBodyPartTypeToArmorPartType();
                        var allArmor = await wikiDataService.GetAllArmor();
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
                    
                    //copy object by value to prevent ref copy
                    var newArmor = Util.ObjectHelper.DeepCopy(armor[result]);
                    newArmor.Adventure = true;
                    newArmor.Fight = true;
                    await Device.InvokeOnMainThreadAsync(() =>
                    {
                        BodyPart.Armor.Add(newArmor);
                    });

                    newArmor.PropertyChanged += OnArmorPropertyChanged;
                    characterViewModel.RecalculateHandicapAttributes();
                });
            });
        }

        private void OnArmorPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName.Equals(nameof(ArmorPartModel.LoadValue))
                || args.PropertyName.Equals(nameof(ArmorPartModel.Fight))
                || args.PropertyName.Equals(nameof(ArmorPartModel.Adventure)))
            {
                _characterViewModel.RecalculateHandicapAttributes();
            }
        }
    }
}