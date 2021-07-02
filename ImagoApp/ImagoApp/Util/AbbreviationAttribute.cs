using System;

namespace ImagoApp.Util
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