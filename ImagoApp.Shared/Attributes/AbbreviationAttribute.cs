using System;

namespace ImagoApp.Shared.Attributes
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