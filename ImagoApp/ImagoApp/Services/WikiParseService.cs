using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;
using ImagoApp.Application.Models;
using ImagoApp.Application.Models.Template;
using ImagoApp.Application.Services;
using ImagoApp.Infrastructure.Repositories;
using ImagoApp.Shared.Enums;
using ImagoApp.Util;
using Serilog.Core;

namespace ImagoApp.Services
{
    public interface IWikiParseService
    {
        int? RefreshArmorFromWiki(Logger logger);
        int? RefreshWeaponsFromWiki(Logger logger);
        int? RefreshTalentsFromWiki(Logger logger);
        int? RefreshMasteriesFromWiki(Logger logger);
    }

    public class WikiParseService : IWikiParseService
    {
        private readonly IWikiDataService _wikiDataService;
        
        public WikiParseService(IWikiDataService wikiDataService)
        {
            _wikiDataService = wikiDataService;
        }

        public int? RefreshArmorFromWiki(Logger logger)
        {
            var armor = ParseArmorFromUrl(WikiConstants.ArmorUrl, logger );
            _wikiDataService.DeleteAllArmor();
            _wikiDataService.AddArmor(armor);
            return armor.Count;
        }

        public int? RefreshWeaponsFromWiki(Logger logger)
        {
            var meleeWeapons = ParseWeaponsFromUrl(WikiConstants.MeleeWeaponUrl, logger);
            var rangedWeapons = ParseWeaponsFromUrl(WikiConstants.RangedWeaponUrl, logger);
            var specialWeapons = ParseWeaponsFromUrl(WikiConstants.SpecialWeaponUrl, logger);
            var shields = ParseWeaponsFromUrl(WikiConstants.ShieldsUrl, logger);
            _wikiDataService.DeleteAllWeapons();

            _wikiDataService.AddWeapons(meleeWeapons);
            _wikiDataService.AddWeapons(rangedWeapons);
            _wikiDataService.AddWeapons(specialWeapons);
            _wikiDataService.AddWeapons(shields);
            return meleeWeapons.Count + rangedWeapons.Count + specialWeapons.Count + shields.Count;
        }

        public int? RefreshTalentsFromWiki(Logger logger)
        {
            var talents = ParseTalentsFromUrls(WikiConstants.ParsableSkillTypeLookUp, logger);
            _wikiDataService.DeleteAllTalents();
            _wikiDataService.AddTalents(talents);
            return talents.Count;
        }

        public int? RefreshMasteriesFromWiki(Logger logger)
        {
            var masteries = ParseMasteriesFromUrls(WikiConstants.SkillGroupTypeLookUp, logger);
            _wikiDataService.DeleteAllMasteries();
            _wikiDataService.AddMasteries(masteries);
            return masteries.Count;
        }
        
        private List<ArmorPartTemplateModel> ParseArmorFromUrl(string url, Logger logger )
        {
            var result = new List<ArmorPartTemplateModel>();

            var doc = WikiHelper.LoadDocumentFromUrl(url, logger);
            if (doc == null)
                return result;

            //parse complete table
            foreach (var table in doc.DocumentNode.SelectNodes("//table[@class='wikitable']"))
            {
                var armorName = string.Empty;
                try
                {
                    var rows = table.SelectNodes("tr");
                    var header = rows[0];
                    var headerData = header.SelectNodes("th");

                    armorName = CleanUpString(headerData[0].InnerText);

                    //parse each row
                    foreach (var dataRow in rows.Skip(1))
                    {
                        var dataCells = dataRow.SelectNodes("td");
                        var bodyPart = ParseBodyPart(CleanUpString(dataCells[0].InnerText), logger);

                        var physical = CleanUpString(dataCells[1].InnerText);
                        var energy = CleanUpString(dataCells[2].InnerText);
                        var load = CleanUpString(dataCells[3].InnerText);
                        var durability = CleanUpString(dataCells[4].InnerText);

                        var armor = new ArmorPartTemplateModel()
                        {
                            Name = armorName,
                            ArmorPartType = bodyPart,
                            LoadValue = int.Parse(load),
                            DurabilityValue = int.Parse(durability),
                            EnergyDefense = int.Parse(energy),
                            PhysicalDefense = int.Parse(physical)
                        };

                        result.Add(armor);
                    }
                }
                catch (Exception e)
                {
                    logger.Error(e, $"Werte für Rüstung \"{armorName}\" konnten nicht gelesen werden");
                }
            }

            logger.Information($"Rüstungen hinzugefügt [{string.Join(", ", result.Select(set => set.Name))}]");
            return result;
        }

        private ArmorPartType ParseBodyPart(string name, Logger logger)
        {
            if (name.Equals("Helm"))
                return ArmorPartType.Helm;
            if (name.Equals("Torso"))
                return ArmorPartType.Torso;
            if (name.Equals("Arm"))
                return ArmorPartType.Arm;
            if (name.Equals("Bein"))
                return ArmorPartType.Bein;

            logger.Error( $"Zuordnung von Rüstungsteil konnte aus Wert \"{name}\" nicht gelesen werden");
            return ArmorPartType.Unknown;
        }
        
        private List<WeaponTemplateModel> ParseWeaponsFromUrl(string url, Logger logger)
        {
            var result = new List<WeaponTemplateModel>();
            var doc = WikiHelper.LoadDocumentFromUrl(url, logger);
            if (doc == null)
                return result;

            //parse complete table
            foreach (var table in doc.DocumentNode.SelectNodes("//table[@class='wikitable']"))
            {
                var weaponName = string.Empty;
                try
                {
                    var weaponStances = new List<WeaponStance>();

                    var rows = table.SelectNodes("tr");
                    var header = rows[0];
                    var headerData = header.SelectNodes("th");

                    weaponName = CleanUpString(headerData[0].InnerText);

                    var firstRow = rows[1].SelectNodes("td");

                    var loadValue = CleanUpString(firstRow[6].InnerText);
                    if (loadValue.Equals("-"))
                        loadValue = "0";

                    var durabilityValue = CleanUpString(firstRow[7].InnerText);
                    if (durabilityValue.Equals("-"))
                        durabilityValue = "0";

                    //parse each row
                    foreach (var dataRow in rows.Skip(1))
                    {
                        var dataCells = dataRow.SelectNodes("td");
                        var weaponStanceType = CleanUpString(dataCells[0].InnerText);

                        var phase = CleanUpString(dataCells[1].InnerText);
                        var damage = CleanUpString(dataCells[2].InnerText);
                        var parry = CleanUpString(dataCells[3].InnerText);
                        var range = CleanUpString(dataCells[4].InnerText);
                        
                        weaponStances.Add(new WeaponStance(weaponStanceType, phase, damage, parry, range));
                    }

                    result.Add(new WeaponTemplateModel
                    {
                        LoadValue = int.Parse(loadValue),
                        Name = weaponName,
                        DurabilityValue = int.Parse(durabilityValue),
                        WeaponStances = weaponStances
                    });
                }
                catch (Exception e)
                {
                    logger.Error(e, $"Werte für die Waffe \"{weaponName}\" konnten nicht von \"{url}\" gelesen werden.{Environment.NewLine}Fehler:{e}");
                }
            }

            logger.Information( $"Waffen hinzugefügt [{string.Join(", ", result.Select(weapon => weapon.Name))}]");
            return result;
        }
        
        private string CleanUpString(string value)
        {
            return value.Replace("\n", "").Replace("\r", "").Trim();
        }

        private List<TalentModel> ParseTalentsFromUrls(Dictionary<SkillModelType, string> urls, Logger logger)
        {
            var talents = new List<TalentModel>();
            foreach (var item in urls)
            {
                talents.AddRange(ParseTalentsFromUrl(item.Key, item.Value, logger));
            }

            return talents;
        }

        private static readonly List<string> DescriptionFilter = new List<string>()
        {
            "Fertigkeit",
            "Freischaltung",
            "Künste",
            "Variationen"
        };

        private Dictionary<string, string> GetTalentDescriptions(HtmlDocument document)
        {
            var result = new Dictionary<string, string>();
            var headLines = document.DocumentNode.SelectNodes("//span[@class='mw-headline']");

            foreach (var headLine in headLines)
            {
                var header = CleanUpString(headLine.InnerText);

                if (DescriptionFilter.Contains(header))
                    continue;

                var parent = headLine.ParentNode;
                var description = "";

                var next = parent?.NextSibling?.NextSibling;
                if (next == null)
                    continue;

                while (next.Name.Equals("p"))
                {
                    description += next.InnerText;
                    next = next.NextSibling?.NextSibling;
                    if (next == null)
                        break;
                }

                if (string.IsNullOrWhiteSpace(description))
                    continue;

                result.Add(header, description);
            }

            return result;
        }

        private List<TalentModel> ParseTalentsFromUrl(SkillModelType modelType, string url, Logger logger)
        {
            var talents = new List<TalentModel>();
            var doc = WikiHelper.LoadDocumentFromUrl(url, logger);
            if (doc == null)
                return talents;

            var descriptions =  GetTalentDescriptions(doc);

            var table = doc.DocumentNode.SelectSingleNode("//table[@class='wikitable']");
            if (table == null)
            {
                logger.Warning($"Keine Tabelle mit Werten auf \"{url}\" gefunden");
                return talents;
            }

            var rows = table.SelectNodes("tr");

            //parse each row
            foreach (var talentDataRow in rows.Skip(1))
            {
                var name = string.Empty;

                try
                {
                    var dataCells = talentDataRow.SelectNodes("td");
                    name = CleanUpString(dataCells[0].InnerText);
                    var requirementsRawValue = CleanUpString(dataCells[1].InnerText);

                    var requirements = new List<SkillRequirementModel>();

                    foreach (var requirement in requirementsRawValue.Split(',').Select(s => s.Trim()))
                    {
                        var strings = requirement.Split(' ');
                        var skill = MappingStringToSkillType(strings[0], logger);
                        if (skill == SkillModelType.Unbekannt)
                        {
                            logger.Error($"Vorraussetztung \"{requirement}\" für Talent \"{name}\" kann nicht gelesen werden .. wird ignoriert");
                            continue;
                        }

                        var value = int.Parse(strings[1]);

                        requirements.Add(new SkillRequirementModel(skill, value));
                    }

                    var difficultyValue = CleanUpString(dataCells[2].InnerText);
                    var difficulty = ParseStringToDifficultyForTalent(difficultyValue, name, url, logger);
                    var activeUse = MapToActiveUse(CleanUpString(dataCells[3].InnerText), logger);
                    var phaseValueMod = CleanUpString(dataCells[4].InnerText);
                    
                    var shortDescription = string.Empty;

                    if (dataCells.Count > 5)
                    {
                        shortDescription = CleanUpString(dataCells[5].InnerText);
                    }
                    else
                    {
                        logger.Warning($"Talent \"{name}\" hat keine Kurzbeschreibung");
                    }

                    var desc = string.Empty;
                    if (!descriptions.ContainsKey(name))
                    {
                        logger.Warning( $"Keine Beschreibung zu \"{name}\" gefunden {url}");
                    }
                    else
                    {
                        desc = descriptions[name];
                        if (string.IsNullOrWhiteSpace(desc))
                        {
                            logger.Warning( $"Nur eine leere Beschreibung zu \"{name}\" gefunden {url}");
                        }
                    }

                    talents.Add(new TalentModel(modelType, name, shortDescription, desc, requirements, difficulty, activeUse,
                        phaseValueMod));
                }
                catch (Exception exception)
                {
                    logger.Error(exception,   $"Talent \"{name}\" kann nicht von \"{url}\" gelesen werden.{Environment.NewLine}Fehler:{exception}");
                }
            }

            logger.Information($"Talente für Fertigkeit \"{modelType}\" hinzugefügt [{string.Join(", ", talents.Select(model => model.Name))}]");
            return talents;
        }

        private int? ParseStringToDifficultyForTalent(string value, string talentName,string url, Logger logger)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                logger.Error($"Schwierigkeit \"{value}\" kann von {talentName} \"{url}\" nicht gelesen werden. Die Schwierigkeit wird als konfigurierbar hinterlegt.");
                return null;
            }

            //user can configure the value
            if (value.Equals("speziell"))
                return null;

            var parsed = int.TryParse(value, out int parsedValue);
            if (!parsed)
            {
                logger.Error( $"Schwierigkeit \"{value}\" kann von {talentName} \"{url}\" nicht in eine Zahl konvertiert werden. Die Schwierigkeit wird als konfigurierbar hinterlegt.");
                return null;
            }

            return parsedValue;
        }

        private SkillModelType MappingStringToSkillType(string value, Logger logger)
        {
            if (Enum.TryParse(value, out SkillModelType castValue))
                return castValue;

            if (value.Equals("Armbrüste"))
                return SkillModelType.Armbrueste;

            if (value.Equals("Körperbeherrschung"))
                return SkillModelType.Koerperbeherrschung;

            if (value.Equals("Bögen"))
                return SkillModelType.Boegen;

            if (value.Equals("Stäbe"))
                return SkillModelType.StaebeSpeere;

            if (value.Equals("Zweihänder"))
                return SkillModelType.Zweihaender;

            logger.Error( $"Keine Fertigkeit für den Wert \"{value}\" hinterlegt");
            return SkillModelType.Unbekannt;
        }

        private SkillGroupModelType MappingStringToSkillGroupType(string value, Logger logger)
        {
            if (Enum.TryParse(value, out SkillGroupModelType castValue))
                return castValue;

            if (value.Equals("Weben"))
                return SkillGroupModelType.Webkunst;

            logger.Error( $"Keine Fertigkeitskategorie für den Wert \"{value}\" hinterlegt");
            return SkillGroupModelType.Unbekannt;
        }

        private bool MapToActiveUse(string value, Logger logger)
        {
            if (value.Equals("passiv"))
                return false;
            if (value.Equals("aktiv"))
                return true;

            logger.Error($"Keinen Einsatz für den Wert \"{value}\" hinterlegt");
            return false;
        }
        
        private List<MasteryModel> ParseMasteriesFromUrls(Dictionary<SkillGroupModelType, string> urls, Logger logger)
        {
            var talents = new List<MasteryModel>();
            foreach (var item in urls)
            {
                talents.AddRange(ParseMasteriesFromUrl(item.Key, item.Value, logger));
            }

            return talents;
        }

        private List<MasteryModel> ParseMasteriesFromUrl(SkillGroupModelType modelType, string url, Logger logger)
        {
            var talents = new List<MasteryModel>();

            var doc = WikiHelper.LoadDocumentFromUrl(url, logger);
            if (doc == null)
                return talents;

            var descriptions = GetTalentDescriptions(doc);


            var table = doc.DocumentNode.SelectSingleNode("//table[@class='wikitable']");
            if (table == null)
            {
                logger.Warning($"Keine Tabelle mit Werten auf \"{url}\" gefunden");
                return talents;
            }
            
            var rows = table.SelectNodes("tr");

            //parse each row
            foreach (var talentDataRow in rows.Skip(1))
            {
                var name = string.Empty;

                try
                {
                    var dataCells = talentDataRow.SelectNodes("td");
                    name = CleanUpString(dataCells[0].InnerText);
                    var requirementsRawValue = CleanUpString(dataCells[1].InnerText);

                    var requirements = new List<SkillGroupRequirementModel>();

                    foreach (var requirement in requirementsRawValue.Split(',').Select(s => s.Trim()))
                    {
                        var strings = requirement.Split(' ');
                        var skill = MappingStringToSkillGroupType(strings[0], logger);
                        if (skill == SkillGroupModelType.Unbekannt)
                        {
                            logger.Error($"Vorraussetztung \"{requirement}\" für Talent \"{name}\" kann nicht gelesen werden .. wird ignoriert");
                            continue;
                        }

                        var value = int.Parse(strings[1]);

                        requirements.Add(new SkillGroupRequirementModel(skill, value));
                    }

                    var difficultyValue = CleanUpString(dataCells[2].InnerText);
                    var difficulty = ParseStringToDifficultyForTalent(difficultyValue, name, url, logger);
                    var activeUse = MapToActiveUse(CleanUpString(dataCells[3].InnerText), logger);
                    var phaseValueMod = CleanUpString(dataCells[4].InnerText);

                    var shortDescription = string.Empty;

                    if (dataCells.Count > 5)
                        shortDescription = CleanUpString(dataCells[5].InnerText);
                    else
                        logger.Warning($"Meisterschaft \"{name}\" hat keine Kurzbeschreibung");

                    var desc = string.Empty;
                    if (!descriptions.ContainsKey(name))
                    {
                        logger.Warning( $"Keine Beschreibung zu \"{name}\" gefunden {url}");
                    }
                    else
                    {
                        desc = descriptions[name];
                        if (string.IsNullOrWhiteSpace(desc))
                        {
                            logger.Warning( $"Nur eine leere Beschreibung zu \"{name}\" gefunden {url}");
                        }
                    }

                    talents.Add(new MasteryModel(modelType, name, shortDescription, desc, requirements, difficulty, activeUse, phaseValueMod));
                }
                catch (Exception exception)
                {
                    logger.Error(exception, $"Talent \"{name}\" kann nicht von \"{url}\" gelesen werden");
                }
            }

            logger.Information($"Talente für Fertigkeit \"{modelType}\" hinzugefügt [{string.Join(", ", talents.Select(model => model.Name))}]");
            return talents;
        }

    }
}