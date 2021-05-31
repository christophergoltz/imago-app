using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Imago.Converter;
using Imago.Models;
using Imago.Models.Base;
using Imago.Models.Enum;
using Imago.Services;
using Imago.Util;
using Xamarin.Forms;

namespace Imago.ViewModels
{
    public class SkillPageViewModel : BindableBase
    {
        private readonly ICharacterService _characterService;
        private AttributeSkillSourceToStringConverter _converter = new AttributeSkillSourceToStringConverter();
        private UpgradeableSkillBase _selectedSkill;
        private string _selectedSkillName;
        private string _selectedSkillSourceName;

        public SkillPageViewModel(Character character, ICharacterService characterService)
        {
            _characterService = characterService;
            Character = character;
        }

        public Character Character { get; private set; }

        public UpgradeableSkillBase SelectedSkill
        {
            get => _selectedSkill;
            set
            {
                Debug.WriteLine("Set [SelectedSkill] to " + value ?? "null");
                SetProperty(ref _selectedSkill, value);

                //update dependet properties
                OnPropertyChanged(nameof(SelectedSkillExperience));
                OnPropertyChanged(nameof(SelectedSkillModification));
            }
        }

        public int SelectedSkillExperience
        {
            get => SelectedSkill?.Experience ?? 0;
            set
            {
                //catch closing event, when SelectedSkill is set to null
                if (SelectedSkill == null)
                    return;

                var newOpenAttributeIncreases = new List<SkillGroupType>();

                if (SelectedSkill is Skill skill)
                    newOpenAttributeIncreases = _characterService.AddExperienceToSkill(skill, _skillParent, 1).ToList();
                
                if (SelectedSkill is SkillGroup skillGroup)
                    newOpenAttributeIncreases = _characterService.AddExperienceToSkillGroup(skillGroup, 1).ToList();
                
                if (newOpenAttributeIncreases.Any())
                    Character.OpenAttributeIncreases.AddRange(newOpenAttributeIncreases);
                OnPropertyChanged(nameof(SelectedSkillExperience));
            }
        }

        public int SelectedSkillModification
        {
            get => SelectedSkill?.ModificationValue ?? 0;
            set
            {
                //catch closing event, when SelectedSkill is set to null
                if (SelectedSkill == null)
                    return;

                if (SelectedSkill is Skill skill)
                    _characterService.SetModificationValue(skill, value);

                if (SelectedSkill is SkillGroup skillGroup)
                    _characterService.SetModificationValue(skillGroup, value);

                OnPropertyChanged(nameof(SelectedSkillModification));
            }
        }

        private SkillGroup _skillParent;

        public ICommand CloseSelectedSkill => new Command(() => SelectedSkill = null);

        public ICommand OpenSelectedSkill => new Command<(SkillGroup SkillGroup, UpgradeableSkillBase SelectedUpgradeableSkill)>(parameter =>
        {
            Debug.WriteLine($"Callback [OpenSelectedSkill]: Skill {parameter.SelectedUpgradeableSkill}, Group {parameter.SkillGroup}" );

            SelectedSkill = parameter.SelectedUpgradeableSkill;

            if (SelectedSkill is Skill skill)
            {
                SelectedSkillName = skill.Type.ToString();
                _skillParent = parameter.SkillGroup;
            }

            if (SelectedSkill is SkillGroup group)
            {
                SelectedSkillName = group.Type.ToString();
                _skillParent = null;
            }

            //todo helper, keinen converter verwenden
            SelectedSkillSourceName = _converter.Convert(parameter.SkillGroup.SkillSource, null, null, CultureInfo.InvariantCulture).ToString();
        });

        public string SelectedSkillName
        {
            get => _selectedSkillName;
            set => SetProperty(ref _selectedSkillName , value);
        }

        public string SelectedSkillSourceName
        {
            get => _selectedSkillSourceName;
            set => SetProperty(ref _selectedSkillSourceName , value);
        }
    }
}
