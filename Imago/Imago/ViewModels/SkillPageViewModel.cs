using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Input;
using Imago.Converter;
using Imago.Models;
using Imago.Models.Base;
using Imago.Util;
using Xamarin.Forms;

namespace Imago.ViewModels
{
    public class SkillPageViewModel : BindableBase
    {
        private AttributeSkillSourceToStringConverter _converter = new AttributeSkillSourceToStringConverter();
        private UpgradeableSkillBase _selectedSkill;
        private string _selectedSkillName;
        private string _selectedSkillSourceName;
        private string _selectedSkillSourceValueText;

        public SkillPageViewModel(Character character)
        {
            Character = character;
        }

        public Character Character { get; private set; }

        public UpgradeableSkillBase SelectedSkill
        {
            get => _selectedSkill;
            set => SetProperty(ref _selectedSkill, value);
        }

        public ICommand CloseSelectedSkill => new Command(() => SelectedSkill = null);

        public ICommand OpenSelectedSkill => new Command<(SkillGroup SkillGroup, UpgradeableSkillBase SelectedUpgradeableSkill)>(parameter =>
        {
            SelectedSkill = parameter.SelectedUpgradeableSkill;

            if (SelectedSkill is Skill skill)
            {
                SelectedSkillName = skill.Type.ToString();
              //  SelectedSkillSourceName
            }

            if (SelectedSkill is SkillGroup group)
            {
                SelectedSkillName = group.Type.ToString();
            }

            SelectedSkillSourceName = _converter.Convert(parameter.SkillGroup.SkillSource, null, null, CultureInfo.InvariantCulture).ToString();
            
            //todo values bzw. kein converter verwenden
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
