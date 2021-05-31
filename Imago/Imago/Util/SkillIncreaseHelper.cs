using System;
using System.Collections.Generic;
using System.Text;
using Imago.Models;
using Imago.Models.Base;
using Attribute = System.Attribute;

namespace Imago.Util
{
    public static class SkillIncreaseHelper
    {
        //todo als formel abloesen, start bei 2, dann i=1; zahl plus i, dann i++
        public static readonly int[] ImagoFolge = { 2, 3, 5, 8, 12, 17, 23, 30, 38, 47 };

        public static bool CanSkillBeIncreased(UpgradeableSkillBase skill)
        {
            //reached local maximum
            if (skill.IncreaseValue == 100)
                return false;

            var requiredExperienceForNextLevel = GetExperienceForNextLevel(skill);
            return skill.Experience >= requiredExperienceForNextLevel;
        }

        public static int GetExperienceForNextLevel(UpgradeableSkillBase skill)
        {
            int? requiredExperience = null;

            if (skill is Attribute)
                requiredExperience = GetExperienceForNextAttributeLevel(skill.IncreaseValue);

            if (skill is SkillGroup)
                requiredExperience = GetExperienceForNextSkillGroupLevel(skill.IncreaseValue);

            if (skill is Skill)
                requiredExperience = GetExperienceForNextSkillLevel(skill.IncreaseValue);

            if (requiredExperience == null)
                throw new InvalidOperationException($"Unable to get experiencecost for next upgrade by current {skill.IncreaseValue} value of type: " + skill.GetType());

            return requiredExperience.Value;
        }


        /// <summary>
        /// Gibt die Kosten der nächsten Steigerung für ein Attribut zurück.
        /// </summary>
        /// <param name="currentIncreaseValue">Der aktuelle Steigerungswert</param>
        /// <returns>Die Anzahl der Erfahrungspunkte, die für den nächsten Aufstieg bezahlt werden müssen.</returns>
        private static int? GetExperienceForNextAttributeLevel(int currentIncreaseValue)
        {
            if (currentIncreaseValue == 100)
                return null;

            double steigerungsWertFaktisch = (double)currentIncreaseValue / 10;
            var temp = (int)Math.Floor(steigerungsWertFaktisch);

            var resultIndex = temp - 3;
            if (resultIndex <= 0)
            {
                resultIndex = 0;
            }

            if (resultIndex > 9)
            {
                resultIndex = 9;
            }

            int benoetigteErfahrungspunkte = ImagoFolge[resultIndex];
            return benoetigteErfahrungspunkte;
        }

        /// <summary>
        /// Gibt die Kosten der nächsten Steigerung für eine Fertigkeitskategorie zurück.
        /// </summary>
        /// <param name="currentIncreaseValue">Der aktuelle Steigerungswert</param>
        /// <returns>Die Anzahl der Erfahrungspunkte, die für den nächsten Aufstieg bezahlt werden müssen.</returns>
        private static int? GetExperienceForNextSkillGroupLevel(int currentIncreaseValue)
        {
            if (currentIncreaseValue == 100)
                return null;

            double steigerungsWertFaktisch = (double)currentIncreaseValue / 5;
            var resultIndex = ((int)Math.Floor(steigerungsWertFaktisch)) + 2;
            if (resultIndex < 0)
            {
                resultIndex = 0;
            }

            if (resultIndex > 9)
            {
                resultIndex = 9;
            }

            int benoetigteErfahrungspunkte = ImagoFolge[resultIndex];
            return benoetigteErfahrungspunkte;
        }

        /// <summary>
        /// Gibt die Kosten der nächsten Steigerung für eine Fertigkeit zurück.
        /// </summary>
        /// <param name="currentIncreaseValue">Der aktuelle Steigerungswert</param>
        /// <returns>Die Anzahl der Erfahrungspunkte, die für den nächsten Aufstieg bezahlt werden müssen.</returns>
        private static int? GetExperienceForNextSkillLevel(int currentIncreaseValue)
        {
            if (currentIncreaseValue == 100)
                return null;

            double steigerungsWertFaktisch = (double)currentIncreaseValue / 15;
            var resultIndex = ((int)Math.Floor(steigerungsWertFaktisch));
            if (resultIndex < 0)
            {
                resultIndex = 0;
            }

            if (resultIndex > 9)
            {
                resultIndex = 9;
            }

            int benoetigteErfahrungspunkte = ImagoFolge[resultIndex];
            return benoetigteErfahrungspunkte;
        }
    }
}
