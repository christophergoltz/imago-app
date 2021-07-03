using System;
using ImagoApp.Application.Models.Base;
using ImagoApp.Shared.Enums;

namespace ImagoApp.Application.Models
{
    //eg. schadensmod, traglast
    public class DerivedAttribute : CalculableBase
    {
        private DerivedAttributeType _type;

        public DerivedAttribute(DerivedAttributeType type)
        {
            Type = type;
        }

        public DerivedAttributeType Type
        {
            get => _type;
            set => SetProperty(ref _type, value);
        }
    }
}