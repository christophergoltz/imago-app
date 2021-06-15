using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Imago.Models;
using Imago.Models.Base;
using Attribute = Imago.Models.Attribute;

namespace Imago.Util
{
    public static class SkillIncreaseHelper
    {
        //todo als formel abloesen, start bei 2, dann i=1; zahl plus i, dann i++
        public static readonly int[] ImagoFolge = {2, 3, 5, 8, 12, 17, 23, 30, 38, 47};

        public static bool CanSkillBeIncreased(Attribute attribute)
        {
            //reached local maximum
            if (attribute.IncreaseValue == 100)
                return false;

            var requiredExperienceForNextLevel = GetExperienceForNextAttributeLevel(attribute.IncreaseValue);
            return attribute.Experience >= requiredExperienceForNextLevel;
        }

        public static bool CanSkillBeIncreased(SkillGroup skillGroup)
        {
            //reached local maximum
            if (skillGroup.IncreaseValue == 100)
                return false;

            var requiredExperienceForNextLevel = GetExperienceForNextSkillBaseLevel(skillGroup);
            return skillGroup.ExperienceValue >= requiredExperienceForNextLevel;
        }

        public static bool CanSkillBeIncreased(Skill skill)
        {
            //reached local maximum
            if (skill.IncreaseValue == 100)
                return false;

            var requiredExperienceForNextLevel = GetExperienceForNextSkillBaseLevel(skill);
            return skill.ExperienceValue >= requiredExperienceForNextLevel;
        }

        /// <summary>
        /// Gibt die Kosten der nächsten Steigerung für ein Attribut zurück.
        /// </summary>
        /// <param name="currentIncreaseValue">Der aktuelle Steigerungswert</param>
        /// <returns>Die Anzahl der Erfahrungspunkte, die für den nächsten Aufstieg bezahlt werden müssen.</returns>
        public static int GetExperienceForNextAttributeLevel(int currentIncreaseValue)
        {
            if (currentIncreaseValue == 100)
                throw new DeletedRowInaccessibleException("Attribute cant be increased above 100");

            double steigerungsWertFaktisch = (double) currentIncreaseValue / 10;
            var temp = (int) Math.Floor(steigerungsWertFaktisch);

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
        public static int GetExperienceForNextSkillBaseLevel(SkillBase skillBase)
        {
            if (skillBase.IncreaseValue == 100)
                throw new DeletedRowInaccessibleException("Skillgroup cant be increased above 100");

            if (skillBase is Skill skill)
            {
                double steigerungsWertFaktisch = (double)skill.IncreaseValue / 15;
                var resultIndex = (int) Math.Floor(steigerungsWertFaktisch);
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

            if (skillBase is SkillGroup skillGroup)
            {
                double steigerungsWertFaktisch = (double)skillGroup.IncreaseValue / 5;
                var resultIndex = (int) Math.Floor(steigerungsWertFaktisch) + 2;
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

            throw new InvalidOperationException();
        }
    }
}