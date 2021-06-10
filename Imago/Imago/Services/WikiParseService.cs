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
using Imago.Repository.WrappingDatabase;

namespace Imago.Services
{
    public interface IWikiParseService
    {
        Task<int?> RefreshWikiData(TableInfoType type);
    }

    public class WikiParseService : IWikiParseService
    {
        private static readonly string ArmorUrl = "http://imago-rp.de/index.php/R%C3%BCstungen";
        private static readonly string MeleeWeaponUrl = "http://imago-rp.de/index.php/Nahkampfwaffen";
        private static readonly string RangedWeaponUrl = "http://imago-rp.de/index.php/Fernkampfwaffen";

        private readonly IMeleeWeaponRepository _meleeWeaponRepository;
        private readonly IRangedWeaponRepository _rangedWeaponRepository;
        private readonly IArmorRepository _armorWeaponRepository;

        public WikiParseService(
            IMeleeWeaponRepository meleeWeaponRepository, 
            IRangedWeaponRepository rangedWeaponRepository,
            IArmorRepository armorWeaponRepository)
        {
            _meleeWeaponRepository = meleeWeaponRepository;
            _rangedWeaponRepository = rangedWeaponRepository;
            _armorWeaponRepository = armorWeaponRepository;
        }
        
        public async Task<int?> RefreshWikiData(TableInfoType type)
        {
            switch (type)
            {
                case TableInfoType.Armor:
                {
                    var armor = ParseArmorFromUrl(ArmorUrl);
                    await _armorWeaponRepository.DeleteAllItems();
                    return await _armorWeaponRepository.AddAllItems(armor);
                }
                case TableInfoType.MeleeWeapons:
                {
                    var meleeWeapons = ParseMeleeWeaponsFromUrl(MeleeWeaponUrl);
                    await _meleeWeaponRepository.DeleteAllItems();
                    return await _meleeWeaponRepository.AddAllItems(meleeWeapons);
                }
                case TableInfoType.RangedWeapons:
                {
                    var meleeWeapons = ParseRangedWeaponsFromUrl(RangedWeaponUrl);
                    await _rangedWeaponRepository.DeleteAllItems();
                    return await _rangedWeaponRepository.AddAllItems(meleeWeapons);
                }
            }

            Debug.WriteLine($"Unknown TableInfoType \"{type}\" for RefreshWikiData");
            return null;
        }

        #region Armor
        private IEnumerable<ArmorSet> ParseArmorFromUrl(string url)
        {
            var web = new HtmlWeb();
            var doc = web.Load(url);

            //parse complete table
            foreach (var table in doc.DocumentNode.SelectNodes("//table[@class='wikitable']"))
            {
                var armorParts = new Dictionary<ArmorPartType, ArmorModel>();

                var rows = table.SelectNodes("tr");
                var header = rows[0];
                var headerData = header.SelectNodes("th");

                var armorName = CleanUpString(headerData[0].InnerText);

                //parse each row
                foreach (var dataRow in rows.Skip(1))
                {
                    var dataCells = dataRow.SelectNodes("td");
                    var bodyPart = ParseBodyPart(CleanUpString(dataCells[0].InnerText));
                    
                    var physical = CleanUpString(dataCells[1].InnerText);
                    var energy = CleanUpString(dataCells[2].InnerText);
                    var load = CleanUpString(dataCells[3].InnerText);
                    var durability = CleanUpString(dataCells[4].InnerText);

                    var armor = new ArmorModel(armorName, int.Parse(physical), int.Parse(energy),
                        int.Parse(load), int.Parse(durability));

                    armorParts.Add(bodyPart, armor);
                }

                yield return new ArmorSet(armorParts);
            }
        }
      
        private ArmorPartType ParseBodyPart(string name)
        {
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
        
        public List<Weapon> ParseMeleeWeaponsFromUrl(string url)
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

                var weaponName = CleanUpString(headerData[0].InnerText);

                var firstRow = rows[1].SelectNodes("td");

                var loadValue = CleanUpString(firstRow[5].InnerText);
                var durabilityValue = CleanUpString(firstRow[6].InnerText);

                //parse each row
                foreach (var dataRow in rows.Skip(1))
                {
                    var dataCells = dataRow.SelectNodes("td");
                    var weaponStanceType = ParseWeaponStance(CleanUpString(dataCells[0].InnerText));

                    var phase = CleanUpString(dataCells[1].InnerText);
                    var damage = CleanUpString(dataCells[2].InnerText);
                    var parry = CleanUpString(dataCells[3].InnerText);
                    
                    var weaponStance = new WeaponStance(weaponStanceType, phase, damage, int.Parse(parry), "nah");
                    weaponStances.Add(weaponStanceType, weaponStance);
                }

                weapons.Add(new Weapon(weaponName, weaponStances, int.Parse(loadValue), int.Parse(durabilityValue)));
            }

            return weapons;
        }


        public List<Weapon> ParseRangedWeaponsFromUrl(string url)
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

                var weaponName = CleanUpString(headerData[0].InnerText);

                var firstRow = rows[1].SelectNodes("td");

                var loadValue = CleanUpString(firstRow[5].InnerText);
                var durabilityValue = CleanUpString(firstRow[6].InnerText);

                //parse each row
                foreach (var dataRow in rows.Skip(1))
                {
                    var dataCells = dataRow.SelectNodes("td");
                    var weaponStanceType = ParseWeaponStance(CleanUpString(dataCells[0].InnerText));

                    var phase = CleanUpString(dataCells[1].InnerText);
                    var damage = CleanUpString(dataCells[2].InnerText);
                    var range = CleanUpString(dataCells[3].InnerText);

                    var weaponStance = new WeaponStance(weaponStanceType, phase, damage, null , range);
                    weaponStances.Add(weaponStanceType, weaponStance);
                }

                weapons.Add(new Weapon(weaponName, weaponStances, int.Parse(loadValue), int.Parse(durabilityValue)));
            }

            return weapons;
        }


        private WeaponStanceType ParseWeaponStance(string name)
        {
            if (name.Equals("leichte Haltung"))
                return WeaponStanceType.Light;
            if (name.Equals("schwere Haltung"))
                return WeaponStanceType.Heavy;

            Debug.WriteLine($"Unable to parse {nameof(WeaponStanceType)} by value \"{name}\"");
            return WeaponStanceType.Unknown;
        }

        private string CleanUpString(string value)
        {
           return value.Replace("\n", "").Replace("\r", "");
        }
    }
}