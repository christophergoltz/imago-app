using System;
using System.Collections.Generic;
using System.Text;

namespace ImagoApp.Application.Models
{
    public class RequirementModel<TType> where TType : Enum
    {
        public RequirementModel(TType type, int value)
        {
            Type = type;
            Value = value;
        }

        public RequirementModel()
        {
            
        }

        public TType Type { get; set; }
        public int Value { get; set; }
    }
}