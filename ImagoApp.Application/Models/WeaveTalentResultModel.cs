using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using ImagoApp.Shared.Attributes;

namespace ImagoApp.Application.Models
{
    public enum WeaveTalentResultType
    {
        [DisplayText("Korrosion")]
        Corrosion,
        [DisplayText("Dauer")]
        Duration,
        [DisplayText("Schwierigkeit")]
        Difficulty,
        [DisplayText("Reichweite")]
        Range
    }

    public class WeaveTalentResultModel : BindableBase
    {
        private WeaveTalentResultType _type;
        private string _formula;
        private string _finalValue;

        public WeaveTalentResultModel(WeaveTalentResultType type, string formula)
        {
            Type = type;
            Formula = formula;
        }

        public WeaveTalentResultType Type
        {
            get => _type;
            set => SetProperty(ref _type, value);
        }

        public string Formula
        {
            get => _formula;
            set => SetProperty(ref _formula, value);
        }

        public string FinalValue
        {
            get => _finalValue;
            private set => SetProperty(ref _finalValue, value);
        }

        public void RecalculateFinalValue(Dictionary<string, string> settingValues)
        {
            settingValues.Add("B", "Berührungsreichweite");

            if (int.TryParse(Formula, out _))
            {
                FinalValue = Formula;
                return;
            }

            //replace all abbreviations with final values
            var calculationFormula = settingValues.Aggregate(Formula, (current, setting) => current.Replace(setting.Key, setting.Value));

            if (Regex.Matches(calculationFormula, @"[a-zA-Z]").Count > 0)
            {
                //calculation cant be done with letters , display remaining
                FinalValue = calculationFormula;
                return;
            }

            try
            {
                FinalValue = new DataTable().Compute(calculationFormula, null).ToString();
            }
            catch (Exception)
            {
                FinalValue = "[Fehler]";
            }
        }
    }
}