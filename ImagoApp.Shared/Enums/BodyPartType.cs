using ImagoApp.Shared.Attributes;

namespace ImagoApp.Shared.Enums
{
    public enum BodyPartType
    {
        Unknown = 0,
        [Formula("KO/15+3")]
        Kopf = 1,
        [Formula("KO/6+2")]
        Torso = 2,

        [Formula("KO/10+1")]
        [DisplayText("Arm links")] 
        ArmLinks = 3,

        [Formula("KO/10+1")]
        [DisplayText("Arm rechts")] 
        ArmRechts = 4,

        [DisplayText("Bein links")]
        [Formula("KO/7+2")]
        BeinLinks = 5,

        [DisplayText("Bein rechts")]
        [Formula("KO/7+2")]
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