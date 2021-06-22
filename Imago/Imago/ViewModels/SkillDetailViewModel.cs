using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Imago.Converter;
using Imago.Models;
using Imago.Models.Enum;
using Imago.Repository;
using Imago.Repository.WrappingDatabase;
using Imago.Services;
using Imago.Util;
using Imago.Views;
using Xamarin.Forms;

namespace Imago.ViewModels
{
    public class SkillDetailViewModel : BindableBase
    {
        private readonly SkillGroupTypeToAttributeSourceStringConverter _converter =
            new SkillGroupTypeToAttributeSourceStringConverter();

        private readonly SkillGroup _parent;
        private readonly CharacterViewModel _characterViewModel;
        private readonly IWikiService _wikiService;
        private readonly IMasteryRepository _masteryRepository;
        private readonly ITalentRepository _talentRepository;
        private readonly IRuleRepository _ruleRepository;

        public event EventHandler CloseRequested;
        public Skill Skill { get; }

        public ICommand IncreaseExperienceCommand { get; set; }
        public ICommand DecreaseExperienceCommand { get; set; }
        public ICommand OpenWikiCommand { get; set; }
        public ICommand CloseCommand { get; set; }

        private HtmlWebViewSource _quickWikiView;

        public HtmlWebViewSource QuickWikiView
        {
            get => _quickWikiView;
            set => SetProperty(ref _quickWikiView, value);
        }

        private string _sourceFormula;
        private List<TalentListItemViewModel> _talents;
        private List<TalentListItemViewModel> _masteries;
        private int _finalTestValue;
        private List<HandicapListViewItemViewModel> _handicaps;

        public string SourceFormula
        {
            get => _sourceFormula;
            set => SetProperty(ref _sourceFormula, value);
        }

        public int SelectedSkillModification
        {
            get => Skill?.ModificationValue ?? 0;
            set
            {
                Debug.WriteLine("Set SelectedSkillModification to " + value);
                _characterViewModel.SetModificationValue(Skill, value);
                OnPropertyChanged(nameof(SelectedSkillModification));
            }
        }

        public List<TalentListItemViewModel> Talents
        {
            get => _talents;
            set => SetProperty(ref _talents, value);
        }

        public List<TalentListItemViewModel> Masteries
        {
            get => _masteries;
            set => SetProperty(ref _masteries, value);
        }

        public List<HandicapListViewItemViewModel> Handicaps
        {
            get => _handicaps;
            set => SetProperty(ref _handicaps, value);
        }

        public SkillDetailViewModel(Skill skill, SkillGroup parent,CharacterViewModel characterViewModel,
            IWikiService wikiService, IMasteryRepository masteryRepository, ITalentRepository talentRepository, IRuleRepository ruleRepository)
        {
            _parent = parent;
            _characterViewModel = characterViewModel;
            _wikiService = wikiService;
            _masteryRepository = masteryRepository;
            _talentRepository = talentRepository;
            _ruleRepository = ruleRepository;
            Skill = skill;

            SourceFormula = _converter.Convert(parent.Type, null, null, CultureInfo.InvariantCulture).ToString();

            IncreaseExperienceCommand = new Command<int>(experience =>
            {
                var newExp = Skill.TotalExperience + experience;
                _characterViewModel.SetExperienceToSkill(Skill, parent, newExp);
                UpdateTalentRequirements();
                RecalcTestValue();
            });

            //todo parameter; _characterViewModel.SetExperienceToSkill
            DecreaseExperienceCommand = new Command(() => { _characterViewModel.RemoveOneExperienceFromSkill(skill); });

            OpenWikiCommand = new Command(async () =>
            {
                var url = wikiService.GetWikiUrl(skill.Type);

                if (string.IsNullOrWhiteSpace(url))
                {
                    await Application.Current.MainPage.DisplayAlert("Fehlender Link",
                        "Uups, hier ist wohl nichts hinterlegt..", "OK");
                    return;
                }

                WikiPageViewModel.RequestedWikiPage = new WikiPageEntry(url);
                await Shell.Current.GoToAsync($"//{nameof(WikiPage)}");
                WikiPageViewModel.RequestedWikiPage = null;
            });

            CloseCommand = new Command(() => { CloseRequested?.Invoke(this, EventArgs.Empty); });

            Task.Run(LoadWikiPage);
            Task.Run(InitializeTestView);
        }

        public int FinalTestValue
        {
            get => _finalTestValue;
            set => SetProperty(ref _finalTestValue, value);
        }

        private void RecalcTestValue()
        {
            var result = (int) Skill.FinalValue;

            //handicap
            if (Handicaps != null)
            {
                foreach (var handicap in Handicaps)
                {
                    if (handicap.IsChecked)
                        result -= handicap.HandiCapValue ?? 0;
                }
            }

            //masteries
            if (Masteries != null)
            {
                foreach (var mastery in Masteries)
                {
                    if (!mastery.Available)
                        continue;

                    if (mastery.Talent.ActiveUse == false || mastery.Talent.ActiveUse && mastery.InUse)
                        result -= mastery.Talent.Difficulty ?? mastery.DifficultyOverride ?? 0;
                }
            }

            //talents
            if (Talents != null)
            {
                foreach (var talent in Talents)
                {
                    if (!talent.Available)
                        continue;

                    if (talent.Talent.ActiveUse == false || talent.Talent.ActiveUse && talent.InUse)
                        result -= talent.Talent.Difficulty ?? talent.DifficultyOverride ?? 0;
                }
            }

            FinalTestValue = result;
        }

        public void UpdateTalentRequirements()
        {
            if (Masteries != null)
            {
                foreach (var mastery in Masteries)
                {
                    if (mastery.Talent is MasteryModel model)
                    {
                        var avaiable = _characterViewModel.CheckMasteryRequirement(model.Requirements);
                        mastery.Available = avaiable;
                    }
                }
            }

            if (Talents != null)
            {
                foreach (var talent in Talents)
                {
                    if (talent.Talent is TalentModel model)
                    {
                        var avaiable = _characterViewModel.CheckTalentRequirement(model.Requirements);
                        talent.Available = avaiable;
                    }
                }
            }
        }

        private static readonly List<(DerivedAttributeType Type, string Text, string IconSource)> HandicapDefinition =
            new List<(DerivedAttributeType Type, string Text, string IconSource)>()
            {
                (DerivedAttributeType.BehinderungKampf, "Kampf", "swords.png"),
                (DerivedAttributeType.BehinderungAbenteuer, "Abenteuer / Reise", "backpack.png"),
                (DerivedAttributeType.BehinderungGesamt, "Gesamt", null),
                (DerivedAttributeType.Unknown, "Ignorieren", null)
            };

        private async Task InitializeTestView()
        {
            //masteries
            var masteries = new List<TalentListItemViewModel>();
            var allMasteries = await _masteryRepository.GetAllItemsAsync();
            foreach (var mastery in allMasteries)
            {
                if (mastery.TargetSkill != _parent.Type)
                    continue;

                var avaiable = _characterViewModel.CheckMasteryRequirement(mastery.Requirements);

                var vm = new TalentListItemViewModel(mastery)
                {
                    Available = avaiable
                };
                vm.TalentValueChanged += (sender, args) => RecalcTestValue();
                masteries.Add(vm);
            }

            Masteries = masteries;

            //talents
            var talents = new List<TalentListItemViewModel>();
            var allTalents = await _talentRepository.GetAllItemsAsync();
            foreach (var talent in allTalents)
            {
                if (talent.TargetSkill != Skill.Type)
                    continue;

                var avaiable = _characterViewModel.CheckTalentRequirement(talent.Requirements);

                var vm = new TalentListItemViewModel(talent)
                {
                    Available = avaiable
                };
                vm.TalentValueChanged += (sender, args) => RecalcTestValue();
                talents.Add(vm);
            }

            Talents = talents;

            //handicap
            var handicaps = new List<HandicapListViewItemViewModel>();
            if (_ruleRepository.GetSkillGroupSources(_parent.Type).Contains(AttributeType.Geschicklichkeit))
            {
                //only add handicap for attributessource with Geschicklichkeit
                foreach (var tuple in HandicapDefinition)
                {
                    var handicapValue = tuple.Type == DerivedAttributeType.Unknown
                        ? (int?) null
                        : (int)_characterViewModel.DerivedAttributes.First(attribute => attribute.Type == tuple.Type).FinalValue;

                    //todo converter
                    var vm = new HandicapListViewItemViewModel(tuple.Type, false, handicapValue, tuple.IconSource,
                        tuple.Text);
                    vm.HandicapValueChanged += (sender, args) => RecalcTestValue();

                    if (vm.Type == DerivedAttributeType.BehinderungAbenteuer)
                        vm.IsChecked = true;

                    handicaps.Add(vm);
                }
            }

            Handicaps = handicaps;

            UpdateTalentRequirements();
            RecalcTestValue();
        }

        private void LoadWikiPage()
        {
            var html = _wikiService.GetTalentHtml(Skill.Type);
            var url = _wikiService.GetWikiUrl(Skill.Type);
            QuickWikiView = new HtmlWebViewSource()
            {
                BaseUrl = url,
                Html = html
            };
        }
    }
}