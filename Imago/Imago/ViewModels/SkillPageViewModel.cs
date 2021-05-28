using System;
using System.Collections.Generic;
using System.Text;
using Imago.Models;
using Imago.Util;

namespace Imago.ViewModels
{
    public class SkillPageViewModel : BindableBase
    {
        public SkillPageViewModel(Character character)
        {
            Character = character;
        }
        public Character Character { get; private set; }
    }
}
