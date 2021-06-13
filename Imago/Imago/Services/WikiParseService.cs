using HtmlAgilityPack;
using Imago.Models;
using Imago.Models.Enum;
using Imago.Repository.WrappingDatabase;
using Imago.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public WikiParseService(
            IMeleeWeaponRepository meleeWeaponRepository,
            IRangedWeaponRepository rangedWeaponRepository,
            IArmorRepository armorWeaponRepository,
            ITalentRepository talentRepository)
        {
            _meleeWeaponRepository = meleeWeaponRepository;
            _rangedWeaponRepository = rangedWeaponRepository;
            _armorWeaponRepository = armorWeaponRepository;
            _talentRepository = talentRepository;
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
                    var meleeWeapons = ParseMeleeWeaponsFromUrl(WikiConstants.MeleeWeaponUrl, logFeed);
                    await _meleeWeaponRepository.DeleteAllItems();
                    return await _meleeWeaponRepository.AddAllItems(meleeWeapons);
                }
                case TableInfoType.RangedWeapons:
                {
                    var meleeWeapons = ParseRangedWeaponsFromUrl(WikiConstants.RangedWeaponUrl, logFeed);
                    await _rangedWeaponRepository.DeleteAllItems();
                    return await _rangedWeaponRepository.AddAllItems(meleeWeapons);
                }
                case TableInfoType.Talents:
                {
                    var talents = ParseTalentsFromUrls(WikiConstants.SkillTypeLookUp, logFeed);
                    await _talentRepository.DeleteAllItems();
                    return await _talentRepository.AddAllItems(talents);
                }
            }

            logFeed.Add(new LogEntry(LogEntryType.Error, $"Unknown TableInfoType \"{type}\" for RefreshWikiData"));
            return null;
        }
        
        private List<ArmorSet> ParseArmorFromUrl(string url, ObservableCollection<LogEntry> logFeed)
        {
            var doc = LoadDocumentFromUrl(url);
            if (doc == null)
            {
                logFeed.Add(new LogEntry(LogEntryType.Error, $"Page not found \"{url}\" .. skipping"));
                return new List<ArmorSet>();
            }

            var result = new List<ArmorSet>();

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
                    var bodyPart = ParseBodyPart(CleanUpString(dataCells[0].InnerText), logFeed);

                    var physical = CleanUpString(dataCells[1].InnerText);
                    var energy = CleanUpString(dataCells[2].InnerText);
                    var load = CleanUpString(dataCells[3].InnerText);
                    var durability = CleanUpString(dataCells[4].InnerText);

                    var armor = new ArmorModel(armorName, int.Parse(physical), int.Parse(energy),
                        int.Parse(load), int.Parse(durability));

                    armorParts.Add(bodyPart, armor);
                }

                
                result.Add(new ArmorSet(armorParts));
            }

            logFeed.Add(new LogEntry(LogEntryType.Success, 
                $"Armor added [{string.Join(", ", result.Select(set => set.ArmorParts.First().Value.Name))}]"));
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
                new LogEntry(LogEntryType.Error, $"Unable to parse {nameof(ArmorPartType)} by value \"{name}\""));
            return ArmorPartType.Unknown;
        }

        private List<Weapon> ParseMeleeWeaponsFromUrl(string url, ObservableCollection<LogEntry> logFeed)
        {
            var doc = LoadDocumentFromUrl(url);
            if (doc == null)
            {
                logFeed.Add(new LogEntry(LogEntryType.Error, $"Page not found \"{url}\" .. skipping"));
                return new List<Weapon>();
            }

            var result = new List<Weapon>();

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
                    var weaponStanceType = ParseWeaponStance(CleanUpString(dataCells[0].InnerText), logFeed);

                    var phase = CleanUpString(dataCells[1].InnerText);
                    var damage = CleanUpString(dataCells[2].InnerText);
                    var parry = CleanUpString(dataCells[3].InnerText);

                    var weaponStance = new WeaponStance(weaponStanceType, phase, damage, int.Parse(parry), "nah");
                    weaponStances.Add(weaponStanceType, weaponStance);
                }

                result.Add(new Weapon(weaponName, weaponStances, int.Parse(loadValue), int.Parse(durabilityValue)));
            }

            logFeed.Add(new LogEntry(LogEntryType.Success,
                $"Melee weapons added [{string.Join(", ", result.Select(weapon => weapon.Name))}]"));
            return result;
        }

        private List<Weapon> ParseRangedWeaponsFromUrl(string url, ObservableCollection<LogEntry> logFeed)
        {
            var doc = LoadDocumentFromUrl(url);
            if (doc == null)
            {
                logFeed.Add(new LogEntry(LogEntryType.Error, $"Page not found \"{url}\" .. skipping"));
                return new List<Weapon>();
            }

            var result = new List<Weapon>();

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
                    var weaponStanceType = ParseWeaponStance(CleanUpString(dataCells[0].InnerText), logFeed);

                    var phase = CleanUpString(dataCells[1].InnerText);
                    var damage = CleanUpString(dataCells[2].InnerText);
                    var range = CleanUpString(dataCells[3].InnerText);

                    var weaponStance = new WeaponStance(weaponStanceType, phase, damage, null, range);
                    weaponStances.Add(weaponStanceType, weaponStance);
                }
                
                result.Add(new Weapon(weaponName, weaponStances, int.Parse(loadValue), int.Parse(durabilityValue)));
            }

            logFeed.Add(new LogEntry(LogEntryType.Success,
                $"Ranged weapons added [{string.Join(", ", result.Select(weapon => weapon.Name))}]"));
            return result;
        }

        private WeaponStanceType ParseWeaponStance(string name, ObservableCollection<LogEntry> logFeed)
        {
            if (name.Equals("leichte Haltung"))
                return WeaponStanceType.Light;
            if (name.Equals("schwere Haltung"))
                return WeaponStanceType.Heavy;

            logFeed.Add(new LogEntry(LogEntryType.Error,
                $"Unable to parse {nameof(WeaponStanceType)} by value \"{name}\""));
            return WeaponStanceType.Unknown;
        }

        private string CleanUpString(string value)
        {
            return value.Replace("\n", "").Replace("\r", "");
        }

        private List<SkillType> SpecialSkillTypeParseFilter = new List<SkillType>()
        {
            SkillType.Sprache,
            SkillType.Bewusstsein,
            SkillType.Chaos,
            SkillType.Einfalt,
            SkillType.Struktur,
            SkillType.Leere,
            SkillType.Materie,
            SkillType.Ekstase,
            SkillType.Kontrolle
        };

        private IEnumerable<TalentModel> ParseTalentsFromUrls(Dictionary<SkillType, string> urls, ObservableCollection<LogEntry> logFeed)
        {
            var talents = new List<TalentModel>();
            foreach (var item in urls)
            {
                if (SpecialSkillTypeParseFilter.Contains(item.Key))
                {
                    logFeed.Add(new LogEntry(LogEntryType.Info, $"Talentpage {item.Key} per definition skipped"));
                    continue;
                }
                
                talents.AddRange(ParseTalentsFromUrl(item.Key, item.Value, logFeed));
            }
            return talents;
        }

        private List<TalentModel> ParseTalentsFromUrl(SkillType type, string url, ObservableCollection<LogEntry> logFeed)
        {
            var doc = LoadDocumentFromUrl(url);
            if (doc == null)
            {
                logFeed.Add(new LogEntry(LogEntryType.Error, $"Page not found \"{url}\" .. skipping"));
                return new List<TalentModel>();
            }
            
            var table = doc.DocumentNode.SelectSingleNode("//table[@class='wikitable']");
            if (table == null)
            {
                logFeed.Add(new LogEntry(LogEntryType.Warning, $"No table found at \"{url}\" .. skipping"));
                return new List<TalentModel>();
            }

            var talents = new List<TalentModel>();

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

                    var requirements = new Dictionary<SkillType, int>();

                    foreach (var requirement in requirementsRawValue.Split(',').Select(s => s.Trim()))
                    {
                        var strings = requirement.Split(' ');
                        var skill = MappingStringToSkillType(strings[0], logFeed);
                        if (skill == SkillType.Unbekannt)
                        {
                            logFeed.Add(new LogEntry(LogEntryType.Warning,
                                $"Unable to map requirement \"{requirement}\" for \"{name}\" .. skipping requirement"));
                            continue;
                        }

                        var value = int.Parse(strings[1]);

                        requirements.Add(skill, value);
                    }

                    var difficultyRaw = CleanUpString(dataCells[2].InnerText);
                    string parsedDifficulty = difficultyRaw;

                    if (string.IsNullOrWhiteSpace(parsedDifficulty))
                    {
                        logFeed.Add(new LogEntry(LogEntryType.Error, $"Unable to parse Difficulty \"{difficultyRaw}\" from \"{url}\""));
                        continue;
                    }

                    //todo user can set value himself
                    if (parsedDifficulty.Equals("speziell"))
                        parsedDifficulty = null;

                    var difficultyParsed = int.TryParse(parsedDifficulty, out int e);

                    var activeUse = MapToActiveUse(CleanUpString(dataCells[3].InnerText), logFeed);
                    var phaseValueMod = CleanUpString(dataCells[4].InnerText);
                    
                    if (difficultyParsed)
                        talents.Add(new TalentModel(name, requirements, e, activeUse, phaseValueMod));
                    else
                    {
                        logFeed.Add(new LogEntry(LogEntryType.Warning,
                            $"Unable to parse difficulty \"{difficultyRaw}\" for \"{name}\" at \"{url}\" .. using default"));
                        talents.Add(new TalentModel(name, requirements, null, activeUse, phaseValueMod));
                    }
                }
                catch (Exception exception)
                {
                    logFeed.Add(new LogEntry(LogEntryType.Error,
                        $"Unable to parse Talent \"{name}\" from \"{url}\".{Environment.NewLine}{exception}"));
                }
            }

            logFeed.Add(new LogEntry(LogEntryType.Success,
                $"Talents for \"{type}\" added [{string.Join(", ", talents.Select(model => model.Name))}]"));
            return talents;
        }

        private SkillType MappingStringToSkillType(string value, ObservableCollection<LogEntry> logFeed)
        {
            if (Enum.TryParse(value, out SkillType castValue))
                return castValue;

            if (value.Equals("Armbrüste"))
                return SkillType.Armbrueste;

            if (value.Equals("Körperbeherrschung"))
                return SkillType.Koerperbeherrschung;

            if (value.Equals("Bögen"))
                return SkillType.Boegen;

            if (value.Equals("Stäbe"))
                return SkillType.StaebeSpeere;

            if (value.Equals("Zweihänder"))
                return SkillType.Zweihaender;

            logFeed.Add(new LogEntry(LogEntryType.Error, $"Unable to parse String \"{value}\" to SkillType"));
            return SkillType.Unbekannt;
        }

        private bool MapToActiveUse(string value, ObservableCollection<LogEntry> logFeed)
        {
            if (value.Equals("passiv"))
                return false;
            if (value.Equals("aktiv"))
                return true;

            logFeed.Add(new LogEntry(LogEntryType.Error, $"Unable to map \"{value}\" in MapToActiveUse"));
            return false;
        }

        private HtmlDocument LoadDocumentFromUrl(string url)
        {
            var htmlWeb = new HtmlWeb();
           
            var doc = htmlWeb.Load(url);

            
            if (htmlWeb.StatusCode == HttpStatusCode.NotFound)
                return null;
            return doc;
        }
    }
}