using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imago.Models;
using Imago.Models.Enum;
using Imago.Repository;
using Imago.Services;
using Imago.Util;
using Imago.Views.CustomControls;

namespace Imago.ViewModels
{
    public class StatusPageViewModel : BindableBase
    {
        private readonly IItemRepository _itemRepository;
        private readonly ICharacterService _characterService;
        public Character Character { get; }

        public List<DerivedAttribute> DerivedAttributes { get; set; }

        public WeaponListViewModel WeaponListViewModel { get; set; }

        public StatusPageViewModel(Character character, IItemRepository itemRepository,
            ICharacterService characterService)
        {
            _itemRepository = itemRepository;
            _characterService = characterService;
            Character = character;

            DerivedAttributes = character.DerivedAttributes
                .Where(_ => _.Type == DerivedAttributeType.SprungreichweiteKampf ||
                            _.Type == DerivedAttributeType.SprunghoeheKampf ||
                            _.Type == DerivedAttributeType.Sprintreichweite ||
                            _.Type == DerivedAttributeType.TaktischeBewegung)
                .ToList();

            KopfViewModel = new BodyPartArmorListViewModel(_itemRepository, _characterService,
                character.BodyParts[BodyPartType.Kopf], Character);
            TorsoViewModel = new BodyPartArmorListViewModel(_itemRepository, _characterService,
                character.BodyParts[BodyPartType.Torso], Character);
            ArmLinksViewModel = new BodyPartArmorListViewModel(_itemRepository, _characterService,
                character.BodyParts[BodyPartType.ArmLinks], Character);
            ArmRechtsViewModel = new BodyPartArmorListViewModel(_itemRepository, _characterService,
                character.BodyParts[BodyPartType.ArmRechts], Character);
            BeinLinksViewModel = new BodyPartArmorListViewModel(_itemRepository, _characterService,
                character.BodyParts[BodyPartType.BeinLinks], Character);
            BeinRechtsViewModel = new BodyPartArmorListViewModel(_itemRepository, _characterService,
                character.BodyParts[BodyPartType.BeinRechts], Character);

            WeaponListViewModel = new WeaponListViewModel(character, _characterService, _itemRepository);
        }

        public BodyPartArmorListViewModel KopfViewModel { get; set; }
        public BodyPartArmorListViewModel TorsoViewModel { get; set; }
        public BodyPartArmorListViewModel ArmLinksViewModel { get; set; }
        public BodyPartArmorListViewModel ArmRechtsViewModel { get; set; }
        public BodyPartArmorListViewModel BeinLinksViewModel { get; set; }
        public BodyPartArmorListViewModel BeinRechtsViewModel { get; set; }
    }
}
