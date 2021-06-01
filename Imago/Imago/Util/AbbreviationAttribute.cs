using System;
using System.Collections.Generic;
using System.Text;

namespace Imago.Util
{
    public class AbbreviationAttribute : Attribute
    {
        public string Abbreviation { get; }

        internal AbbreviationAttribute(string abbreviation)
        {
            Abbreviation = abbreviation;
        }
    }
}