using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Text;
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
            set => SetProperty(ref _formula ,value);
        }

        public string FinalValue
        {
            get => _finalValue;
            private set => SetProperty(ref _finalValue, value);
        }

        public void RecalculateFinalValue(Dictionary<string, string> settingValues)
        {
            var isNumeric = int.TryParse(Formula, out _);
            if (isNumeric)
            {
                FinalValue = Formula;
                return;
            }

            if (Formula.Equals("B"))
            {
                FinalValue = "Berührungsreichweite";
            }

            var calculationFormula = Formula;

            foreach (var setting in settingValues)
            {
                calculationFormula = calculationFormula.Replace(setting.Key, setting.Value);
            }

            Debug.WriteLine($"Berechnung.. Alt: \"{Formula}\", Neu: \"{calculationFormula}\"");

            try
            {
                var calculation = new DataTable().Compute(calculationFormula, null).ToString();
                FinalValue = calculation;
            }
            catch (Exception)
            {
                //calculation failed, display remaining
                FinalValue = calculationFormula;
            }
        }
    }
}