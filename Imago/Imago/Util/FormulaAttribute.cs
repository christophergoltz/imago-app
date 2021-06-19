using System;
using System.Collections.Generic;
using System.Text;

namespace Imago.Util
{
    public class FormulaAttribute : Attribute
    {
        public string Formula { get; }

        public FormulaAttribute(string formula)
        {
            Formula = formula;
        }
    }
}
