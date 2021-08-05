using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ImagoApp.Application;
using ImagoApp.Util;

namespace ImagoApp.ViewModels
{
    public class DatabaseInfoViewModel : BindableBase
    {
        private int _armorTemplateCount;
        private int _weaponTemplateCount;
        private int _talentTemplateCount;
        private int _masteryTemplateCount;
        private FileInfo _wikiDatabaseInfo;
        private FileInfo _characterDatabaseInfo;

        public int ArmorTemplateCount
        {
            get => _armorTemplateCount;
            set => SetProperty(ref _armorTemplateCount, value);
        }

        public int WeaponTemplateCount
        {
            get => _weaponTemplateCount;
            set => SetProperty(ref _weaponTemplateCount, value);
        }

        public int TalentTemplateCount
        {
            get => _talentTemplateCount;
            set => SetProperty(ref _talentTemplateCount, value);
        }

        public int WeaveTalentTemplateCount
        {
            get => _talentTemplateCount;
            set => SetProperty(ref _talentTemplateCount, value);
        }

        public int MasteryTemplateCount
        {
            get => _masteryTemplateCount;
            set => SetProperty(ref _masteryTemplateCount, value);
        }

        public FileInfo WikiDatabaseInfo
        {
            get => _wikiDatabaseInfo;
            set => SetProperty(ref _wikiDatabaseInfo, value);
        }

        public FileInfo CharacterDatabaseInfo
        {
            get => _characterDatabaseInfo;
            set => SetProperty(ref _characterDatabaseInfo, value);
        }
    }
}