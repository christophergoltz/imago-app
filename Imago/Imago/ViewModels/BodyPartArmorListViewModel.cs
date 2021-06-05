using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Imago.Models;
using Imago.Repository;
using Imago.Services;

namespace Imago.ViewModels
{
    public class BodyPartArmorListViewModel
    {
        public ICharacterService CharacterService { get; }
        public IItemRepository ItemRepository { get; }
        public BodyPart BodyPart  { get; set; }
        public Character Character  { get; }
        
        public BodyPartArmorListViewModel(IItemRepository itemRepository, ICharacterService characterService, BodyPart bodyPart, Character character)
        {
            CharacterService = characterService;
            ItemRepository = itemRepository;
            BodyPart = bodyPart;
            Character = character;

            var armors = bodyPart.Armor.Select(_ => ItemRepository.GetArmorSet(_))
                .Select(set => set.ArmorParts[bodyPart.Type]).ToList();

            Armor = new ObservableCollection<Armor>(armors);
        }

        public ObservableCollection<Armor> Armor { get; set; }
    }
}
