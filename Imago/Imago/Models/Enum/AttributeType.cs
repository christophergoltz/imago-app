using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Imago.Util;

namespace Imago.Models.Enum
{
    public enum AttributeType
    {
        Unknown = 0,

        [DisplayText("Stärke"), Abbreviation("ST")] 
        Staerke = 1,

        [Abbreviation("GE")] 
        Geschicklichkeit = 2,

        [Abbreviation("KO")]
        Konstitution = 3,

        [Abbreviation("IN")]
        Intelligenz = 4,

        [Abbreviation("Wi")] 
        Willenskraft = 5,

        [Abbreviation("CH")] 
        Charisma = 6,

        [Abbreviation("WA")]
        Wahrnehmung = 7
    }
}