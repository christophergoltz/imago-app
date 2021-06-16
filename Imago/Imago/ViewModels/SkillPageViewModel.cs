using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Imago.Converter;
using Imago.Models;
using Imago.Models.Base;
using Imago.Models.Enum;
using Imago.Repository;
using Imago.Repository.WrappingDatabase;
using Imago.Services;
using Imago.Util;
using Imago.Views;
using Xamarin.Forms;

namespace Imago.ViewModels
{
    public class SkillPageViewModel : BindableBase
    {
        private readonly IRuleRepository _ruleRepository;
        private SkillGroupDetailViewModel _skillGroupDetailViewModel;
        private SkillDetailViewModel _skillDetailViewModel;

        public Character Character { get; private set; }

        public SkillGroup Bewegung => Character.SkillGroups[SkillGroupType.Bewegung];
        public SkillGroup Nahkampf => Character.SkillGroups[SkillGroupType.Nahkampf];
        public SkillGroup Heimlichkeit => Character.SkillGroups[SkillGroupType.Heimlichkeit];
        public SkillGroup Fernkampf => Character.SkillGroups[SkillGroupType.Fernkampf];
        public SkillGroup Webkunst => Character.SkillGroups[SkillGroupType.Webkunst];
        public SkillGroup Wissenschaft => Character.SkillGroups[SkillGroupType.Wissenschaft];
        public SkillGroup Handwerk => Character.SkillGroups[SkillGroupType.Handwerk];
        public SkillGroup Soziales => Character.SkillGroups[SkillGroupType.Soziales];

        public ICommand OpenSkillDetailCommand { get; set; }
        public ICommand OpenSkillGroupDetailCommand { get; set; }

        public SkillDetailViewModel SkillDetailViewModel
        {
            get => _skillDetailViewModel;
            set => SetProperty(ref _skillDetailViewModel, value);
        }

        public SkillGroupDetailViewModel SkillGroupDetailViewModel
        {
            get => _skillGroupDetailViewModel;
            set => SetProperty(ref _skillGroupDetailViewModel, value);
        }

        public SkillPageViewModel(Character character, ICharacterService characterService, IWikiService wikiService, 
            IMasteryRepository masteryRepository,
            ITalentRepository talentRepository,
            IRuleRepository ruleRepository)
        {
            _ruleRepository = ruleRepository;
            Character = character;

            OpenSkillDetailCommand = new Command<(Skill Skill, SkillGroup SkillGroup)>(parameter =>
            {
                var vm = new SkillDetailViewModel(parameter.Skill, parameter.SkillGroup, character, characterService,
                    wikiService, masteryRepository, talentRepository, _ruleRepository);
                vm.CloseRequested += (sender, args) => { SkillDetailViewModel = null; };

                SkillDetailViewModel = vm;
            });

            OpenSkillGroupDetailCommand = new Command<SkillGroup>(group =>
            {
                var vm = new SkillGroupDetailViewModel(group, characterService, wikiService);
                vm.CloseRequested += (sender, args) => { SkillGroupDetailViewModel = null; };

                SkillGroupDetailViewModel = vm;
            });
        }
    }
}