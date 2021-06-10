using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Imago.Models;
using Imago.Repository;
using Imago.Services;
using Imago.Shared.Models;
using Xamarin.Forms;

namespace Imago.ViewModels
{
    public class BodyPartArmorListViewModel
    {
        private ICharacterService _characterService;
        private IItemRepository _itemRepository;
        private Character _character;
        public BodyPart BodyPart { get; }
        

        public ICommand RemoveArmorCommand { get; set; }
        public ICommand AddArmorCommand { get; set; }

        public BodyPartArmorListViewModel(IItemRepository itemRepository, ICharacterService characterService,
            BodyPart bodyPart, Character character)
        {
            _characterService = characterService;
            _itemRepository = itemRepository;
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
                //todo use converter
                var armor = _itemRepository.GetAllArmorParts(bodyPart.Type).ToDictionary(s => s.Type.ToString(), s => s);

                var result =
                    await Shell.Current.DisplayActionSheet($"Rüstung hinzufügen", "Abbrechen", null, armor.Keys.ToArray());

                if (result == null || result.Equals("Abbrechen"))
                    return;

                var newArmor = armor[result];
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