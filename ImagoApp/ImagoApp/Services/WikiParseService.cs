using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;
using ImagoApp.Application.Models;
using ImagoApp.Application.Services;
using ImagoApp.Infrastructure.Repositories;
using ImagoApp.Shared.Enums;
using Microsoft.Extensions.Logging;

namespace ImagoApp.Services
{
    public interface IWikiParseService
    {
        Task<int?> RefreshArmorFromWiki(ILogger logger);
        Task<int?> RefreshWeaponsFromWiki(ILogger logger);
        Task<int?> RefreshTalentsFromWiki(ILogger logger);
        Task<int?> RefreshMasteriesFromWiki(ILogger logger);
    }

    public class WikiParseService : IWikiParseService
    {
        private readonly IWikiDataService _wikiDataService;
        
        public WikiParseService(IWikiDataService wikiDataService)
        {
            _wikiDataService = wikiDataService;
        }

        public async Task<int?> RefreshArmorFromWiki(ILogger logger)
        {
            var armor = ParseArmorFromUrl(Util.WikiConstants.ArmorUrl, logger );
            await _wikiDataService.DeleteAllArmor();
            return await _wikiDataService.AddArmor(armor);
        }

        public async Task<int?> RefreshWeaponsFromWiki(ILogger logger)
        {
            var meleeWeapons = ParseWeaponsFromUrl(Util.WikiConstants.MeleeWeaponUrl, logger);
            var rangedWeapons = ParseWeaponsFromUrl(Util.WikiConstants.RangedWeaponUrl, logger);
            var specialWeapons = ParseWeaponsFromUrl(Util.WikiConstants.SpecialWeaponUrl, logger);
            var shields = ParseWeaponsFromUrl(Util.WikiConstants.ShieldsUrl, logger);
            await _wikiDataService.DeleteAllWeapons();
            var result = 0;
            result += await _wikiDataService.AddWeapons(meleeWeapons);
            result += await _wikiDataService.AddWeapons(rangedWeapons);
            result += await _wikiDataService.AddWeapons(specialWeapons);
            result += await _wikiDataService.AddWeapons(shields);
            return result;
        }

        public async Task<int?> RefreshTalentsFromWiki(ILogger logger)
        {
            var talents = ParseTalentsFromUrls(Util.WikiConstants.ParsableSkillTypeLookUp, logger);
            await _wikiDataService.DeleteAllTalents();
            return await _wikiDataService.AddTalents(talents);
        }

        public async Task<int?> RefreshMasteriesFromWiki(ILogger logger)
        {
            var masteries = ParseMasteriesFromUrls(Util.WikiConstants.SkillGroupTypeLookUp, logger);
            await _wikiDataService.DeleteAllTalents();
            return await _wikiDataService.AddMasteries(masteries);
        }
        
        private List<ArmorPartModel> ParseArmorFromUrl(string url, ILogger logger )
        {
            var result = new List<ArmorPartModel>();

            var doc = Util.WikiHelper.LoadDocumentFromUrl(url, logger);
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

                        var armor = new ArmorPartModel(bodyPart, armorName, int.Parse(load), false, false,
                            int.Parse(durability), int.Parse(energy), int.Parse(physical));
                        result.Add(armor);
                    }
                }
                catch (Exception e)
                {
                    logger.LogError(e, $"Werte für Rüstung \"{armorName}\" konnten nicht gelesen werden");
                }
            }

            logger.LogInformation($"Rüstungen hinzugefügt [{string.Join(", ", result.Select(set => set.Name))}]");
            return result;
        }

        private ArmorPartType ParseBodyPart(string name, ILogger logger)
        {
            if (name.Equals("Helm"))
                return ArmorPartType.Helm;
            if (name.Equals("Torso"))
                return ArmorPartType.Torso;
            if (name.Equals("Arm"))
                return ArmorPartType.Arm;
            if (name.Equals("Bein"))
                return ArmorPartType.Bein;

            logger.LogError( $"Zuordnung von Rüstungsteil konnte aus Wert \"{name}\" nicht gelesen werden");
            return ArmorPartType.Unknown;
        }
        
        private List<Weapon> ParseWeaponsFromUrl(string url, ILogger logger)
        {
            var result = new List<Weapon>();
            var doc = Util.WikiHelper.LoadDocumentFromUrl(url, logger);
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

                    result.Add(new Weapon(weaponName, weaponStances, false, false, int.Parse(loadValue), int.Parse(durabilityValue)));
                }
                catch (Exception e)
                {
                    logger.LogError(e, $"Werte für die Waffe \"{weaponName}\" konnten nicht von \"{url}\" gelesen werden.{Environment.NewLine}Fehler:{e}");
                }
            }

            logger.LogInformation( $"Waffen hinzugefügt [{string.Join(", ", result.Select(weapon => weapon.Name))}]");
            return result;
        }
        
        private string CleanUpString(string value)
        {
            return value.Replace("\n", "").Replace("\r", "").Trim();
        }

        private List<TalentModel> ParseTalentsFromUrls(Dictionary<SkillModelType, string> urls, ILogger logger)
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

        private List<TalentModel> ParseTalentsFromUrl(SkillModelType modelType, string url, ILogger logger)
        {
            var talents = new List<TalentModel>();
            var doc = Util.WikiHelper.LoadDocumentFromUrl(url, logger);
            if (doc == null)
                return talents;

            var descriptions =  GetTalentDescriptions(doc);

            var table = doc.DocumentNode.SelectSingleNode("//table[@class='wikitable']");
            if (table == null)
            {
                logger.LogWarning($"Keine Tabelle mit Werten auf \"{url}\" gefunden");
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

                    var requirements = new List<RequirementModel<SkillModelType>>();

                    foreach (var requirement in requirementsRawValue.Split(',').Select(s => s.Trim()))
                    {
                        var strings = requirement.Split(' ');
                        var skill = MappingStringToSkillType(strings[0], logger);
                        if (skill == SkillModelType.Unbekannt)
                        {
                            logger.LogError($"Vorraussetztung \"{requirement}\" für Talent \"{name}\" kann nicht gelesen werden .. wird ignoriert");
                            continue;
                        }

                        var value = int.Parse(strings[1]);

                        requirements.Add(new RequirementModel<SkillModelType>(skill, value));
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
                        logger.LogWarning($"Talent \"{name}\" hat keine Kurzbeschreibung");
                    }

                    var desc = string.Empty;
                    if (!descriptions.ContainsKey(name))
                    {
                        logger.LogWarning( $"Keine Beschreibung zu \"{name}\" gefunden {url}");
                    }
                    else
                    {
                        desc = descriptions[name];
                        if (string.IsNullOrWhiteSpace(desc))
                        {
                            logger.LogWarning( $"Nur eine leere Beschreibung zu \"{name}\" gefunden {url}");
                        }
                    }

                    talents.Add(new TalentModel(modelType, name, shortDescription, desc, requirements, difficulty, activeUse,
                        phaseValueMod));
                }
                catch (Exception exception)
                {
                    logger.LogError(exception,   $"Talent \"{name}\" kann nicht von \"{url}\" gelesen werden.{Environment.NewLine}Fehler:{exception}");
                }
            }

            logger.LogInformation($"Talente für Fertigkeit \"{modelType}\" hinzugefügt [{string.Join(", ", talents.Select(model => model.Name))}]");
            return talents;
        }

        private int? ParseStringToDifficultyForTalent(string value, string talentName,string url, ILogger logger)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                logger.LogError($"Schwierigkeit \"{value}\" kann von {talentName} \"{url}\" nicht gelesen werden. Die Schwierigkeit wird als konfigurierbar hinterlegt.");
                return null;
            }

            //user can configure the value
            if (value.Equals("speziell"))
                return null;

            var parsed = int.TryParse(value, out int parsedValue);
            if (!parsed)
            {
                logger.LogError( $"Schwierigkeit \"{value}\" kann von {talentName} \"{url}\" nicht in eine Zahl konvertiert werden. Die Schwierigkeit wird als konfigurierbar hinterlegt.");
                return null;
            }

            return parsedValue;
        }

        private SkillModelType MappingStringToSkillType(string value, ILogger logger)
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

            logger.LogError( $"Keine Fertigkeit für den Wert \"{value}\" hinterlegt");
            return SkillModelType.Unbekannt;
        }

        private SkillGroupModelType MappingStringToSkillGroupType(string value, ILogger logger)
        {
            if (Enum.TryParse(value, out SkillGroupModelType castValue))
                return castValue;

            if (value.Equals("Weben"))
                return SkillGroupModelType.Webkunst;

            logger.LogError( $"Keine Fertigkeitskategorie für den Wert \"{value}\" hinterlegt");
            return SkillGroupModelType.Unbekannt;
        }

        private bool MapToActiveUse(string value, ILogger logger)
        {
            if (value.Equals("passiv"))
                return false;
            if (value.Equals("aktiv"))
                return true;

            logger.LogError($"Keinen Einsatz für den Wert \"{value}\" hinterlegt");
            return false;
        }
        
        private List<MasteryModel> ParseMasteriesFromUrls(Dictionary<SkillGroupModelType, string> urls, ILogger logger)
        {
            var talents = new List<MasteryModel>();
            foreach (var item in urls)
            {
                talents.AddRange(ParseMasteriesFromUrl(item.Key, item.Value, logger));
            }

            return talents;
        }

        private List<MasteryModel> ParseMasteriesFromUrl(SkillGroupModelType modelType, string url, ILogger logger)
        {
            var talents = new List<MasteryModel>();

            var doc = Util.WikiHelper.LoadDocumentFromUrl(url, logger);
            if (doc == null)
                return talents;

            var descriptions = GetTalentDescriptions(doc);


            var table = doc.DocumentNode.SelectSingleNode("//table[@class='wikitable']");
            if (table == null)
            {
                logger.LogWarning($"Keine Tabelle mit Werten auf \"{url}\" gefunden");
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

                    var requirements = new List<RequirementModel<SkillGroupModelType>>();

                    foreach (var requirement in requirementsRawValue.Split(',').Select(s => s.Trim()))
                    {
                        var strings = requirement.Split(' ');
                        var skill = MappingStringToSkillGroupType(strings[0], logger);
                        if (skill == SkillGroupModelType.Unbekannt)
                        {
                            logger.LogError($"Vorraussetztung \"{requirement}\" für Talent \"{name}\" kann nicht gelesen werden .. wird ignoriert");
                            continue;
                        }

                        var value = int.Parse(strings[1]);

                        requirements.Add(new RequirementModel<SkillGroupModelType>(skill, value));
                    }

                    var difficultyValue = CleanUpString(dataCells[2].InnerText);
                    var difficulty = ParseStringToDifficultyForTalent(difficultyValue, name, url, logger);
                    var activeUse = MapToActiveUse(CleanUpString(dataCells[3].InnerText), logger);
                    var phaseValueMod = CleanUpString(dataCells[4].InnerText);

                    var shortDescription = string.Empty;

                    if (dataCells.Count > 5)
                        shortDescription = CleanUpString(dataCells[5].InnerText);
                    else
                        logger.LogWarning($"Meisterschaft \"{name}\" hat keine Kurzbeschreibung");

                    var desc = string.Empty;
                    if (!descriptions.ContainsKey(name))
                    {
                        logger.LogWarning( $"Keine Beschreibung zu \"{name}\" gefunden {url}");
                    }
                    else
                    {
                        desc = descriptions[name];
                        if (string.IsNullOrWhiteSpace(desc))
                        {
                            logger.LogWarning( $"Nur eine leere Beschreibung zu \"{name}\" gefunden {url}");
                        }
                    }

                    talents.Add(new MasteryModel(modelType, name, shortDescription, desc, requirements, difficulty, activeUse, phaseValueMod));
                }
                catch (Exception exception)
                {
                    logger.LogError(exception, $"Talent \"{name}\" kann nicht von \"{url}\" gelesen werden");
                }
            }

            logger.LogInformation($"Talente für Fertigkeit \"{modelType}\" hinzugefügt [{string.Join(", ", talents.Select(model => model.Name))}]");
            return talents;
        }

    }
}