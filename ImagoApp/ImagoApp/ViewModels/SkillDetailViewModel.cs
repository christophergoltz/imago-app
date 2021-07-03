using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ImagoApp.Application.Models;
using ImagoApp.Application.Services;
using ImagoApp.Shared.Enums;
using Xamarin.Forms;

namespace ImagoApp.ViewModels
{
    public class SkillDetailViewModel : Util.BindableBase
    {
        private readonly Converter.SkillGroupTypeToAttributeSourceStringConverter _converter =
            new Converter.SkillGroupTypeToAttributeSourceStringConverter();

        private readonly SkillGroupModel _parent;
        private readonly CharacterViewModel _characterViewModel;
        private readonly Services.IWikiService _wikiService;
        private readonly IWikiDataService _wikiDataService;
        private readonly IRuleService _ruleService;

        public event EventHandler CloseRequested;
        public SkillModel SkillModel { get; }

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
            get => SkillModel?.ModificationValue ?? 0;
            set
            {
                Debug.WriteLine("Set SelectedSkillModification to " + value);
                _characterViewModel.SetModificationValue(SkillModel, value);
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

        public SkillDetailViewModel(SkillModel skillModel, SkillGroupModel parent,CharacterViewModel characterViewModel,
            Services.IWikiService wikiService, IWikiDataService wikiDataService, IRuleService ruleService)
        {
            _parent = parent;
            _characterViewModel = characterViewModel;
            _wikiService = wikiService;
            _wikiDataService = wikiDataService;
            _ruleService = ruleService;
            SkillModel = skillModel;

            SourceFormula = _converter.Convert(parent.Type, null, null, CultureInfo.InvariantCulture).ToString();

            IncreaseExperienceCommand = new Command<int>(experience =>
            {
                var newExp = SkillModel.TotalExperience + experience;
                _characterViewModel.SetExperienceToSkill(SkillModel, parent, newExp);
                UpdateTalentRequirements();
                RecalcTestValue();
            });

            //todo parameter; _characterViewModel.SetExperienceToSkill
            DecreaseExperienceCommand = new Command(() => { _characterViewModel.RemoveOneExperienceFromSkill(skillModel); });

            OpenWikiCommand = new Command(async () =>
            {
                var url = wikiService.GetWikiUrl(skillModel.Type);

                if (string.IsNullOrWhiteSpace(url))
                {
                    await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Fehlender Link",
                        "Uups, hier ist wohl nichts hinterlegt..", "OK");
                    return;
                }

                WikiPageViewModel.RequestedWikiPage = new WikiPageEntry(url);
                await Shell.Current.GoToAsync($"//{nameof(Views.WikiPage)}");
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
            var result = (int) SkillModel.FinalValue;

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
                (DerivedAttributeType.BehinderungAbenteuer, "Abenteuer / Reise", "inventar_weiss.png"),
                (DerivedAttributeType.BehinderungGesamt, "Gesamt", null),
                (DerivedAttributeType.Unknown, "Ignorieren", null)
            };

        private async Task InitializeTestView()
        {
            //masteries
            var masteries = new List<TalentListItemViewModel>();
            var allMasteries = await _wikiDataService.GetAllMasteries();
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
            var allTalents = await _wikiDataService.GetAllTalents();
            foreach (var talent in allTalents)
            {
                if (talent.TargetSkillModel != SkillModel.Type)
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
            if (_ruleService.GetSkillGroupSources(_parent.Type).Contains(AttributeType.Geschicklichkeit))
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
            var html = _wikiService.GetTalentHtml(SkillModel.Type);
            var url = _wikiService.GetWikiUrl(SkillModel.Type);
            QuickWikiView = new HtmlWebViewSource()
            {
                BaseUrl = url,
                Html = html
            };
        }
    }
}