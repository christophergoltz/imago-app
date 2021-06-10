using System;

namespace Imago.Shared.Util
{
    public class DisplayTextAttribute : Attribute
    {
        public string Text { get; }

        public DisplayTextAttribute(string text)
        {
            Text = text;
        }
    }
}
