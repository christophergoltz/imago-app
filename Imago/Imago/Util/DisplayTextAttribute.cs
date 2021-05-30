using System;
using System.Collections.Generic;
using System.Text;

namespace Imago.Attributes
{
    public class DisplayTextAttribute : Attribute
    {
        public string Text { get; }
        public string Abbreviation { get; }

        internal DisplayTextAttribute(string text, string abbreviation)
        {
            Text = text;
            Abbreviation = abbreviation;
        }

        internal DisplayTextAttribute(string abbreviation)
        {
            Abbreviation = abbreviation;
        }
    }
}
