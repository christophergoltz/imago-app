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
        public CharacterViewModel CharacterViewModel { get; }
        private readonly IRuleRepository _ruleRepository;
        private SkillGroupDetailViewModel _skillGroupDetailViewModel;
        private SkillDetailViewModel _skillDetailViewModel;
        
        public SkillGroupModel Bewegung => CharacterViewModel.Character.SkillGroups[SkillGroupModelType.Bewegung];
        public SkillGroupModel Nahkampf => CharacterViewModel.Character.SkillGroups[SkillGroupModelType.Nahkampf];
        public SkillGroupModel Heimlichkeit => CharacterViewModel.Character.SkillGroups[SkillGroupModelType.Heimlichkeit];
        public SkillGroupModel Fernkampf => CharacterViewModel.Character.SkillGroups[SkillGroupModelType.Fernkampf];
        public SkillGroupModel Webkunst => CharacterViewModel.Character.SkillGroups[SkillGroupModelType.Webkunst];
        public SkillGroupModel Wissenschaft => CharacterViewModel.Character.SkillGroups[SkillGroupModelType.Wissenschaft];
        public SkillGroupModel Handwerk => CharacterViewModel.Character.SkillGroups[SkillGroupModelType.Handwerk];
        public SkillGroupModel Soziales => CharacterViewModel.Character.SkillGroups[SkillGroupModelType.Soziales];

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

        public SkillPageViewModel(CharacterViewModel characterViewModel, IWikiService wikiService, 
            IMasteryRepository masteryRepository,
            ITalentRepository talentRepository,
            IRuleRepository ruleRepository)
        {
            CharacterViewModel = characterViewModel;
            _ruleRepository = ruleRepository;

            OpenSkillDetailCommand = new Command<(SkillModel Skill, SkillGroupModel SkillGroup)>(parameter =>
            {
                var vm = new SkillDetailViewModel(parameter.Skill, parameter.SkillGroup, characterViewModel,
                    wikiService, masteryRepository, talentRepository, _ruleRepository);
                vm.CloseRequested += (sender, args) => { SkillDetailViewModel = null; };

                SkillDetailViewModel = vm;
            });

            OpenSkillGroupDetailCommand = new Command<SkillGroupModel>(group =>
            {
                var vm = new SkillGroupDetailViewModel(group, characterViewModel, wikiService);
                vm.CloseRequested += (sender, args) => { SkillGroupDetailViewModel = null; };

                SkillGroupDetailViewModel = vm;
            });
        }
    }
}