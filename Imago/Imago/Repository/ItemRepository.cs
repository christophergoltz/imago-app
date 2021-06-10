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
using Imago.Shared;
using Imago.Shared.Models;
using Newtonsoft.Json;

namespace Imago.Repository
{
    public interface IItemRepository
    {
        ArmorSet GetArmorSet(ArmorModelType armorType);
        ArmorModel GetArmorPart(ArmorModelType armorType, BodyPartType bodyPart);
        IEnumerable<ArmorModel> GetAllArmorParts(BodyPartType bodyPart);
        Weapon GetWeapon(WeaponType weaponType);
        List<Weapon> GetAllWeapons();
    }

    public class ItemRepository : IItemRepository
    {
        public ItemRepository()
        {
            _armorLookUp = JsonConvert.DeserializeObject<Dictionary<ArmorModelType, ArmorSet>>(
                "{\"Chitin\":{\"ArmorParts\":{\"Helm\":{\"PhysicalDefense\":1,\"EnergyDefense\":3,\"Type\":3,\"Fight\":false,\"Adventure\":false,\"DurabilityValue\":40,\"LoadValue\":10},\"Torso\":{\"PhysicalDefense\":1,\"EnergyDefense\":4,\"Type\":3,\"Fight\":false,\"Adventure\":false,\"DurabilityValue\":70,\"LoadValue\":40},\"Arm\":{\"PhysicalDefense\":1,\"EnergyDefense\":3,\"Type\":3,\"Fight\":false,\"Adventure\":false,\"DurabilityValue\":40,\"LoadValue\":10},\"Bein\":{\"PhysicalDefense\":1,\"EnergyDefense\":3,\"Type\":3,\"Fight\":false,\"Adventure\":false,\"DurabilityValue\":60,\"LoadValue\":25}}},\"HolzKnochen\":{\"ArmorParts\":{\"Helm\":{\"PhysicalDefense\":1,\"EnergyDefense\":5,\"Type\":7,\"Fight\":false,\"Adventure\":false,\"DurabilityValue\":35,\"LoadValue\":10},\"Torso\":{\"PhysicalDefense\":2,\"EnergyDefense\":6,\"Type\":7,\"Fight\":false,\"Adventure\":false,\"DurabilityValue\":65,\"LoadValue\":50},\"Arm\":{\"PhysicalDefense\":2,\"EnergyDefense\":5,\"Type\":7,\"Fight\":false,\"Adventure\":false,\"DurabilityValue\":35,\"LoadValue\":15},\"Bein\":{\"PhysicalDefense\":2,\"EnergyDefense\":5,\"Type\":7,\"Fight\":false,\"Adventure\":false,\"DurabilityValue\":60,\"LoadValue\":30}}},\"Ketten\":{\"ArmorParts\":{\"Helm\":{\"PhysicalDefense\":3,\"EnergyDefense\":1,\"Type\":5,\"Fight\":false,\"Adventure\":false,\"DurabilityValue\":70,\"LoadValue\":15},\"Torso\":{\"PhysicalDefense\":4,\"EnergyDefense\":1,\"Type\":5,\"Fight\":false,\"Adventure\":false,\"DurabilityValue\":120,\"LoadValue\":55},\"Arm\":{\"PhysicalDefense\":3,\"EnergyDefense\":1,\"Type\":5,\"Fight\":false,\"Adventure\":false,\"DurabilityValue\":80,\"LoadValue\":25},\"Bein\":{\"PhysicalDefense\":4,\"EnergyDefense\":1,\"Type\":5,\"Fight\":false,\"Adventure\":false,\"DurabilityValue\":100,\"LoadValue\":30}}},\"Komposit\":{\"ArmorParts\":{\"Helm\":{\"PhysicalDefense\":2,\"EnergyDefense\":2,\"Type\":2,\"Fight\":false,\"Adventure\":false,\"DurabilityValue\":30,\"LoadValue\":10},\"Torso\":{\"PhysicalDefense\":2,\"EnergyDefense\":2,\"Type\":2,\"Fight\":false,\"Adventure\":false,\"DurabilityValue\":50,\"LoadValue\":25},\"Arm\":{\"PhysicalDefense\":2,\"EnergyDefense\":2,\"Type\":2,\"Fight\":false,\"Adventure\":false,\"DurabilityValue\":30,\"LoadValue\":12},\"Bein\":{\"PhysicalDefense\":2,\"EnergyDefense\":2,\"Type\":2,\"Fight\":false,\"Adventure\":false,\"DurabilityValue\":40,\"LoadValue\":18}}},\"Platten\":{\"ArmorParts\":{\"Helm\":{\"PhysicalDefense\":5,\"EnergyDefense\":1,\"Type\":4,\"Fight\":false,\"Adventure\":false,\"DurabilityValue\":70,\"LoadValue\":25},\"Torso\":{\"PhysicalDefense\":7,\"EnergyDefense\":1,\"Type\":4,\"Fight\":false,\"Adventure\":false,\"DurabilityValue\":120,\"LoadValue\":70},\"Arm\":{\"PhysicalDefense\":5,\"EnergyDefense\":1,\"Type\":4,\"Fight\":false,\"Adventure\":false,\"DurabilityValue\":70,\"LoadValue\":30},\"Bein\":{\"PhysicalDefense\":6,\"EnergyDefense\":1,\"Type\":4,\"Fight\":false,\"Adventure\":false,\"DurabilityValue\":80,\"LoadValue\":40}}},\"Stepp\":{\"ArmorParts\":{\"Helm\":{\"PhysicalDefense\":1,\"EnergyDefense\":1,\"Type\":6,\"Fight\":false,\"Adventure\":false,\"DurabilityValue\":20,\"LoadValue\":5},\"Torso\":{\"PhysicalDefense\":1,\"EnergyDefense\":1,\"Type\":6,\"Fight\":false,\"Adventure\":false,\"DurabilityValue\":40,\"LoadValue\":20},\"Arm\":{\"PhysicalDefense\":1,\"EnergyDefense\":1,\"Type\":6,\"Fight\":false,\"Adventure\":false,\"DurabilityValue\":30,\"LoadValue\":2},\"Bein\":{\"PhysicalDefense\":1,\"EnergyDefense\":1,\"Type\":6,\"Fight\":false,\"Adventure\":false,\"DurabilityValue\":20,\"LoadValue\":10}}}}");
        }

        private static Dictionary<ArmorModelType, ArmorSet> _armorLookUp;

        private static readonly Dictionary<WeaponType, Weapon> WeaponLookUp = new Dictionary<WeaponType, Weapon>
        {
            {
                WeaponType.HolzfaellerAxt, new Weapon(WeaponType.HolzfaellerAxt,
                    new Dictionary<WeaponStanceType, WeaponStance>
                    {
                        {
                            WeaponStanceType.Light,
                            new WeaponStance(WeaponStanceType.Light, "5", "2W6P", -60, "nah", 55)
                        },
                        {
                            WeaponStanceType.Heavy,
                            new WeaponStance(WeaponStanceType.Heavy, "6", "3W6+2P", -40, "nah", 55)
                        },
                    })
            },
            {
                WeaponType.Dolch, new Weapon(WeaponType.Dolch,
                    new Dictionary<WeaponStanceType, WeaponStance>
                    {
                        {
                            WeaponStanceType.Light,
                            new WeaponStance(WeaponStanceType.Light, "3", "1W6+3P", -45, "nah", 0)
                        },
                        {
                            WeaponStanceType.Heavy,
                            new WeaponStance(WeaponStanceType.Heavy, "4", "1W6+3P", -30, "nah", 0)
                        },
                    })
            },
            {
                WeaponType.Blankbogen, new Weapon(WeaponType.Blankbogen,
                    new Dictionary<WeaponStanceType, WeaponStance>
                    {
                        {
                            WeaponStanceType.Light,
                            new WeaponStance(WeaponStanceType.Light, "2-1", "1W3+1P", null, "5/20/40/70", 12)
                        },
                        {
                            WeaponStanceType.Heavy,
                            new WeaponStance(WeaponStanceType.Heavy, "3-1", "1W3+3P", null, "7/25/50/90", 12)
                        },
                    })
            },
            {
                WeaponType.Schwert, new Weapon(WeaponType.Schwert,
                    new Dictionary<WeaponStanceType, WeaponStance>
                    {
                        {
                            WeaponStanceType.Light,
                            new WeaponStance(WeaponStanceType.Light, "4", "2W6P", -35, "nah", 0)
                        },
                        {
                            WeaponStanceType.Heavy,
                            new WeaponStance(WeaponStanceType.Heavy, "4", "1W6+1P", -10, "nah", 0)
                        },
                    })
            }
        };
        
        public ArmorSet GetArmorSet(ArmorModelType armorType)
        {
            if (_armorLookUp.ContainsKey(armorType))
                return _armorLookUp[armorType];

            return null;
        }

        public ArmorModel GetArmorPart(ArmorModelType armorType, BodyPartType bodyPart)
        {
            var armorPartType = MapBodyPartTypeToArmorPartType(bodyPart);
            if (_armorLookUp.ContainsKey(armorType))
            {
                var armorSet = _armorLookUp[armorType];
                if (armorSet.ArmorParts.ContainsKey(armorPartType))
                {
                    return DeepCopy(armorSet.ArmorParts[armorPartType]);
                }
            }

            return null;
        }

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
            foreach (var armorSet in _armorLookUp.Values.Where(armorSet => armorSet.ArmorParts.ContainsKey(armorPartType)))
            {
                yield return DeepCopy(armorSet.ArmorParts[armorPartType]);
            }
        }

        public List<Weapon> GetAllWeapons()
        {
            return WeaponLookUp.Values.Select(DeepCopy).ToList();
        }

        public Weapon GetWeapon(WeaponType weaponType)
        {
            if (WeaponLookUp.ContainsKey(weaponType))
                return DeepCopy(WeaponLookUp[weaponType]);

            return null;
        }
    }
}
