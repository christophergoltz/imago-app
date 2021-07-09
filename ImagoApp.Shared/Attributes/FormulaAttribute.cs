using System;

namespace ImagoApp.Shared.Attributes
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
