using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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
        private int _timeQuantity;
        private int _timeSteps;
        private int _rangeQuantity;
        private double _rangeSteps;
        private bool _isDurationConfigurable;
        private bool _isRangeConfigurable;
        private bool _isTalentLevelConfigurable;
        private int _talentLevel;
        private int _corrosionCost;
        private bool _isVolumeConfigurable;
        private int _volumeQuantity;
        private int _volumeSteps;
        private int _volumeDescription;
        private List<MasteryListItemViewModel> _masteries;
        private SkillModel _selectedSkillModel;
        public ICommand CloseCommand { get; set; }

        public event EventHandler CloseRequested;

        public WeaveTalentDetailViewModel(WeaveTalentModel weaveTalent, List<SkillModel> skills, CharacterViewModel characterViewModel, IWikiDataService wikiDataService)
        {
            _characterViewModel = characterViewModel;
            _wikiDataService = wikiDataService;
            WeaveTalent = weaveTalent;
            Skills = skills;

            CloseCommand = new Command(() =>
            {
                CloseRequested?.Invoke(this, EventArgs.Empty);
            });

            SelectedSkillModel = Skills.First();

            Task.Run(InitializeTestView);

            ParseDurationFormula(weaveTalent.DurationFormula);
            ParseRangeFormula(weaveTalent.RangeFormula);
            ParseCorrosionFormula(weaveTalent.CorrosionFormula);
            ParseDifficultyFormula(weaveTalent.DifficultyFormula);
            RecalculateCorrosion();
        }

        public SkillModel SelectedSkillModel {
            get => _selectedSkillModel;
            set => SetProperty(ref _selectedSkillModel, value);
        }

        private void InitializeTestView()
        {
            //masteries
            var masteries = new List<MasteryListItemViewModel>();
            var allMasteries = _wikiDataService.GetAllMasteries(SkillGroupModelType.Webkunst);
            foreach (var mastery in allMasteries)
            {
                var avaiable = _characterViewModel.CheckMasteryRequirement(mastery.Requirements);

                var vm = new MasteryListItemViewModel(mastery)
                {
                    Available = avaiable
                };
                vm.TalentValueChanged += (sender, args) => RecalcDifficultyValue();
                masteries.Add(vm);
            }

            Masteries = masteries;
        }

        private void RecalcDifficultyValue()
        {
            var result = SelectedSkillModel.FinalValue.GetRoundedValue();

            ////handicap
            //if (Handicaps != null)
            //{
            //    foreach (var handicap in Handicaps)
            //    {
            //        if (handicap.IsChecked)
            //            result -= handicap.HandiCapValue ?? 0;
            //    }
            //}

            ////masteries
            //if (Masteries != null)
            //{
            //    foreach (var mastery in Masteries)
            //    {
            //        if (!mastery.Available)
            //            continue;

            //        if (mastery.Mastery.ActiveUse == false || mastery.Mastery.ActiveUse && mastery.InUse)
            //            result -= mastery.Mastery.Difficulty ?? mastery.DifficultyOverride ?? 0;
            //    }
            //}

            ////talents
            //if (Talents != null)
            //{
            //    foreach (var talent in Talents)
            //    {
            //        if (!talent.Available)
            //            continue;

            //        if (talent.Talent.ActiveUse == false || talent.Talent.ActiveUse && talent.InUse)
            //            result -= talent.Talent.Difficulty ?? talent.DifficultyOverride ?? 0;
            //    }
            //}

            //FinalTestValue = result;
        }

        public List<MasteryListItemViewModel> Masteries
        {
            get => _masteries;
            set => SetProperty(ref _masteries, value);
        }

        public bool IsDurationConfigurable
        {
            get => _isDurationConfigurable;
            set => SetProperty(ref _isDurationConfigurable, value);
        }

        public bool IsRangeConfigurable
        {
            get => _isRangeConfigurable;
            set => SetProperty(ref _isRangeConfigurable, value);
        }

        public int TimeQuantity
        {
            get => _timeQuantity;
            set
            {
                SetProperty(ref _timeQuantity, value);
                OnPropertyChanged(nameof(TimeValue));
                RecalculateCorrosion();
            }
        }

        public int TimeSteps
        {
            get => _timeSteps;
            set
            {
                SetProperty(ref _timeSteps, value);
                OnPropertyChanged(nameof(TimeValue));
            }
        }

        public int TimeValue => TimeQuantity * TimeSteps;

        public string TimeDescription { get; set; }


        public int RangeQuantity
        {
            get => _rangeQuantity;
            set
            {
                SetProperty(ref _rangeQuantity, value);
                OnPropertyChanged(nameof(RangeValue));
                RecalculateCorrosion();
            }
        }

        public double RangeSteps
        {
            get => _rangeSteps;
            set
            {
                SetProperty(ref _rangeSteps, value);
                OnPropertyChanged(nameof(RangeValue));
            }
        }

        public string RangeDescription { get; set; }

        public double RangeValue => RangeQuantity * RangeSteps;

        public bool IsTalentLevelConfigurable
        {
            get => _isTalentLevelConfigurable;
            set => SetProperty(ref _isTalentLevelConfigurable, value);
        }

        public int TalentLevel
        {
            get => _talentLevel;
            set
            {
                SetProperty(ref _talentLevel, value);
                RecalculateCorrosion();
            }
        }

        public int VolumeQuantity
        {
            get => _volumeQuantity;
            set => SetProperty(ref _volumeQuantity, value);
        }

        public int VolumeSteps
        {
            get => _volumeSteps;
            set => SetProperty(ref _volumeSteps, value);
        }

        public int VolumeDescription
        {
            get => _volumeDescription;
            set => SetProperty(ref _volumeDescription, value);
        }

        public bool IsVolumeConfigurable
        {
            get => _isVolumeConfigurable;
            set => SetProperty(ref _isVolumeConfigurable, value);
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

        private void ParseDurationFormula(string formula)
        {
            if (formula.Equals("Permanent", StringComparison.OrdinalIgnoreCase))
            {
                IsDurationConfigurable = false;
                TimeDescription = "Permanent";
                return;
            }

            if (formula.Equals("Instantan", StringComparison.OrdinalIgnoreCase))
            {
                IsDurationConfigurable = false;
                TimeDescription = "Instantan";
                return;
            }

            if (formula.Equals("speziell", StringComparison.OrdinalIgnoreCase))
            {
                IsDurationConfigurable = false;
                TimeDescription = "Speziell";
                return;
            }

            if (formula.StartsWith("t"))
            {
                IsDurationConfigurable = true;

                var rawTimeValue = Regex.Match(formula, @"([[])([0-9]?)( *)(min|h|d|Phasen|phasen)([\]])").Value;
                var timeValue = rawTimeValue.Trim('[', ']').Replace(" ", string.Empty);

                var timeQuantity = Regex.Match(timeValue, "([0-9])");
                if (timeQuantity.Success)
                {
                    TimeSteps = int.Parse(timeQuantity.Value);
                }
                else
                {
                    TimeSteps = 1;
                }

                TimeQuantity = TimeSteps;

                var timeUnit = Regex.Match(timeValue, "(min|h|d|Phasen|phasen)").Value;
                TimeDescription = timeUnit;
                return;
            }

            throw new InvalidOperationException($"Unknown Durationformula: \"{formula}\"");
        }

        private void ParseRangeFormula(string formula)
        {
            if (formula.Contains("B;"))
            {
                formula = formula.Replace("B;", string.Empty);
            }

            if (formula.Equals("B, 0"))
            {
                IsRangeConfigurable = false;
                RangeDescription = "Berührungsreichweite bzw. 0";
                return;
            }


            if (formula.Equals("0"))
            {
                IsRangeConfigurable = false;
                RangeDescription = string.Empty;
                return;
            }

            if (formula.Equals("B"))
            {
                IsRangeConfigurable = false;
                RangeDescription = "Berührungsreichweite";
                return;
            }


            var radiusFormula = formula.Contains("r");
            var diameterFormula = formula.Contains("d");
            if (radiusFormula || diameterFormula)
            {
                //if (radiusFormula)
                //    RangeType = WeaveTalentRangeType.Radius;
                //if (diameterFormula)
                //    RangeType = WeaveTalentRangeType.Diameter;

                IsRangeConfigurable = true;

                var rawRangeValue = Regex.Match(formula, @"([[])(([0-9]*[,])?[0-9]?)( *)(m|M|Meter|meter)([\]])").Value;
                var rangeValue = rawRangeValue.Trim('[', ']').Replace(" ", string.Empty);

                var rangeQuantity = Regex.Match(rangeValue, "(([0-9]*[,])?[0-9]+)");
                if (rangeQuantity.Success)
                {
                    RangeSteps = double.Parse(rangeQuantity.Value);
                }
                else
                {
                    RangeSteps = 1;
                }

                var rangeUnit = Regex.Match(rangeValue, "(m|M|Meter|meter)").Value;
                RangeDescription = rangeUnit;
                return;
            }

            throw new InvalidOperationException($"Unknown Rangeformula: \"{formula}\"");
        }

        private void ParseDifficultyFormula(string formula)
        {

        }

        private void ParseCorrosionFormula(string formula)
        {
            if (formula.Contains("S"))
            {
                IsTalentLevelConfigurable = true;
                TalentLevel = 1;
            }
        }

        public int CorrosionCost
        {
            get => _corrosionCost;
            set => SetProperty(ref _corrosionCost, value);
        }

        private void RecalculateCorrosion()
        {
            var formula = WeaveTalent.CorrosionFormula;
            var calculation = formula
                .Replace("S", TalentLevel.ToString())
                .Replace("t", TimeQuantity.ToString())
                .Replace("d", RangeQuantity.ToString())
                .Replace("r", RangeQuantity.ToString());

            Debug.WriteLine($"Korrosion, Alt: \"{formula}\", Neu: \"{calculation}\"");

            try
            {
                var result = new DataTable().Compute(calculation, null).ToString();
                CorrosionCost = int.Parse(result);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }
    }
}
