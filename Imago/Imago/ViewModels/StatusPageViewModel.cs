using System;
using System.Collections.Generic;
using System.Text;
using Imago.Models;
using Imago.Models.Enum;
using Imago.Util;
using Imago.Views.CustomControls;

namespace Imago.ViewModels
{
    public class StatusPageViewModel : BindableBase
    {
        public Character Character { get; }

        public StatusPageViewModel(Character character)
        {
            Character = character;
        }
    }
}
