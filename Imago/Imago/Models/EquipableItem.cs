using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using Imago.Models.Base;

namespace Imago.Models
{
    public class EquipableItem : ItemBase
    {
        private bool _fight;
        private bool _adventure;

        public EquipableItem()
        {
            }

        public EquipableItem(string name, int loadValue, bool adventure, bool fight) : base(name, loadValue)
        {
            Adventure = adventure;
            Fight = fight;
        }

        public bool Fight
        {
            get => _fight;
            set => SetProperty(ref _fight, value);
        }

        public bool Adventure
        {
            get => _adventure;
            set => SetProperty(ref _adventure, value);
        }
    }
}