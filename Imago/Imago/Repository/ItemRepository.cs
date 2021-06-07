using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imago.Models;
using Imago.Models.Enum;

namespace Imago.Repository
{
    public interface IItemRepository
    {
        ArmorSet GetArmorSet(ArmorType armorType);
        Armor GetArmorPart(ArmorType armorType, BodyPartType bodyPart);
        IEnumerable<Armor> GetAllArmorParts(BodyPartType bodyPart);
        Weapon GetWeapon(WeaponType weaponType);
        List<Weapon> GetAllWeapons();
    }

    public class ItemRepository : IItemRepository
    {
        private static readonly Dictionary<ArmorType, ArmorSet> ArmorLookUp = new Dictionary<ArmorType, ArmorSet>
        {
            {ArmorType.Komposit, new ArmorSet(new Dictionary<BodyPartType, Armor>
                {
                    {BodyPartType.Kopf, new Armor(ArmorType.Komposit, 2, 2, 10)},
                    {BodyPartType.Torso, new Armor(ArmorType.Komposit, 2, 2, 25)},
                    {BodyPartType.ArmLinks, new Armor(ArmorType.Komposit, 2, 2, 12)},
                    {BodyPartType.ArmRechts, new Armor(ArmorType.Komposit, 2, 2, 12)},
                    {BodyPartType.BeinLinks, new Armor(ArmorType.Komposit, 2, 2, 18)},
                    {BodyPartType.BeinRechts, new Armor(ArmorType.Komposit, 2, 2, 18)},
                })},
            {ArmorType.Chitin, new ArmorSet(new Dictionary<BodyPartType, Armor>
            {
                {BodyPartType.Kopf, new Armor(ArmorType.Chitin, 1, 3, 10)},
                {BodyPartType.Torso, new Armor(ArmorType.Chitin, 1, 4, 40)},
                {BodyPartType.ArmLinks, new Armor(ArmorType.Chitin, 1, 3, 10)},
                {BodyPartType.ArmRechts, new Armor(ArmorType.Chitin, 1, 3, 10)},
                {BodyPartType.BeinLinks, new Armor(ArmorType.Chitin, 1, 3, 25)},
                {BodyPartType.BeinRechts, new Armor(ArmorType.Chitin, 1, 3, 25)},
            })},
            {ArmorType.Platten, new ArmorSet(new Dictionary<BodyPartType, Armor>
            {
                {BodyPartType.Kopf, new Armor(ArmorType.Platten, 5, 1, 25)},
                {BodyPartType.Torso, new Armor(ArmorType.Platten, 7, 1, 75)},
                {BodyPartType.ArmLinks, new Armor(ArmorType.Platten, 5, 1, 30)},
                {BodyPartType.ArmRechts, new Armor(ArmorType.Platten, 5, 1, 30)},
                {BodyPartType.BeinLinks, new Armor(ArmorType.Platten, 6, 1, 40)},
                {BodyPartType.BeinRechts, new Armor(ArmorType.Platten, 6, 1, 40)},
            })},
            {ArmorType.Natuerlich, new ArmorSet(new Dictionary<BodyPartType, Armor>
            {
                {BodyPartType.Kopf, new Armor(ArmorType.Natuerlich,0,0)},
                {BodyPartType.Torso, new Armor(ArmorType.Natuerlich, 0,0)},
                {BodyPartType.ArmLinks, new Armor(ArmorType.Natuerlich, 0,0)},
                {BodyPartType.ArmRechts, new Armor(ArmorType.Natuerlich,0,0)},
                {BodyPartType.BeinLinks, new Armor(ArmorType.Natuerlich,0,0)},
                {BodyPartType.BeinRechts, new Armor(ArmorType.Natuerlich,0,0)},
            })}
        };

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
        
        public ArmorSet GetArmorSet(ArmorType armorType)
        {
            if (ArmorLookUp.ContainsKey(armorType))
                return ArmorLookUp[armorType];

            return null;
        }

        public Armor GetArmorPart(ArmorType armorType, BodyPartType bodyPart)
        {
            if (ArmorLookUp.ContainsKey(armorType))
            {
                var armorSet = ArmorLookUp[armorType];
                if (armorSet.ArmorParts.ContainsKey(bodyPart))
                    return armorSet.ArmorParts[bodyPart];
            }

            return null;
        }

        public IEnumerable<Armor> GetAllArmorParts(BodyPartType bodyPart)
        {
            foreach (var armorSet in ArmorLookUp.Values)
            {
                if (armorSet.ArmorParts.ContainsKey(bodyPart))
                {
                    yield return armorSet.ArmorParts[bodyPart];
                }
            }
        }

        public List<Weapon> GetAllWeapons()
        {
            return WeaponLookUp.Values.ToList();
        }

        public Weapon GetWeapon(WeaponType weaponType)
        {
            if (WeaponLookUp.ContainsKey(weaponType))
                return WeaponLookUp[weaponType];

            return null;
        }
    }
}
