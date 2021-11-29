using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Input;
using ImagoApp.Application;
using ImagoApp.Application.Models;
using ImagoApp.Application.Services;
using ImagoApp.Shared.Attributes;
using ImagoApp.Shared.Enums;
using Xamarin.Forms;

namespace ImagoApp.ViewModels
{
    public class DiceSearchModelGroup : List<DiceSearchModel>
    {
        public string Name { get; private set; }

        public DiceSearchModelGroup(string name, List<DiceSearchModel> models) : base(models)
        {
            Name = name;
        }
    }

    public class DiceSearchModel
    {
        public object Value { get; set; }
        public string DisplayText { get; set; }
        public DiceSearchModelType Type { get; set; }
    }

    public enum DiceSearchModelType
    {
        Skill,
        SkillGroup,
        WeaveTalent
    }

    public class DicePageViewModel : BindableBase
    {
        public CharacterViewModel CharacterViewModel { get; }
        private readonly IWikiService _wikiService;
        private readonly IWikiDataService _wikiDataService;

        public DicePageViewModel(CharacterViewModel characterViewModel, IWikiService wikiService,
            IWikiDataService wikiDataService)
        {
            CharacterViewModel = characterViewModel;
            CharacterViewModel.LoadoutViewModel.LoadoutValueChanged += (sender, args) => RecalculateFinalDiceValue();

            ConcentrationPerAction = 15;

            _wikiService = wikiService;
            _wikiDataService = wikiDataService;
            _selectableDiceTypes = CreateSearchList();

            Search(string.Empty);
        }

        private List<DiceSearchModelGroup> CreateSearchList()
        {
            var skills = new List<DiceSearchModel>();
            var skillGroups = new List<DiceSearchModel>();

            //skills and skillgroups
            foreach (var skillGroup in CharacterViewModel.CharacterModel.SkillGroups)
            {
                //skillgroup
                var skillGroupNameAttribute = Util.EnumExtensions.GetAttribute<DisplayTextAttribute>(skillGroup.Type);
                skillGroups.Add(new DiceSearchModel()
                {
                    Value = skillGroup.Type,
                    DisplayText = skillGroupNameAttribute?.Text ?? skillGroup.Type.ToString(),
                    Type = DiceSearchModelType.SkillGroup
                });

                foreach (var skill in skillGroup.Skills)
                {
                    //skill
                    var skillNameAttribute = Util.EnumExtensions.GetAttribute<DisplayTextAttribute>(skill.Type);
                    skills.Add(new DiceSearchModel()
                    {
                        Value = skill.Type,
                        DisplayText = skillNameAttribute?.Text ?? skill.Type.ToString(),
                        Type = DiceSearchModelType.Skill
                    });
                }
            }

            var result = new List<DiceSearchModelGroup>()
            {
                new DiceSearchModelGroup("Fertigkeit", skills.OrderBy(model => model.DisplayText).ToList()),
                new DiceSearchModelGroup("Fertigkeitsgruppe", skillGroups.OrderBy(model => model.DisplayText).ToList())
            };

            //weave talents
            var allWeaveTalents = _wikiDataService.GetAllWeaveTalents();
            var weaveTalentGroups = allWeaveTalents.GroupBy(model => model.WeaveSource).ToList();
            foreach (var weaveTalentGroup in weaveTalentGroups)
            {
                //group
                var weaveTalents = new List<DiceSearchModel>();

                foreach (var weaveTalent in weaveTalentGroup)
                {
                    //talent
                    var available = CharacterViewModel.CheckTalentRequirement(weaveTalent.Requirements);
                    if (!available)
                        continue;

                    weaveTalents.Add(new DiceSearchModel()
                    {
                        Type = DiceSearchModelType.WeaveTalent,
                        DisplayText = weaveTalent.Name,
                        Value = weaveTalent
                    });
                }

                if (weaveTalents.Any())
                {
                    result.Add(new DiceSearchModelGroup(weaveTalentGroup.Key, weaveTalents));
                }
            }

            return result;
        }

        private readonly List<DiceSearchModelGroup> _selectableDiceTypes;
        private List<DiceSearchModelGroup> _searchResults;

        public List<DiceSearchModelGroup> SearchResults
        {
            get => _searchResults;
            set => SetProperty(ref _searchResults, value);
        }

        private HtmlWebViewSource _wikiSource;
        private SkillGroupModel _selectedSkillGroup;
        private SkillViewModel _selectedSkill;
        private WeaveTalentModel _selectedWeaveTalent;
        private TalentViewModel _talentViewModel;
        private MasteryViewModel _masteryViewModel;
        private int _finalDiceValue;
        private bool _searchresultVisible;

        public HtmlWebViewSource WikiSource
        {
            get => _wikiSource;
            set => SetProperty(ref _wikiSource, value);
        }

        public SkillGroupModel SelectedSkillGroup
        {
            get => _selectedSkillGroup;
            private set => SetProperty(ref _selectedSkillGroup, value);
        }


        private ICommand _closeSearchbarCommand;
        private int _diceModification;

        public ICommand CloseSearchbarCommand => _closeSearchbarCommand ?? (_closeSearchbarCommand = new Command(() =>
        {
            SearchresultVisible = false;
        }));

        public bool SearchresultVisible
        {
            get => _searchresultVisible;
            set => SetProperty(ref _searchresultVisible, value);
        }

        public SkillViewModel SelectedSkill
        {
            get => _selectedSkill;
            private set => SetProperty(ref _selectedSkill, value);
        }

        public WeaveTalentModel SelectedWeaveTalent
        {
            get => _selectedWeaveTalent;
            private set => SetProperty(ref _selectedWeaveTalent, value);
        }

        public TalentViewModel TalentViewModel
        {
            get => _talentViewModel;
            set => SetProperty(ref _talentViewModel, value);
        }

        public MasteryViewModel MasteryViewModel
        {
            get => _masteryViewModel;
            set => SetProperty(ref _masteryViewModel, value);
        }

        public int FinalDiceValue
        {
            get => _finalDiceValue;
            set => SetProperty(ref _finalDiceValue, value);
        }

        public int DiceModification
        {
            get => _diceModification;
            set
            {
                SetProperty(ref _diceModification, value);
                RecalculateFinalDiceValue();
            }
        }

        public void SetSelection(DiceSearchModelType type, object value)
        {
            var z = _selectableDiceTypes.SelectMany(group => group)
                .Where(model => model.Type == type)
                .First(model => (SkillModelType)model.Value == (SkillModelType)value);

            SetSelectedItem(z);
        }

        private bool _allDifficultyRemoved;
        private int _concentrationPerAction;
        private int _concentrationQuantity;

        public int ConcentrationPerAction
        {
            get => _concentrationPerAction;
            set
            {
                SetProperty(ref _concentrationPerAction, value);
                OnPropertyChanged(nameof(ConcentrationFinalValue));
                RecalculateFinalDiceValue();
            }
        }

        public int ConcentrationQuantity
        {
            get => _concentrationQuantity;
            set
            {
                SetProperty(ref _concentrationQuantity, value);
                OnPropertyChanged(nameof(ConcentrationFinalValue));
                RecalculateFinalDiceValue();
            }
        }

        public bool AllDifficultyRemoved
        {
            get => _allDifficultyRemoved;
            set
            {
                SetProperty(ref _allDifficultyRemoved, value);
                RecalculateFinalDiceValue();
            }
        }

        public List<WeaveTalentResultModel> WeaveTalentResults
        {
            get => _weaveTalentResults;
            set => SetProperty(ref _weaveTalentResults, value);
        }

        private List<WeaveTalentSettingModel> _weaveTalentSettings;
        private List<WeaveTalentResultModel> _weaveTalentResults;

        public List<WeaveTalentSettingModel> WeaveTalentSettings
        {
            get => _weaveTalentSettings;
            set => SetProperty(ref _weaveTalentSettings, value);
        }

        private SkillViewModel _selectedWeaveSkillModel;
        public SkillViewModel SelectedWeaveSkillModel
        {
            get => _selectedWeaveSkillModel;
            set
            {
                SetProperty(ref _selectedWeaveSkillModel, value);
                if (value != null)
                    SetWeaveWikiSource(value.Skill.Type);
                RecalculateFinalDiceValue();
            }
        }


        public int ConcentrationFinalValue => ConcentrationPerAction * ConcentrationQuantity;

        private List<SkillViewModel> _weaveSourceSkills;
        public List<SkillViewModel> WeaveSourceSkills
        {
            get => _weaveSourceSkills;
            private set => SetProperty(ref _weaveSourceSkills, value);
        }

        public void SetSelectedItem(DiceSearchModel selection)
        {
            SearchresultVisible = false;
            DiceModification = 0;

            //reset all
            SelectedSkill = null;
            SelectedSkillGroup = null;

            TalentViewModel = null;
            MasteryViewModel = null;

            SelectedWeaveTalent = null;
            WeaveSourceSkills = null;
            WeaveTalentSettings = null;
            WeaveTalentResults = null;

            if (selection == null)
            {
                RecalculateFinalDiceValue();
                return;
            }

            switch (selection.Value)
            {
                case SkillModelType skillModelType:
                {
                    var group = CharacterViewModel.CharacterModel.SkillGroups.First(groupModel =>
                        groupModel.Skills.Any(e => e.Type == skillModelType));

                    var skillModel = @group.Skills.First(skill => skill.Type == skillModelType);
                    SelectedSkill = new SkillViewModel(skillModel, @group, CharacterViewModel);
                    SelectedSkill.PropertyChanged += (sender, args) =>
                    {
                        if (args.PropertyName != nameof(SkillViewModel.ModificationValue))
                            RecalculateFinalDiceValue();
                    };

                    var html = _wikiService.GetTalentHtml(skillModelType, @group.Type);
                    var url = _wikiService.GetWikiUrl(skillModelType);
                    WikiSource = new HtmlWebViewSource()
                    {
                        BaseUrl = url,
                        Html = html
                    };

                    TalentViewModel = new TalentViewModel(_wikiDataService.GetAllTalents(SelectedSkill.Skill.Type),
                        CharacterViewModel);
                    TalentViewModel.TalentValueChanged += (sender, args) => RecalculateFinalDiceValue();
                    MasteryViewModel =
                        new MasteryViewModel(_wikiDataService.GetAllMasteries(@group.Type), CharacterViewModel);
                    MasteryViewModel.MasteryValueChanged += (sender, args) => RecalculateFinalDiceValue();

                    RecalculateFinalDiceValue();
                    return;
                }
                case SkillGroupModelType skillGroupModelType:
                {
                    var skillGroupModel =
                        CharacterViewModel.CharacterModel.SkillGroups.First(group =>
                            @group.Type == skillGroupModelType);
                    SelectedSkillGroup = skillGroupModel;
                    SelectedSkillGroup.PropertyChanged += (sender, args) =>
                    {
                        if (args.PropertyName != nameof(SkillGroupViewModel.SkillGroupModificationValue))
                            RecalculateFinalDiceValue();
                    };

                    var html = _wikiService.GetMasteryHtml(skillGroupModelType);
                    var url = _wikiService.GetWikiUrl(skillGroupModelType);
                    WikiSource = new HtmlWebViewSource()
                    {
                        BaseUrl = url,
                        Html = html
                    };
                    MasteryViewModel = new MasteryViewModel(_wikiDataService.GetAllMasteries(skillGroupModel.Type),
                        CharacterViewModel);
                    MasteryViewModel.MasteryValueChanged += (sender, args) => RecalculateFinalDiceValue();

                    RecalculateFinalDiceValue();
                    return;
                }
                case WeaveTalentModel weaveTalent:
                {
                    SelectedWeaveTalent = weaveTalent;
                    var webkunst = CharacterViewModel.CharacterModel.SkillGroups.First(model =>
                        model.Type == SkillGroupModelType.Webkunst);

                    var list = new List<SkillViewModel>();
                    foreach (var skill in GetSkillsFromRequirements(weaveTalent.Requirements))
                    {
                        var vm = new SkillViewModel(skill, webkunst, CharacterViewModel);
                        vm.PropertyChanged += (sender, args) =>
                        {
                            if (args.PropertyName != nameof(SkillViewModel.ModificationValue))
                                RecalculateFinalDiceValue();
                        };
                        list.Add(vm);
                    }

                    WeaveSourceSkills = list;

                    SelectedWeaveSkillModel = WeaveSourceSkills.First();

                    WeaveTalentResults = CreateWeaveTalentResults(weaveTalent.DifficultyFormula,
                        weaveTalent.RangeFormula, weaveTalent.DurationFormula, weaveTalent.CorrosionFormula);
                    WeaveTalentSettings = GetWeaveTalentSettings(weaveTalent.FormulaSettings);

                    MasteryViewModel = new MasteryViewModel(_wikiDataService.GetAllMasteries(webkunst.Type),
                        CharacterViewModel);
                    MasteryViewModel.MasteryValueChanged += (sender, args) => RecalculateFinalDiceValue();

                    SetWeaveWikiSource(SelectedWeaveSkillModel.Skill.Type);

                    RecalculateFinalDiceValue();
                    return;
                }
            }
        }

        private void SetWeaveWikiSource(SkillModelType skillModelType)
        {
            var html = _wikiService.GetTalentHtml(skillModelType, SkillGroupModelType.Webkunst);
            var url = _wikiService.GetWikiUrl(skillModelType);
            WikiSource = new HtmlWebViewSource()
            {
                BaseUrl = url,
                Html = html
            };
        }

        public int WeaveTalentDifficulty
        {
            get
            {
                if (WeaveTalentResults == null)
                    return 0;

                var rawValue = WeaveTalentResults.FirstOrDefault(model => model.Type == WeaveTalentResultType.Difficulty)?.FinalValue ?? "0";
                return int.TryParse(rawValue, out var difficulty) ? difficulty : 0;
            }
        }

        private List<WeaveTalentResultModel> CreateWeaveTalentResults(string difficultyFormula, string rangeFormula, string durationFormula, string corrosionFormula)
        {
            var diff = new WeaveTalentResultModel(WeaveTalentResultType.Difficulty, difficultyFormula);
            diff.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(WeaveTalentResultModel.FinalValue))
                {
                    OnPropertyChanged(nameof(WeaveTalentDifficulty));
                    RecalculateFinalDiceValue();
                }
            };

            var result = new List<WeaveTalentResultModel>()
            {
                diff,
                new WeaveTalentResultModel(WeaveTalentResultType.Range, rangeFormula),
                new WeaveTalentResultModel(WeaveTalentResultType.Duration, durationFormula),
                new WeaveTalentResultModel(WeaveTalentResultType.Corrosion, corrosionFormula)
            };

            foreach (var model in result)
            {
                model.RecalculateFinalValue(new Dictionary<string, string>());
            }

            return result;
        }

        private List<WeaveTalentSettingModel> GetWeaveTalentSettings(string formulaSettings)
        {
            if (string.IsNullOrWhiteSpace(formulaSettings))
                return null;

            formulaSettings = formulaSettings.Replace(" ", string.Empty);

            var splitSettings = formulaSettings.Split(';');
            var settings = splitSettings.Select(GetSettingFromString).ToList();

            foreach (var setting in settings)
            {
                setting.PropertyChanged += (sender, args) =>
                {
                    if (args.PropertyName != nameof(WeaveTalentSettingModel.FinalValue))
                        return;

                    var dic = WeaveTalentSettings.ToDictionary(
                        _ => _.Abbreviation,
                        _ => _.FinalValue.ToString(CultureInfo.InvariantCulture));

                    //recalc all results
                    foreach (var item in WeaveTalentResults)
                    {
                        item.RecalculateFinalValue(dic);
                    }
                };
            }

            return settings;
        }

        private WeaveTalentSettingModel GetSettingFromString(string setting)
        {
            //settings without units
            switch (setting)
            {
                case "S":
                    return new WeaveTalentSettingModel()
                    {
                        Abbreviation = setting,
                        Type = WeaveTalentSettingModelType.StrengthOfTalent,
                        Quantity = 1,
                        StepValue = 1
                    };
                case "Sw":
                    return new WeaveTalentSettingModel()
                    {
                        Abbreviation = setting,
                        Type = WeaveTalentSettingModelType.StrengthOfWorld,
                        Quantity = 1,
                        StepValue = 1
                    };
            }

            if (!setting.Contains('['))
                throw new InvalidOperationException($"Setting-formula couldn't be read: \"{setting}\"");

            var parts = setting.Split('[');

            //unit parsing
            var abbreviation = parts.First();

            WeaveTalentSettingModelType type;

            switch (abbreviation)
            {
                case "R":
                    type = WeaveTalentSettingModelType.RadiusRange;
                    break;
                case "D":
                    type = WeaveTalentSettingModelType.DiameterRange;
                    break;
                case "t":
                    type = WeaveTalentSettingModelType.Duration;
                    break;
                case "Vo":
                    type = WeaveTalentSettingModelType.ObjectVolume;
                    break;
                case "Vp":
                    type = WeaveTalentSettingModelType.ProductVolume;
                    break;
                case "VdB":
                    type = WeaveTalentSettingModelType.ImageVolume;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("Unknown abbreviation: " + abbreviation);
            }

            var unitRawValue = parts.Last().TrimEnd(']');

            double step = 1;

            //parse step from substring
            var stepValueMatch = Regex.Match(unitRawValue, "(([0-9]*[,])?[0-9]+)");
            if (stepValueMatch.Success)
            {
                step = double.Parse(stepValueMatch.Value);
            }

            //parse unit from substring
            var unit = Regex.Match(unitRawValue, "(h|m|d|w|y|M|P|L)").Value;

            switch (unit)
            {
                case "h":
                    unit = "Stunden";
                    break;
                case "m":
                    unit = "Minuten";
                    break;
                case "d":
                    unit = "Tage";
                    break;
                case "w":
                    unit = "Wochen";
                    break;
                case "y":
                    unit = "Jahre";
                    break;
                case "M":
                    unit = "Meter";
                    break;
                case "P":
                    unit = "Phasen";
                    break;
                case "L":
                    unit = "Liter";
                    break;
            }



            return new WeaveTalentSettingModel
            {
                Type = type,
                Abbreviation = abbreviation,
                StepValue = step,
                Unit = unit,
                Quantity = 1
            };
        }

        public void Search(string searchValue)
        {
            if (string.IsNullOrWhiteSpace(searchValue))
            {
                //show all
                var searchResult = new List<DiceSearchModelGroup>();

                //copy list into searchresults to prevent ref removing
                foreach (var group in _selectableDiceTypes)
                {
                    searchResult.Add(new DiceSearchModelGroup(group.Name, group));
                }

                SearchResults = searchResult;
                return;
            }

            var result = new List<DiceSearchModelGroup>();
            //copy list into searchresults to prevent ref removing
            foreach (var group in _selectableDiceTypes)
            {
                var newGroup = new DiceSearchModelGroup(group.Name, group);
                foreach (var possibleHit in group)
                {
                    var match = CultureInfo.InvariantCulture.CompareInfo.IndexOf(possibleHit.DisplayText, searchValue,
                        CompareOptions.IgnoreCase) >= 0;

                    if (!match)
                        newGroup.Remove(possibleHit);
                }

                result.Add(newGroup);
            }

            SearchResults = result;
        }

        private void RecalculateFinalDiceValue()
        {
            double value = 0.0;

            if (SelectedSkill != null)
                value = SelectedSkill.Skill.FinalValue;
            if (SelectedSkillGroup != null)
                value = SelectedSkillGroup.FinalValue;
            if (SelectedWeaveTalent != null)
                value = SelectedWeaveSkillModel.Skill.FinalValue;

            if (SelectedSkill != null || SelectedSkillGroup != null)
            {
                //only add handicap if skill or skillgroup
                value -= CharacterViewModel.LoadoutViewModel.GetLoadoutValue();
            }

            if (TalentViewModel != null)
                value -= TalentViewModel.GetDifficultyValue();

            if (MasteryViewModel != null)
                value -= MasteryViewModel.GetDifficultyValue();

            if (SelectedWeaveTalent != null)
            {
                if (!AllDifficultyRemoved)
                {
                    value += ConcentrationFinalValue;
                    value -= WeaveTalentDifficulty;
                }
            }
            else
            {
                value += ConcentrationFinalValue;
            }

            value += DiceModification;

            FinalDiceValue = value.GetRoundedValue();
        }

        private List<SkillModel> GetSkillsFromRequirements(List<SkillRequirementModel> requirements)
        {  //todo obsolete?
            var result = new List<SkillModel>();
            foreach (var requirementModel in requirements)
            {
                if (requirementModel.Type == SkillModelType.Philosophie)
                    continue;

                var item = CharacterViewModel.CharacterModel.SkillGroups.SelectMany(model => model.Skills)
                    .First(model => model.Type == requirementModel.Type);
                result.Add(item);
            }

            return result;
        }

    }
}