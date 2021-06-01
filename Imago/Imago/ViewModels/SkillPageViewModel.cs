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
        private readonly SkillGroupTypeToAttributeSourceStringConverter _converter = new SkillGroupTypeToAttributeSourceStringConverter();
        private SkillBase _selectedSkill;
        private string _selectedSkillName;
        private string _selectedSkillSourceName;
        private SkillGroup _skillParent;
        private bool _isSelectedSkillNotAGroup;

        public Character Character { get; private set; }

        public SkillGroup Bewegung => Character.SkillGroups[SkillGroupType.Bewegung];
        public SkillGroup Nahkampf => Character.SkillGroups[SkillGroupType.Nahkampf];
        public SkillGroup Heimlichkeit => Character.SkillGroups[SkillGroupType.Heimlichkeit];
        public SkillGroup Fernkampf => Character.SkillGroups[SkillGroupType.Fernkampf];
        public SkillGroup Webkunst => Character.SkillGroups [SkillGroupType.Webkunst];
        public SkillGroup Wissenschaft => Character.SkillGroups[SkillGroupType.Wissenschaft];
        public SkillGroup Handwerk => Character.SkillGroups[SkillGroupType.Handwerk];
        public SkillGroup Soziales => Character.SkillGroups[SkillGroupType.Soziales];

        public SkillBase SelectedSkill
        {
            get => _selectedSkill;
            set
            {
                Debug.WriteLine("Set [SelectedSkill] to " + (value == null ? "null" : value.ToString()));
                SetProperty(ref _selectedSkill, value);
            }
        }

        public bool IsSelectedSkillNotAGroup
        {
            get => _isSelectedSkillNotAGroup;
            set => SetProperty(ref _isSelectedSkillNotAGroup, value);
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

        public string SelectedSkillName
        {
            get => _selectedSkillName;
            set => SetProperty(ref _selectedSkillName, value);
        }

        public string SelectedSkillSourceName
        {
            get => _selectedSkillSourceName;
            set => SetProperty(ref _selectedSkillSourceName, value);
        }

        public int ExperienceReqiredForLevelUp
        {
            get
            {
                if (SelectedSkill == null) 
                    return 0;

                if (SelectedSkill is SkillGroup group)
                    return SkillIncreaseHelper.GetExperienceForNextSkillGroupLevel(group.IncreaseValue) - SelectedSkill.ExperienceValue;
                
                if (SelectedSkill is Skill skill)
                    return SkillIncreaseHelper.GetExperienceForNextSkillLevel(skill.IncreaseValue) - SelectedSkill.ExperienceValue;

                throw new InvalidOperationException($"Unknown SelectedSkill type: {SelectedSkill.GetType()} for [{nameof(ExperienceReqiredForLevelUp)}]");
            }
        }

        public ICommand IncreaseExperienceCommand { get; set; }
        public ICommand DecreaseExperienceCommand { get; set; }
        public ICommand OpenSelectedSkill { get; set; }
        public ICommand CloseSelectedSkill { get; set; }

        public SkillPageViewModel(Character character, ICharacterService characterService)
        {
            _characterService = characterService;
            Character = character;

            IncreaseExperienceCommand = new Command(() =>
            {
                if (SelectedSkill is SkillGroup)
                    throw new InvalidOperationException("Cannot change Experience of Skillgroup via UI");

                var newOpenAttributeIncreases =
                    _characterService.AddOneExperienceToSkill((Skill) SelectedSkill, _skillParent).ToList();
                if (newOpenAttributeIncreases.Any())
                {
                    foreach (var increase in newOpenAttributeIncreases)
                    {
                        Character.OpenAttributeIncreases.Add(increase);
                    }
                }
                OnPropertyChanged(nameof(ExperienceReqiredForLevelUp));
            });

            DecreaseExperienceCommand = new Command(() =>
            {
                if (SelectedSkill is SkillGroup)
                    throw new InvalidOperationException("Cannot change Experience of Skillgroup via UI");

                _characterService.RemoveOneExperienceFromSkill((Skill) SelectedSkill);
                OnPropertyChanged(nameof(ExperienceReqiredForLevelUp));
            });

            OpenSelectedSkill = new Command<(SkillGroup SkillGroup, SkillBase SelectedUpgradeableSkill)>(
                parameter =>
                {
                    Debug.WriteLine(
                        $"Callback [OpenSelectedSkill]: Skill {parameter.SelectedUpgradeableSkill}, Group {parameter.SkillGroup}");

                    SelectedSkill = parameter.SelectedUpgradeableSkill;

                    if (SelectedSkill is Skill skill)
                    {
                        IsSelectedSkillNotAGroup = true;
                        SelectedSkillName = skill.Type.ToString();
                        _skillParent = parameter.SkillGroup;
                    }

                    if (SelectedSkill is SkillGroup group)
                    {
                        IsSelectedSkillNotAGroup = false;
                        SelectedSkillName = group.Type.ToString();
                        _skillParent = null;
                    }

                    //todo helper, keinen converter verwenden
                    SelectedSkillSourceName = _converter.Convert(parameter.SkillGroup.Type, null, null,
                        CultureInfo.InvariantCulture).ToString();

                    //update dependet properties
                    OnPropertyChanged(nameof(SelectedSkillModification));
                    OnPropertyChanged(nameof(ExperienceReqiredForLevelUp));
                });

            CloseSelectedSkill = new Command(() =>
            {
                Debug.WriteLine("Closing detail dialog");
                SelectedSkill = null;
            });

        }
    }
}