using System;

namespace ImagoApp.Util
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
