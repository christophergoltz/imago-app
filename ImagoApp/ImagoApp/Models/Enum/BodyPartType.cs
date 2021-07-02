namespace ImagoApp.Models.Enum
{
    public enum BodyPartType
    {
        Unknown = 0,
        [Util.Formula("KO/15+3")]
        Kopf = 1,
        [Util.Formula("KO/6+2")]
        Torso = 2,

        [Util.Formula("KO/10+1")]
        [Util.DisplayText("Arm links")] 
        ArmLinks = 3,

        [Util.Formula("KO/10+1")]
        [Util.DisplayText("Arm rechts")] 
        ArmRechts = 4,

        [Util.DisplayText("Bein links")]
        [Util.Formula("KO/7+2")]
        BeinLinks = 5,

        [Util.DisplayText("Bein rechts")]
        [Util.Formula("KO/7+2")]
        BeinRechts = 6
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