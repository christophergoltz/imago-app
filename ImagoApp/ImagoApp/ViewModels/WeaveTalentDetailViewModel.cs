using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Input;
using ImagoApp.Application;
using ImagoApp.Application.Models;
using ImagoApp.Application.Services;
using ImagoApp.Shared.Enums;
using Xamarin.Forms;

namespace ImagoApp.ViewModels
{
    public class WeaveTalentDetailViewModel : BindableBase
    {
        private readonly CharacterViewModel _characterViewModel;
        private readonly IWikiDataService _wikiDataService;
        private WeaveTalentModel _weaveTalent;
        private List<SkillModel> _skills;
        private List<MasteryListItemViewModel> _masteries;
        private SkillModel _selectedSkillModel;
        private int _concentrationPerAction;
        private int _concentrationQuantity;
        private int _modification;
        private int _finalValue;
        private List<WeaveTalentSettingModel> _weaveTalentSettings;
        private List<WeaveTalentResultModel> _weaveTalentResults;
        private bool _allDifficultyRemoved;
        public ICommand CloseCommand { get; set; }

        public event EventHandler CloseRequested;

        public int Difficulty
        {
            get
            {
                if (WeaveTalentResults == null)
                    return 0;

                var rawValue = WeaveTalentResults.FirstOrDefault(model => model.Type == WeaveTalentResultType.Difficulty)?.FinalValue ?? "0";
                return int.TryParse(rawValue, out var difficulty) ? difficulty : 0;
            }
        }

        public event EventHandler<SkillModelType> OpenSkillPageRequested;

        public WeaveTalentDetailViewModel(WeaveTalentModel weaveTalent, List<SkillModel> skills, CharacterViewModel characterViewModel, IWikiDataService wikiDataService)
        {
            _characterViewModel = characterViewModel;
            _wikiDataService = wikiDataService;
            WeaveTalent = weaveTalent;
            Skills = skills;
            ConcentrationPerAction = 15;

            CloseCommand = new Command(() =>
            {
                CloseRequested?.Invoke(this, EventArgs.Empty);
            });
            
            InitializeTestView();
            CreateWeaveTalentResults();
            CreateWeaveTalentSettings(weaveTalent.FormulaSettings);
            RecalculateFinalValue();

            Device.BeginInvokeOnMainThread(() =>
            {
                SelectedSkillModel = Skills.First();
            });
        }

        private void CreateWeaveTalentResults()
        {
            var diff = new WeaveTalentResultModel(WeaveTalentResultType.Difficulty, WeaveTalent.DifficultyFormula);
            diff.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(WeaveTalentResultModel.FinalValue))
                {
                    OnPropertyChanged(nameof(Difficulty));
                    RecalculateFinalValue();
                }
            };

            var result = new List<WeaveTalentResultModel>()
            {
                diff,
                new WeaveTalentResultModel(WeaveTalentResultType.Range, WeaveTalent.RangeFormula),
                new WeaveTalentResultModel(WeaveTalentResultType.Duration, WeaveTalent.DurationFormula),
                new WeaveTalentResultModel(WeaveTalentResultType.Corrosion, WeaveTalent.CorrosionFormula)
            };

            foreach (var model in result)
            {
                model.RecalculateFinalValue(new Dictionary<string, string>());
            }

            WeaveTalentResults = result;
        }

        public List<WeaveTalentSettingModel> WeaveTalentSettings
        {
            get => _weaveTalentSettings;
            set => SetProperty(ref _weaveTalentSettings, value);
        }

        public List<WeaveTalentResultModel> WeaveTalentResults
        {
            get => _weaveTalentResults;
            set => SetProperty(ref _weaveTalentResults, value);
        }

        private void CreateWeaveTalentSettings(string formulaSettings)
        {
            if (string.IsNullOrWhiteSpace(formulaSettings))
                return;

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

            WeaveTalentSettings = settings;
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

        private ICommand _openSkillCommand;

        public ICommand OpenSkillCommand => _openSkillCommand ?? (_openSkillCommand = new Command<SkillModelType>(skill =>
        {
            OpenSkillPageRequested?.Invoke(this, skill);
        }));

        public SkillModel SelectedSkillModel
        {
            get => _selectedSkillModel;
            set
            {
                SetProperty(ref _selectedSkillModel, value);
                RecalculateFinalValue();
            }
        }

        public int ConcentrationPerAction
        {
            get => _concentrationPerAction;
            set
            {
                SetProperty(ref _concentrationPerAction, value);
                OnPropertyChanged(nameof(ConcentrationFinalValue));
                RecalculateFinalValue();
            }
        }

        public int ConcentrationQuantity
        {
            get => _concentrationQuantity;
            set
            {
                SetProperty(ref _concentrationQuantity, value);
                OnPropertyChanged(nameof(ConcentrationFinalValue));
                RecalculateFinalValue();
            }
        }

        public bool AllDifficultyRemoved
        {
            get => _allDifficultyRemoved;
            set
            {
                SetProperty(ref _allDifficultyRemoved, value);
                RecalculateFinalValue();
            }
        }

        public int ConcentrationFinalValue => ConcentrationPerAction * ConcentrationQuantity;

        private void InitializeTestView()
        {
            //masteries
            var masteries = new List<MasteryListItemViewModel>();
            var allMasteries = _wikiDataService.GetAllMasteries(SkillGroupModelType.Webkunst);
            foreach (var mastery in allMasteries)
            {
                var available = _characterViewModel.CheckMasteryRequirement(mastery.Requirements);

                var vm = new MasteryListItemViewModel(mastery)
                {
                    Available = available
                };
                vm.TalentValueChanged += (sender, args) => RecalculateFinalValue();
                masteries.Add(vm);
            }

            Masteries = masteries;
        }

        public int Modification
        {
            get => _modification;
            set
            {
                SetProperty(ref _modification, value); 
                RecalculateFinalValue();
            }
        }

        public int FinalValue
        {
            get => _finalValue;
            set => SetProperty(ref _finalValue, value);
        }

        private void RecalculateFinalValue()
        {
            if (SelectedSkillModel == null)
                return;

            var result = SelectedSkillModel.FinalValue.GetRoundedValue();

            if (!AllDifficultyRemoved)
            {
                result += ConcentrationFinalValue;
                result -= Difficulty;
            }

            result += Modification;

            //masteries
            if (Masteries != null)
            {
                foreach (var mastery in Masteries)
                {
                    if (!mastery.Available)
                        continue;

                    if (mastery.Mastery.ActiveUse == false || mastery.Mastery.ActiveUse && mastery.InUse)
                        result -= mastery.Mastery.Difficulty ?? mastery.DifficultyOverride ?? 0;
                }
            }


            FinalValue = result;
        }

        public List<MasteryListItemViewModel> Masteries
        {
            get => _masteries;
            set => SetProperty(ref _masteries, value);
        }

        public List<SkillModel> Skills
        {
            get => _skills;
            private set => _skills = value;
        }

        public WeaveTalentModel WeaveTalent
        {
            get => _weaveTalent;
            private set => _weaveTalent = value;
        }
    }
}
