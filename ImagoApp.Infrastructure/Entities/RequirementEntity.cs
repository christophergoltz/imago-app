using System;
using System.Collections.Generic;
using System.Text;

namespace ImagoApp.Infrastructure.Entities
{
    public class RequirementEntity<TType> where TType : Enum
    {
        public TType Type { get; set; }
        public int Value { get; set; }
    }
}
