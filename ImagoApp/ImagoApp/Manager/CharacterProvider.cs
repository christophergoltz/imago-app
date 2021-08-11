using System;
using System.Collections.Generic;
using System.Text;
using ImagoApp.Application;
using ImagoApp.Application.Models;
using ImagoApp.Application.Services;
using ImagoApp.ViewModels;

namespace ImagoApp.Manager
{
    public interface ICharacterProvider
    {
        CharacterViewModel CurrentCharacter { get; }
        void SetCurrentCharacter(CharacterModel characterModel, bool editMode);
        void ClearCurrentCharacter();
    }

    public class CharacterProvider : ICharacterProvider
    {
        private readonly IAttributeCalculationService _attributeCalculationService;
        private readonly ISkillGroupCalculationService _skillGroupCalculationService;
        private readonly ISkillCalculationService _skillCalculationService;

        public CharacterProvider(IAttributeCalculationService attributeCalculationService,
            ISkillGroupCalculationService skillGroupCalculationService,
            ISkillCalculationService skillCalculationService)
        {
            _attributeCalculationService = attributeCalculationService;
            _skillGroupCalculationService = skillGroupCalculationService;
            _skillCalculationService = skillCalculationService;
        }

        public CharacterViewModel CurrentCharacter { get; private set; }

        public void SetCurrentCharacter(CharacterModel characterModel, bool editMode)
        {
            CurrentCharacter = new CharacterViewModel(characterModel,
                _attributeCalculationService,
                _skillGroupCalculationService,
                _skillCalculationService)
            {
                EditMode = editMode
            };
        }

        public void ClearCurrentCharacter()
        {
            CurrentCharacter = null;
        }
    }
}
