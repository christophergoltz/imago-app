﻿using System;
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

        public ICommand IncreaseExperienceCommand { get; set; }
        public ICommand DecreaseExperienceCommand { get; set; }
        
        public SkillPageViewModel(Character character, ICharacterService characterService)
        {
            _characterService = characterService;
            Character = character;

            IncreaseExperienceCommand = new Command(() =>
            {
                if (SelectedSkill is SkillGroup)
                    throw new InvalidOperationException("Cannot change Experience of Skillgroup via UI");
                
                var newOpenAttributeIncreases = _characterService.AddExperienceToSkill((Skill)SelectedSkill, _skillParent, 1).ToList();
                if (newOpenAttributeIncreases.Any())
                    Character.OpenAttributeIncreases.AddRange(newOpenAttributeIncreases);
            });

            DecreaseExperienceCommand = new Command(() =>
            {
                if (SelectedSkill is SkillGroup)
                    throw new InvalidOperationException("Cannot change Experience of Skillgroup via UI");

                var newOpenAttributeIncreases = _characterService.AddExperienceToSkill((Skill)SelectedSkill, _skillParent, -1).ToList();
                if (newOpenAttributeIncreases.Any())
                    Character.OpenAttributeIncreases.AddRange(newOpenAttributeIncreases);
            });
        }

        public Character Character { get; private set; }

        public UpgradeableSkillBase SelectedSkill
        {
            get => _selectedSkill;
            set
            {
                Debug.WriteLine("Set [SelectedSkill] to " + (value == null ? "null" : value.ToString()));
                SetProperty(ref _selectedSkill, value);
            }
        }
        
        public int SelectedSkillModification
        {
            get => SelectedSkill?.ModificationValue ?? 0;
            set
            {
                Debug.WriteLine("Set SelectedSkillModification to " + value);

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

        public ICommand CloseSelectedSkill => new Command(() =>
        {
            Debug.WriteLine("Closing detail dialog");
            SelectedSkill = null;
        });

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
            
            //update dependet properties
            OnPropertyChanged(nameof(SelectedSkillModification));
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
