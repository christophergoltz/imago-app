using System;
using System.Collections.Generic;
using System.Text;
using ImagoApp.Shared.Enums;

namespace ImagoApp.Application.Models
{
    public class BodyPartDamageStateModel : BindableBase
    {
        private BodyPartDamageStateType _stateType;

        public BodyPartDamageStateModel(BodyPartType bodyPart)
        {
            BodyPart = bodyPart;
        }

        public BodyPartDamageStateType StateType
        {
            get => _stateType;
            set => SetProperty(ref _stateType , value);
        }

        public BodyPartType BodyPart { get; set; }
    }
}
