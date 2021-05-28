using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imago.Models;
using Imago.Models.Enum;

namespace Imago.Services
{
    public interface IAttributeService
    {
        void AddCorrosion(AttributeType type, int corrosion);
    }

    public class AttributeService : IAttributeService
    {
        public AttributeService()
        {
            
        }
        public Character Character { get; set; }

        public void AddCorrosion(AttributeType type, int corrosion)
        {
            var attr = Character.Attributes.First(_ => _.Type == type);
            attr.Corrosion += corrosion;
            attr.FinalValue = attr.NaturalValue + attr.IncreaseValue + attr.ModificationValue - attr.Corrosion;
            UpdateDependentSkills(type, attr.FinalValue);
        }

        private void UpdateDependentSkills(AttributeType type, int newFinalValue)
        {
            //todo
        }
    }
}
