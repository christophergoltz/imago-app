using System;

namespace Imago.Util
{
    public class DisplayTextAttribute : Attribute
    {
        public string Text { get; }

        internal DisplayTextAttribute(string text)
        {
            Text = text;
        }
    }
}
