using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Imago.Database;
using Imago.Models;
using Imago.Models.Entity;
using Imago.Models.Enum;
using Imago.Repository;

namespace Imago.Services
{
    public interface IWikiParseService
    {
        Task RefreshWikiData(TableInfoType type);
    }

    public class WikiParseService : IWikiParseService
    {
        private static readonly string ArmorUrl = "http://imago-rp.de/index.php/R%C3%BCstungen";
        private static readonly string MeleeWeaponUrl = "http://imago-rp.de/index.php/Nahkampfwaffen";
        private static readonly string RangedWeaponUrl = "http://imago-rp.de/index.php/Fernkampfwaffen";

        private readonly IWrappingRepository<Weapon, WeaponEntity> _meleeWeaponRepository;
        private readonly IWrappingRepository<Weapon, WeaponEntity> _rangedWeaponRepository;
        private readonly IWrappingRepository<ArmorSet, ArmorSetEntity> _armorWeaponRepository;

        public WikiParseService(
            IWrappingRepository<Weapon, WeaponEntity> meleeWeaponRepository, 
            IWrappingRepository<Weapon, WeaponEntity> rangedWeaponRepository,
            IWrappingRepository<ArmorSet, ArmorSetEntity> armorWeaponRepository)
        {
            _meleeWeaponRepository = meleeWeaponRepository;
            _rangedWeaponRepository = rangedWeaponRepository;
            _armorWeaponRepository = armorWeaponRepository;
        }

       

        public async Task RefreshWikiData(TableInfoType type)
        {
            switch (type)
            {
                case TableInfoType.Armor:
                {
                    var armor = ParseArmorFromUrl(ArmorUrl);
                    await _armorWeaponRepository.DeleteAllItems();
                    await _armorWeaponRepository.AddAllItems(armor);
                    break;
                }
                case TableInfoType.MeleeWeapons:
                {
                    var meleeWeapons = ParseCloseRangeWeaponsFromUrl(MeleeWeaponUrl);
                    await _meleeWeaponRepository.DeleteAllItems();
                    await _meleeWeaponRepository.AddAllItems(meleeWeapons);
                    break;
                }

                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }

            await database.Update(new TableInfoModel() { TimeStamp = DateTime.Now, Type = type });
        }

        #region Armor
        private List<ArmorSet> ParseArmorFromUrl(string url)
        {
            var web = new HtmlWeb();
            var doc = web.Load(url);
            var armorSets = new List<ArmorSet>();

            //parse complete table
            foreach (var table in doc.DocumentNode.SelectNodes("//table[@class='wikitable']"))
            {
                var armorParts = new Dictionary<ArmorPartType, ArmorModel>();

                var rows = table.SelectNodes("tr");
                var header = rows[0];
                var headerData = header.SelectNodes("th");

                var armorName = headerData[0].InnerText;
                CleanUpString(ref armorName);

                //parse each row
                foreach (var dataRow in rows.Skip(1))
                {
                    var dataCells = dataRow.SelectNodes("td");
                    var bodyPart = ParseBodyPart(dataCells[0].InnerText);
                    
                    var physical = dataCells[1].InnerText;
                    var energy = dataCells[2].InnerText;
                    var load = dataCells[3].InnerText;
                    var durability = dataCells[4].InnerText;

                    var armor = new ArmorModel(armorName, int.Parse(physical), int.Parse(energy),
                        int.Parse(load), int.Parse(durability));

                    armorParts.Add(bodyPart, armor);
                }

                armorSets.Add(new ArmorSet(armorParts));
            }

            return armorSets;
        }
      
        private ArmorPartType ParseBodyPart(string name)
        {
            CleanUpString(ref name);

            if (name.Equals("Helm"))
                return ArmorPartType.Helm;
            if (name.Equals("Torso"))
                return ArmorPartType.Torso;
            if (name.Equals("Arm"))
                return ArmorPartType.Arm;
            if (name.Equals("Bein"))
                return ArmorPartType.Bein;

            Debug.WriteLine($"Unable to parse {nameof(ArmorPartType)} by value \"{name}\"");
            return ArmorPartType.Unknown;
        }
        #endregion

        #region CloseRangeWeaons
        public List<Weapon> ParseCloseRangeWeaponsFromUrl(string url)
        {
            var web = new HtmlWeb();
            var doc = web.Load(url);
            var weapons = new List<Weapon>();

            //parse complete table
            foreach (var table in doc.DocumentNode.SelectNodes("//table[@class='wikitable']"))
            {
                var weaponStances = new Dictionary<WeaponStanceType, WeaponStance>();

                var rows = table.SelectNodes("tr");
                var header = rows[0];
                var headerData = header.SelectNodes("th");

                var weaponName = headerData[0].InnerText;
                CleanUpString(ref weaponName);

                var firstRow = rows[1].SelectNodes("td");

                var loadValue = firstRow[6].InnerText;
                var durabilityValue = firstRow[7].InnerText;

                //parse each row
                foreach (var dataRow in rows.Skip(1))
                {
                    var dataCells = dataRow.SelectNodes("td");
                    var weaponStanceType = ParseWeaponStance(dataCells[0].InnerText);

                    var phase = dataCells[2].InnerText;
                    var damage = dataCells[3].InnerText;
                    var parry = dataCells[4].InnerText;
                    
                    var weaponStance = new WeaponStance(weaponStanceType, phase, damage, int.Parse(parry), "nah");
                    weaponStances.Add(weaponStanceType, weaponStance);
                }

                weapons.Add(new Weapon(weaponName, weaponStances, int.Parse(loadValue), int.Parse(durabilityValue)));
            }

            return weapons;
        }

        private WeaponStanceType ParseWeaponStance(string name)
        {
            CleanUpString(ref name);

            if (name.Equals("leichte Haltung"))
                return WeaponStanceType.Light;
            if (name.Equals("schwere Haltung"))
                return WeaponStanceType.Heavy;

            Debug.WriteLine($"Unable to parse {nameof(WeaponStanceType)} by value \"{name}\"");
            return WeaponStanceType.Unknown;
        }
        #endregion

        private void CleanUpString(ref string value)
        {
            value = value.Replace("\n", "").Replace("\r", "");
        }
    }
}