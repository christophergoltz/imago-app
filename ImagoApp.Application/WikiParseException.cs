using System;
using System.Collections.Generic;
using System.Text;

namespace ImagoApp.Application
{
    public class WikiParseException : Exception
    {
        public WikiParseException()
        {
        }

        public WikiParseException(string message)
            : base(message)
        {
        }

        public WikiParseException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
