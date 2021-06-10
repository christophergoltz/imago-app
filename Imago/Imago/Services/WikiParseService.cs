using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Imago.Database;
using Imago.Models;
using Imago.Models.Enum;
using Imago.Shared;
using Imago.Shared.Models;

namespace Imago.Services
{
    public interface IWikiParseService
    {
        Task RefreshWikiData(TableInfoType type);
    }

    public class WikiParseService : IWikiParseService
    {
        private static readonly string ArmorUrl = "http://imago-rp.de/index.php/R%C3%BCstungen";

        public async Task RefreshWikiData(TableInfoType type)
        {
            switch (type)
            {
                case TableInfoType.Armor:
                {
                    var data = ParseArmorFromUrl(ArmorUrl);
                    var database =await LocalDatabase.Instance;
                    database.DeleteAllArmor();
                    await database.InsertMany(data);
                    await database.Update(new TableInfoModel() {TimeStamp = DateTime.Now, Type = type});
                    break;
                }
                case TableInfoType.Weapons:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        private List<ArmorSet> ParseArmorFromUrl(string url)
        {
            var web = new HtmlWeb();
            var doc = web.Load(url);
            var armorSets = new List<ArmorSet>();

            foreach (var table in doc.DocumentNode.SelectNodes("//table[@class='wikitable']"))
            {
                var armorParts = new Dictionary<ArmorPartType, ArmorModel>();

                var rows = table.SelectNodes("tr");
                var header = rows[0];
                var headerData = header.SelectNodes("th");

                var armorType = ParseArmorNameToArmorType(headerData[0].InnerText);
                if (armorType == ArmorModelType.Unknown)
                    continue;

                foreach (var dataRow in rows.Skip(1))
                {
                    var dataCells = dataRow.SelectNodes("td");
                    var bodyPart = ParseBodyPart(dataCells[0].InnerText);
                    
                    var physical = dataCells[1].InnerText;
                    var energy = dataCells[2].InnerText;
                    var load = dataCells[3].InnerText;
                    var durability = dataCells[4].InnerText;

                    var armor = new ArmorModel(armorType, int.Parse(physical), int.Parse(energy),
                        int.Parse(load), int.Parse(durability));

                    armorParts.Add(bodyPart, armor);
                }

                armorSets.Add(new ArmorSet(armorParts));
            }

            return armorSets;
        }

        private ArmorModelType ParseArmorNameToArmorType(string name)
        {
            CleanUpString(ref name);

            if (name.Equals("Chitinpanzer"))
                return ArmorModelType.Chitin;
            if (name.Equals("Holz-/Knochenpanzer"))
                return ArmorModelType.HolzKnochen;
            if (name.Equals("Kettenpanzer"))
                return ArmorModelType.Ketten;
            if (name.Equals("Kompositpanzer"))
                return ArmorModelType.Komposit;
            if (name.Equals("Plattenpanzer"))
                return ArmorModelType.Platten;
            if (name.Equals("Stepppanzer"))
                return ArmorModelType.Stepp;

            Debug.WriteLine($"Unable to parse {nameof(ArmorModelType)} by value \"{name}\"");
            return ArmorModelType.Unknown;
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

        private void CleanUpString(ref string value)
        {
            value = value.Replace("\n", "").Replace("\r", "");
        }
    }
}