using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Transactions;
using Imago.Models;
using Imago.Models.Enum;
using Newtonsoft.Json;

namespace Imago.Repository
{
    public interface IItemRepository
    {
     //   ArmorSet GetArmorSet(ArmorModelType armorType);
      //  ArmorModel GetArmorPart(ArmorModelType armorType, BodyPartType bodyPart);
        IEnumerable<ArmorModel> GetAllArmorParts(BodyPartType bodyPart);
        List<Weapon> GetAllWeapons();
    }

    public class ItemRepository : IItemRepository
    {
        public ItemRepository()
        {
            _armorLookUp = new List<ArmorSet>();
        }

        private static List<ArmorSet> _armorLookUp;

        private static readonly List<Weapon> WeaponLookUp = new List<Weapon>
        {
            
        };
        
        //public ArmorSet GetArmorSet(ArmorModelType armorType)
        //{
        //    if (_armorLookUp.ContainsKey(armorType))
        //        return _armorLookUp[armorType];

        //    return null;
        //}

        //public ArmorModel GetArmorPart(ArmorModelType armorType, BodyPartType bodyPart)
        //{
        //    var armorPartType = MapBodyPartTypeToArmorPartType(bodyPart);
        //    if (_armorLookUp.ContainsKey(armorType))
        //    {
        //        var armorSet = _armorLookUp[armorType];
        //        if (armorSet.ArmorParts.ContainsKey(armorPartType))
        //        {
        //            return DeepCopy(armorSet.ArmorParts[armorPartType]);
        //        }
        //    }

        //    return null;
        //}

        private static T DeepCopy<T>(T other)
        {
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(other));
        }

        private ArmorPartType MapBodyPartTypeToArmorPartType(BodyPartType bodyPart)
        {
            var armorPartType = ArmorPartType.Unknown;

            if (bodyPart == BodyPartType.Kopf)
                armorPartType = ArmorPartType.Helm;
            if (bodyPart == BodyPartType.Torso)
                armorPartType = ArmorPartType.Torso;
            if (bodyPart == BodyPartType.ArmLinks || bodyPart == BodyPartType.ArmRechts)
                armorPartType = ArmorPartType.Arm;
            if (bodyPart == BodyPartType.BeinLinks || bodyPart == BodyPartType.BeinRechts)
                armorPartType = ArmorPartType.Bein;

            return armorPartType;
        }

        public IEnumerable<ArmorModel> GetAllArmorParts(BodyPartType bodyPart)
        {
            var armorPartType = MapBodyPartTypeToArmorPartType(bodyPart);
            foreach (var armorSet in _armorLookUp.Where(armorSet => armorSet.ArmorParts.ContainsKey(armorPartType)))
            {
                yield return DeepCopy(armorSet.ArmorParts[armorPartType]);
            }
        }

        public List<Weapon> GetAllWeapons()
        {
            return WeaponLookUp.Select(DeepCopy).ToList();
        }
    }
}
