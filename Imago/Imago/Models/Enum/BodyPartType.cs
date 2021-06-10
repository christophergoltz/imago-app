using System;
using System.Collections.Generic;
using System.Text;
using Imago.Util;

namespace Imago.Models.Enum
{
    public enum BodyPartType
    {
        Unknown = 0,
        Kopf = 1,
        Torso = 2,
        [DisplayText("Arm links")] ArmLinks = 3,
        [DisplayText("Arm rechts")] ArmRechts = 4,
        [DisplayText("Bein links")] BeinLinks = 5,
        [DisplayText("Bein rechts")] BeinRechts = 6
    }

    public static class BodyPartTypeExtension
    {
        public static ArmorPartType MapBodyPartTypeToArmorPartType(this BodyPartType bodyPart)
        {
            var armorPartType = ArmorPartType.Unknown;

            if (bodyPart == BodyPartType.Kopf)
                armorPartType = ArmorPartType.Helm;
            if (bodyPart == BodyPartType.Torso)
                armorPartType = ArmorPartType.Torso;
            if (bodyPart == BodyPartType.ArmLinks || bodyPart == BodyPartType.ArmRechts)
                armorPartType = ArmorPartType.Arm;
            if (bodyPart == BodyPartType.BeinLinks || bodyPart == BodyPartType.BeinRechts)
                armorPartType = ArmorPartType.Bein;

            return armorPartType;
        }
    }
}