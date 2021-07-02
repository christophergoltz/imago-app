using HtmlAgilityPack;
using Imago.Models;
using Imago.Models.Enum;
using Imago.Repository.WrappingDatabase;
using Imago.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Imago.Services
{
    public interface IWikiParseService
    {
        Task<int?> RefreshWikiData(TableInfoType type, ObservableCollection<LogEntry> logFeed);
    }

    public class WikiParseService : IWikiParseService
    {
        private readonly IMeleeWeaponRepository _meleeWeaponRepository;
        private readonly IRangedWeaponRepository _rangedWeaponRepository;
        private readonly IArmorRepository _armorWeaponRepository;
        private readonly ITalentRepository _talentRepository;
        private readonly ISpecialWeaponRepository _specialWeaponRepository;
        private readonly IShieldRepository _shieldRepository;
        private readonly IMasteryRepository _masteryRepository;

        public WikiParseService(
            IMeleeWeaponRepository meleeWeaponRepository,
            IRangedWeaponRepository rangedWeaponRepository,
            IArmorRepository armorWeaponRepository,
            ITalentRepository talentRepository,
            ISpecialWeaponRepository specialWeaponRepository,
            IShieldRepository shieldRepository,
            IMasteryRepository masteryRepository)
        {
            _meleeWeaponRepository = meleeWeaponRepository;
            _rangedWeaponRepository = rangedWeaponRepository;
            _armorWeaponRepository = armorWeaponRepository;
            _talentRepository = talentRepository;
            _specialWeaponRepository = specialWeaponRepository;
            _shieldRepository = shieldRepository;
            _masteryRepository = masteryRepository;
        }

        public async Task<int?> RefreshWikiData(TableInfoType type, ObservableCollection<LogEntry> logFeed)
        {
            switch (type)
            {
                case TableInfoType.Armor:
                {
                    var armor = ParseArmorFromUrl(WikiConstants.ArmorUrl, logFeed);
                    await _armorWeaponRepository.DeleteAllItems();
                    return await _armorWeaponRepository.AddAllItems(armor);
                }
                case TableInfoType.MeleeWeapons:
                {
                    var weapons = ParseWeaponsFromUrl(WikiConstants.MeleeWeaponUrl, logFeed);
                    await _meleeWeaponRepository.DeleteAllItems();
                    return await _meleeWeaponRepository.AddAllItems(weapons);
                }
                case TableInfoType.RangedWeapons:
                {
                    var weapons = ParseWeaponsFromUrl(WikiConstants.RangedWeaponUrl, logFeed);
                    await _rangedWeaponRepository.DeleteAllItems();
                    return await _rangedWeaponRepository.AddAllItems(weapons);
                }
                case TableInfoType.SpecialWeapons:
                {
                    var weapons = ParseWeaponsFromUrl(WikiConstants.SpecialWeaponUrl, logFeed);
                    await _specialWeaponRepository.DeleteAllItems();
                    return await _specialWeaponRepository.AddAllItems(weapons);
                }
                case TableInfoType.Shields:
                {
                    var weapons = ParseWeaponsFromUrl(WikiConstants.ShieldsUrl, logFeed);
                    await _shieldRepository.DeleteAllItems();
                    return await _shieldRepository.AddAllItems(weapons);
                }
                case TableInfoType.Talents:
                {
                    var talents = ParseTalentsFromUrls(WikiConstants.ParsableSkillTypeLookUp, logFeed);
                    await _talentRepository.DeleteAllItems();
                    return await _talentRepository.AddAllItems(talents);
                }
                case TableInfoType.Masteries:
                {
                    var masteries = ParseMasteriesFromUrls(WikiConstants.SkillGroupTypeLookUp, logFeed);
                    await _masteryRepository.DeleteAllItems();
                    return await _masteryRepository.AddAllItems(masteries);
                }
            }

            logFeed.Add(new LogEntry(LogEntryType.Error, $"Keine Parse-Funktion für \"{type}\" hinterlegt"));
            return null;
        }

        private List<ArmorSet> ParseArmorFromUrl(string url, ObservableCollection<LogEntry> logFeed)
        {
            var result = new List<ArmorSet>();

            var doc = WikiHelper.LoadDocumentFromUrl(url, logFeed);
            if (doc == null)
                return result;

            //parse complete table
            foreach (var table in doc.DocumentNode.SelectNodes("//table[@class='wikitable']"))
            {
                var armorName = string.Empty;
                try
                {
                    var armorParts = new Dictionary<ArmorPartType, ArmorModel>();

                    var rows = table.SelectNodes("tr");
                    var header = rows[0];
                    var headerData = header.SelectNodes("th");

                    armorName = CleanUpString(headerData[0].InnerText);

                    //parse each row
                    foreach (var dataRow in rows.Skip(1))
                    {
                        var dataCells = dataRow.SelectNodes("td");
                        var bodyPart = ParseBodyPart(CleanUpString(dataCells[0].InnerText), logFeed);

                        var physical = CleanUpString(dataCells[1].InnerText);
                        var energy = CleanUpString(dataCells[2].InnerText);
                        var load = CleanUpString(dataCells[3].InnerText);
                        var durability = CleanUpString(dataCells[4].InnerText);

                        var armor = new ArmorModel(armorName, int.Parse(load), false, false, int.Parse(durability), int.Parse(energy), int.Parse(physical));

                        armorParts.Add(bodyPart, armor);
                    }


                    result.Add(new ArmorSet(armorParts));
                }
                catch (Exception e)
                {
                    logFeed.Add(new LogEntry(LogEntryType.Error,
                        $"Werte für Rüstung \"{armorName}\" konnten nicht gelesen werden.{Environment.NewLine}Fehler:{e}"));
                }
            }

            logFeed.Add(new LogEntry(LogEntryType.Success,
                $"Rüstungen hinzugefügt [{string.Join(", ", result.Select(set => set.ArmorParts.First().Value.Name))}]"));
            return result;
        }

        private ArmorPartType ParseBodyPart(string name, ObservableCollection<LogEntry> logFeed)
        {
            if (name.Equals("Helm"))
                return ArmorPartType.Helm;
            if (name.Equals("Torso"))
                return ArmorPartType.Torso;
            if (name.Equals("Arm"))
                return ArmorPartType.Arm;
            if (name.Equals("Bein"))
                return ArmorPartType.Bein;

            logFeed.Add(
                new LogEntry(LogEntryType.Error,
                    $"Zuordnung von Rüstungsteil konnte aus Wert \"{name}\" nicht gelesen werden"));
            return ArmorPartType.Unknown;
        }
        
        private List<Weapon> ParseWeaponsFromUrl(string url, ObservableCollection<LogEntry> logFeed)
        {
            var result = new List<Weapon>();
            var doc = WikiHelper.LoadDocumentFromUrl(url, logFeed);
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
                    logFeed.Add(new LogEntry(LogEntryType.Error,
                        $"Werte für die Waffe \"{weaponName}\" konnten nicht von \"{url}\" gelesen werden.{Environment.NewLine}Fehler:{e}"));
                }
            }

            logFeed.Add(new LogEntry(LogEntryType.Success,
                $"Waffen hinzugefügt [{string.Join(", ", result.Select(weapon => weapon.Name))}]"));
            return result;
        }
        
        private string CleanUpString(string value)
        {
            return value.Replace("\n", "").Replace("\r", "").Trim();
        }

        private IEnumerable<TalentModel> ParseTalentsFromUrls(Dictionary<SkillModelType, string> urls,
            ObservableCollection<LogEntry> logFeed)
        {
            var talents = new List<TalentModel>();
            foreach (var item in urls)
            {
                talents.AddRange(ParseTalentsFromUrl(item.Key, item.Value, logFeed));
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

        private List<TalentModel> ParseTalentsFromUrl(SkillModelType modelType, string url,
            ObservableCollection<LogEntry> logFeed)
        {
            var talents = new List<TalentModel>();
            var doc = WikiHelper.LoadDocumentFromUrl(url, logFeed);
            if (doc == null)
                return talents;

            var descriptions =  GetTalentDescriptions(doc);

            var table = doc.DocumentNode.SelectSingleNode("//table[@class='wikitable']");
            if (table == null)
            {
                logFeed.Add(new LogEntry(LogEntryType.Warning, $"Keine Tabelle mit Werten auf \"{url}\" gefunden"));
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

                    var requirements = new Dictionary<SkillModelType, int>();

                    foreach (var requirement in requirementsRawValue.Split(',').Select(s => s.Trim()))
                    {
                        var strings = requirement.Split(' ');
                        var skill = MappingStringToSkillType(strings[0], logFeed);
                        if (skill == SkillModelType.Unbekannt)
                        {
                            logFeed.Add(new LogEntry(LogEntryType.Error,
                                $"Vorraussetztung \"{requirement}\" für Talent \"{name}\" kann nicht gelesen werden .. wird ignoriert"));
                            continue;
                        }

                        var value = int.Parse(strings[1]);

                        requirements.Add(skill, value);
                    }

                    var difficultyValue = CleanUpString(dataCells[2].InnerText);
                    var difficulty = ParseStringToDifficultyForTalent(difficultyValue, name, logFeed, url);
                    var activeUse = MapToActiveUse(CleanUpString(dataCells[3].InnerText), logFeed);
                    var phaseValueMod = CleanUpString(dataCells[4].InnerText);
                    
                    var shortDescription = string.Empty;

                    if (dataCells.Count > 5)
                    {
                        shortDescription = CleanUpString(dataCells[5].InnerText);
                    }
                    else
                    {
                        logFeed.Add(new LogEntry(LogEntryType.Warning, $"Talent \"{name}\" hat keine Kurzbeschreibung"));
                    }

                    var desc = string.Empty;
                    if (!descriptions.ContainsKey(name))
                    {
                        logFeed.Add(new LogEntry(LogEntryType.Warning, $"Keine Beschreibung zu \"{name}\" gefunden {url}"));
                    }
                    else
                    {
                        desc = descriptions[name];
                        if (string.IsNullOrWhiteSpace(desc))
                        {
                            logFeed.Add(new LogEntry(LogEntryType.Warning, $"Nur eine leere Beschreibung zu \"{name}\" gefunden {url}"));
                        }
                    }

                    talents.Add(new TalentModel(modelType, name, shortDescription, desc, requirements, difficulty, activeUse,
                        phaseValueMod));
                }
                catch (Exception exception)
                {
                    logFeed.Add(new LogEntry(LogEntryType.Error,
                        $"Talent \"{name}\" kann nicht von \"{url}\" gelesen werden.{Environment.NewLine}Fehler:{exception}"));
                }
            }

            logFeed.Add(new LogEntry(LogEntryType.Success,
                $"Talente für Fertigkeit \"{modelType}\" hinzugefügt [{string.Join(", ", talents.Select(model => model.Name))}]"));
            return talents;
        }

        private int? ParseStringToDifficultyForTalent(string value, string talentName,
            ObservableCollection<LogEntry> logFeed, string url)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                logFeed.Add(new LogEntry(LogEntryType.Error,
                    $"Schwierigkeit \"{value}\" kann von {talentName} \"{url}\" nicht gelesen werden. Die Schwierigkeit wird als konfigurierbar hinterlegt."));
                return null;
            }

            //user can configure the value
            if (value.Equals("speziell"))
                return null;

            var parsed = int.TryParse(value, out int parsedValue);
            if (!parsed)
            {
                logFeed.Add(new LogEntry(LogEntryType.Error,
                    $"Schwierigkeit \"{value}\" kann von {talentName} \"{url}\" nicht in eine Zahl konvertiert werden. Die Schwierigkeit wird als konfigurierbar hinterlegt."));
                return null;
            }

            return parsedValue;
        }

        private SkillModelType MappingStringToSkillType(string value, ObservableCollection<LogEntry> logFeed)
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

            logFeed.Add(new LogEntry(LogEntryType.Error, $"Keine Fertigkeit für den Wert \"{value}\" hinterlegt"));
            return SkillModelType.Unbekannt;
        }

        private SkillGroupModelType MappingStringToSkillGroupType(string value, ObservableCollection<LogEntry> logFeed)
        {
            if (Enum.TryParse(value, out SkillGroupModelType castValue))
                return castValue;

            if (value.Equals("Weben"))
                return SkillGroupModelType.Webkunst;

            logFeed.Add(new LogEntry(LogEntryType.Error, $"Keine Fertigkeitskategorie für den Wert \"{value}\" hinterlegt"));
            return SkillGroupModelType.Unbekannt;
        }

        private bool MapToActiveUse(string value, ObservableCollection<LogEntry> logFeed)
        {
            if (value.Equals("passiv"))
                return false;
            if (value.Equals("aktiv"))
                return true;

            logFeed.Add(new LogEntry(LogEntryType.Error, $"Keinen Einsatz für den Wert \"{value}\" hinterlegt"));
            return false;
        }
        
        private IEnumerable<MasteryModel> ParseMasteriesFromUrls(Dictionary<SkillGroupModelType, string> urls,
            ObservableCollection<LogEntry> logFeed)
        {
            var talents = new List<MasteryModel>();
            foreach (var item in urls)
            {
                talents.AddRange(ParseMasteriesFromUrl(item.Key, item.Value, logFeed));
            }

            return talents;
        }

        private List<MasteryModel> ParseMasteriesFromUrl(SkillGroupModelType modelType, string url,
            ObservableCollection<LogEntry> logFeed)
        {
            var talents = new List<MasteryModel>();

            var doc = WikiHelper.LoadDocumentFromUrl(url, logFeed);
            if (doc == null)
                return talents;

            var descriptions = GetTalentDescriptions(doc);


            var table = doc.DocumentNode.SelectSingleNode("//table[@class='wikitable']");
            if (table == null)
            {
                logFeed.Add(new LogEntry(LogEntryType.Warning, $"Keine Tabelle mit Werten auf \"{url}\" gefunden"));
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

                    var requirements = new Dictionary<SkillGroupModelType, int>();

                    foreach (var requirement in requirementsRawValue.Split(',').Select(s => s.Trim()))
                    {
                        var strings = requirement.Split(' ');
                        var skill = MappingStringToSkillGroupType(strings[0], logFeed);
                        if (skill == SkillGroupModelType.Unbekannt)
                        {
                            logFeed.Add(new LogEntry(LogEntryType.Error,
                                $"Vorraussetztung \"{requirement}\" für Talent \"{name}\" kann nicht gelesen werden .. wird ignoriert"));
                            continue;
                        }

                        var value = int.Parse(strings[1]);

                        requirements.Add(skill, value);
                    }

                    var difficultyValue = CleanUpString(dataCells[2].InnerText);
                    var difficulty = ParseStringToDifficultyForTalent(difficultyValue, name, logFeed, url);
                    var activeUse = MapToActiveUse(CleanUpString(dataCells[3].InnerText), logFeed);
                    var phaseValueMod = CleanUpString(dataCells[4].InnerText);

                    var shortDescription = string.Empty;

                    if (dataCells.Count > 5)
                        shortDescription = CleanUpString(dataCells[5].InnerText);
                    else
                        logFeed.Add(new LogEntry(LogEntryType.Warning,
                            $"Meisterschaft \"{name}\" hat keine Kurzbeschreibung"));

                    var desc = string.Empty;
                    if (!descriptions.ContainsKey(name))
                    {
                        logFeed.Add(new LogEntry(LogEntryType.Warning, $"Keine Beschreibung zu \"{name}\" gefunden {url}"));
                    }
                    else
                    {
                        desc = descriptions[name];
                        if (string.IsNullOrWhiteSpace(desc))
                        {
                            logFeed.Add(new LogEntry(LogEntryType.Warning, $"Nur eine leere Beschreibung zu \"{name}\" gefunden {url}"));
                        }
                    }

                    talents.Add(new MasteryModel(modelType, name, shortDescription, desc, requirements, difficulty, activeUse, phaseValueMod));
                }
                catch (Exception exception)
                {
                    logFeed.Add(new LogEntry(LogEntryType.Error,
                        $"Talent \"{name}\" kann nicht von \"{url}\" gelesen werden.{Environment.NewLine}Fehler:{exception}"));
                }
            }

            logFeed.Add(new LogEntry(LogEntryType.Success,
                $"Talente für Fertigkeit \"{modelType}\" hinzugefügt [{string.Join(", ", talents.Select(model => model.Name))}]"));
            return talents;
        }

    }
}