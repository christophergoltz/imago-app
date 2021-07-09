using System;

namespace ImagoApp.Shared.Attributes
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
