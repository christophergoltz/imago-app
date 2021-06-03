using System;
using System.Collections.Generic;
using System.Text;
using Imago.Models;
using Imago.Services;
using Imago.Util;

namespace Imago.ViewModels
{
    public class EquippableItemViewModel : BindableBase
    {
        private EquipableItem _equipableItem;
        private readonly ICharacterService _characterService;
        private readonly Character _character;

        public EquippableItemViewModel(EquipableItem equipableItem, ICharacterService characterService, Character character)
        {
            _equipableItem = equipableItem;
            _characterService = characterService;
            _character = character;
        }

        public EquipableItem EquipableItem
        {
            get => _equipableItem;
            set => SetProperty(ref _equipableItem, value);
        }

        public int LoadValue
        {
            get => _equipableItem.LoadValue;
            set
            {
                _equipableItem.LoadValue = value;
                OnPropertyChanged(nameof(LoadValue));
                _characterService.RecalculateHandicapAttributes(_character);
            }
        }

        public bool Fight
        {
            get => _equipableItem.Fight;
            set
            {
                _equipableItem.Fight = value;
                OnPropertyChanged(nameof(Fight));
                _characterService.RecalculateHandicapAttributes(_character);
            }
        }

        public bool Adventure
        {
            get => _equipableItem.Adventure;
            set
            {
                _equipableItem.Adventure = value;
                OnPropertyChanged(nameof(Adventure));
                _characterService.RecalculateHandicapAttributes(_character);
            }
        }
    }
}
