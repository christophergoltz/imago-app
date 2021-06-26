using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.XPath;
using Acr.UserDialogs;
using Imago.Models;
using Imago.Models.Enum;
using Imago.Repository;
using Imago.Repository.WrappingDatabase;
using Imago.Services;
using Imago.Util;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace Imago.ViewModels
{
    public class BodyPartArmorListViewModel
    {
        private readonly CharacterViewModel _characterViewModel;
        public BodyPart BodyPart { get; }

        public ICommand RemoveArmorCommand { get; set; }
        public ICommand AddArmorCommand { get; set; }

        public BodyPartArmorListViewModel(CharacterViewModel characterViewModel,  IArmorRepository armorRepository, BodyPart bodyPart)
        {
            _characterViewModel = characterViewModel;
            BodyPart = bodyPart;
            foreach (var armor in bodyPart.Armor)
            {
                armor.PropertyChanged += OnArmorPropertyChanged;
            }
            
            RemoveArmorCommand = new Command<ArmorModel>(armor =>
            {
                BodyPart.Armor.Remove(armor);
                characterViewModel.RecalculateHandicapAttributes();
            });

            AddArmorCommand = new Command(() =>
            {
                Task.Run(async () =>
                {
                    Dictionary<string, ArmorModel> armor;

                    using (UserDialogs.Instance.Loading("Rüstungen werden geladen", null, null, true, MaskType.Black))
                    {
                        await Task.Delay(250);

                        var currentBodyPart = bodyPart.Type.MapBodyPartTypeToArmorPartType();
                        var allArmor = await armorRepository.GetAllItemsAsync();
                        armor = allArmor
                            .SelectMany(armorSet => armorSet.ArmorParts)
                            .Where(pair => pair.Key == currentBodyPart)
                            .Select(pair => pair.Value)
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
                    var newArmor = armor[result].DeepCopy();
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
            if (args.PropertyName.Equals(nameof(ArmorModel.LoadValue))
                || args.PropertyName.Equals(nameof(ArmorModel.Fight))
                || args.PropertyName.Equals(nameof(ArmorModel.Adventure)))
            {
                _characterViewModel.RecalculateHandicapAttributes();
            }
        }
    }
}