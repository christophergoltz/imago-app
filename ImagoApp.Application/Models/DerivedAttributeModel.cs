using System;
using ImagoApp.Application.Models.Base;
using ImagoApp.Shared.Enums;

namespace ImagoApp.Application.Models
{
    //eg. schadensmod, traglast
    public class DerivedAttributeModel : CalculableBaseModel
    {
        private DerivedAttributeType _type;

        public DerivedAttributeModel(DerivedAttributeType type)
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