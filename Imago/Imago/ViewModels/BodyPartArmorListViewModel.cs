using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Xml.XPath;
using Imago.Models;
using Imago.Models.Enum;
using Imago.Repository;
using Imago.Repository.WrappingDatabase;
using Imago.Services;
using Imago.Util;
using Xamarin.Forms;

namespace Imago.ViewModels
{
    public class BodyPartArmorListViewModel
    {
        private readonly ICharacterService _characterService;
        private readonly Character _character;
        public BodyPart BodyPart { get; }
        

        public ICommand RemoveArmorCommand { get; set; }
        public ICommand AddArmorCommand { get; set; }

        public BodyPartArmorListViewModel( ICharacterService characterService, IArmorRepository armorRepository,
            BodyPart bodyPart, Character character)
        {
            _characterService = characterService;
            BodyPart = bodyPart;
            _character = character;
            foreach (var armor in bodyPart.Armor)
            {
                armor.PropertyChanged += OnArmorPropertyChanged;
            }
            
            RemoveArmorCommand = new Command<ArmorModel>(armor =>
            {
                BodyPart.Armor.Remove(armor);
                _characterService.RecalculateHandicapAttributes(_character);
            });

            AddArmorCommand = new Command(async () =>
            {
                var currentBodyPart = bodyPart.Type.MapBodyPartTypeToArmorPartType();
                var allArmor = await armorRepository.GetAllItemsAsync();
                var armor = allArmor
                    .SelectMany(armorSet => armorSet.ArmorParts)
                    .Where(pair => pair.Key == currentBodyPart)
                    .Select(pair => pair.Value)
                    .ToDictionary(_ => _.Name, _ => _);
                
                var result =
                    await Shell.Current.DisplayActionSheet($"Rüstung hinzufügen", "Abbrechen", null, armor.Keys.OrderBy(s => s).ToArray());

                if (result == null || result.Equals("Abbrechen"))
                    return;

                //copy object by value to prevent ref copy
                var newArmor = armor[result].DeepCopy();
                newArmor.Adventure = true;
                newArmor.Fight = true;
                newArmor.PropertyChanged += OnArmorPropertyChanged;
                BodyPart.Armor.Add(newArmor);
                _characterService.RecalculateHandicapAttributes(_character);
            });
        }

        private void OnArmorPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName.Equals(nameof(ArmorModel.LoadValue))
                || args.PropertyName.Equals(nameof(ArmorModel.Fight))
                || args.PropertyName.Equals(nameof(ArmorModel.Adventure)))
            {
                _characterService.RecalculateHandicapAttributes(_character);
            }
        }
    }
}