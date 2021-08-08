using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows.Input;
using ImagoApp.Application;
using ImagoApp.Application.Models;
using Xamarin.Forms;

namespace ImagoApp.ViewModels
{
    public class WeaveTalentDetailViewModel : BindableBase
    {
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

        public ICommand CloseCommand { get; set; }

        public event EventHandler CloseRequested;

        public WeaveTalentDetailViewModel(WeaveTalentModel weaveTalent, List<SkillModel> skills)
        {
            WeaveTalent = weaveTalent;
            Skills = skills;

            CloseCommand = new Command(() =>
            {
                CloseRequested?.Invoke(this, EventArgs.Empty);
            });

            ParseDurationFormula(weaveTalent.DurationFormula);
            ParseRangeFormula(weaveTalent.RangeFormula);
            ParseCorrosionFormula(weaveTalent.CorrosionFormula);
            ParseDifficultyFormula(weaveTalent.DifficultyFormula);
            RecalculateCorrosion();
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
