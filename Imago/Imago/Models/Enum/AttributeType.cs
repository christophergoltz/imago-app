using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Imago.Attributes;

namespace Imago.Models.Enum
{
    public enum AttributeType
    {
        Unknown = 0,

        [DisplayText("Stärke", "ST")] Staerke = 1,

        [DisplayText("GE")] Geschicklichkeit = 2,

        [DisplayText("KO")] Konstitution = 3,

        [DisplayText("IN")] Intelligenz = 4,

        [DisplayText("Wi")] Willenskraft = 5,

        [DisplayText("CH")] Charisma = 6,

        [DisplayText("WA")] Wahrnehmung = 7
    }
}