using System;

namespace ImagoApp.Util
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
