using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace ImagoApp.Services
{
    public interface IWikiParseService
    {
        Task<int?> RefreshWikiData(Models.Enum.TableInfoType type, ObservableCollection<Util.LogEntry> logFeed);
    }

    public class WikiParseService : IWikiParseService
    {
        private readonly Repository.WrappingDatabase.IMeleeWeaponRepository _meleeWeaponRepository;
        private readonly Repository.WrappingDatabase.IRangedWeaponRepository _rangedWeaponRepository;
        private readonly Repository.WrappingDatabase.IArmorRepository _armorWeaponRepository;
        private readonly Repository.WrappingDatabase.ITalentRepository _talentRepository;
        private readonly Repository.WrappingDatabase.ISpecialWeaponRepository _specialWeaponRepository;
        private readonly Repository.WrappingDatabase.IShieldRepository _shieldRepository;
        private readonly Repository.WrappingDatabase.IMasteryRepository _masteryRepository;

        public WikiParseService(
            Repository.WrappingDatabase.IMeleeWeaponRepository meleeWeaponRepository,
            Repository.WrappingDatabase.IRangedWeaponRepository rangedWeaponRepository,
            Repository.WrappingDatabase.IArmorRepository armorWeaponRepository,
            Repository.WrappingDatabase.ITalentRepository talentRepository,
            Repository.WrappingDatabase.ISpecialWeaponRepository specialWeaponRepository,
            Repository.WrappingDatabase.IShieldRepository shieldRepository,
            Repository.WrappingDatabase.IMasteryRepository masteryRepository)
        {
            _meleeWeaponRepository = meleeWeaponRepository;
            _rangedWeaponRepository = rangedWeaponRepository;
            _armorWeaponRepository = armorWeaponRepository;
            _talentRepository = talentRepository;
            _specialWeaponRepository = specialWeaponRepository;
            _shieldRepository = shieldRepository;
            _masteryRepository = masteryRepository;
        }

        public async Task<int?> RefreshWikiData(Models.Enum.TableInfoType type, ObservableCollection<Util.LogEntry> logFeed)
        {
            switch (type)
            {
                case Models.Enum.TableInfoType.Armor:
                {
                    var armor = ParseArmorFromUrl(Util.WikiConstants.ArmorUrl, logFeed);
                    await _armorWeaponRepository.DeleteAllItems();
                    return await _armorWeaponRepository.AddAllItems(armor);
                }
                case Models.Enum.TableInfoType.MeleeWeapons:
                {
                    var weapons = ParseWeaponsFromUrl(Util.WikiConstants.MeleeWeaponUrl, logFeed);
                    await _meleeWeaponRepository.DeleteAllItems();
                    return await _meleeWeaponRepository.AddAllItems(weapons);
                }
                case Models.Enum.TableInfoType.RangedWeapons:
                {
                    var weapons = ParseWeaponsFromUrl(Util.WikiConstants.RangedWeaponUrl, logFeed);
                    await _rangedWeaponRepository.DeleteAllItems();
                    return await _rangedWeaponRepository.AddAllItems(weapons);
                }
                case Models.Enum.TableInfoType.SpecialWeapons:
                {
                    var weapons = ParseWeaponsFromUrl(Util.WikiConstants.SpecialWeaponUrl, logFeed);
                    await _specialWeaponRepository.DeleteAllItems();
                    return await _specialWeaponRepository.AddAllItems(weapons);
                }
                case Models.Enum.TableInfoType.Shields:
                {
                    var weapons = ParseWeaponsFromUrl(Util.WikiConstants.ShieldsUrl, logFeed);
                    await _shieldRepository.DeleteAllItems();
                    return await _shieldRepository.AddAllItems(weapons);
                }
                case Models.Enum.TableInfoType.Talents:
                {
                    var talents = ParseTalentsFromUrls(Util.WikiConstants.ParsableSkillTypeLookUp, logFeed);
                    await _talentRepository.DeleteAllItems();
                    return await _talentRepository.AddAllItems(talents);
                }
                case Models.Enum.TableInfoType.Masteries:
                {
                    var masteries = ParseMasteriesFromUrls(Util.WikiConstants.SkillGroupTypeLookUp, logFeed);
                    await _masteryRepository.DeleteAllItems();
                    return await _masteryRepository.AddAllItems(masteries);
                }
            }

            logFeed.Add(new Util.LogEntry(Util.LogEntryType.Error, $"Keine Parse-Funktion für \"{type}\" hinterlegt"));
            return null;
        }

        private List<Models.ArmorPartModel> ParseArmorFromUrl(string url, ObservableCollection<Util.LogEntry> logFeed)
        {
            var result = new List<Models.ArmorPartModel>();

            var doc = Util.WikiHelper.LoadDocumentFromUrl(url, logFeed);
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
                        var bodyPart = ParseBodyPart(CleanUpString(dataCells[0].InnerText), logFeed);

                        var physical = CleanUpString(dataCells[1].InnerText);
                        var energy = CleanUpString(dataCells[2].InnerText);
                        var load = CleanUpString(dataCells[3].InnerText);
                        var durability = CleanUpString(dataCells[4].InnerText);

                        var armor = new Models.ArmorPartModel(bodyPart, armorName, int.Parse(load), false, false,
                            int.Parse(durability), int.Parse(energy), int.Parse(physical));
                        result.Add(armor);
                    }
                }
                catch (Exception e)
                {
                    logFeed.Add(new Util.LogEntry(Util.LogEntryType.Error,
                        $"Werte für Rüstung \"{armorName}\" konnten nicht gelesen werden.{Environment.NewLine}Fehler:{e}"));
                }
            }

            logFeed.Add(new Util.LogEntry(Util.LogEntryType.Success,
                $"Rüstungen hinzugefügt [{string.Join(", ", result.Select(set => set.Name))}]"));
            return result;
        }

        private Models.Enum.ArmorPartType ParseBodyPart(string name, ObservableCollection<Util.LogEntry> logFeed)
        {
            if (name.Equals("Helm"))
                return Models.Enum.ArmorPartType.Helm;
            if (name.Equals("Torso"))
                return Models.Enum.ArmorPartType.Torso;
            if (name.Equals("Arm"))
                return Models.Enum.ArmorPartType.Arm;
            if (name.Equals("Bein"))
                return Models.Enum.ArmorPartType.Bein;

            logFeed.Add(
                new Util.LogEntry(Util.LogEntryType.Error,
                    $"Zuordnung von Rüstungsteil konnte aus Wert \"{name}\" nicht gelesen werden"));
            return Models.Enum.ArmorPartType.Unknown;
        }
        
        private List<Models.Weapon> ParseWeaponsFromUrl(string url, ObservableCollection<Util.LogEntry> logFeed)
        {
            var result = new List<Models.Weapon>();
            var doc = Util.WikiHelper.LoadDocumentFromUrl(url, logFeed);
            if (doc == null)
                return result;



                //parse complete table
            foreach (var table in doc.DocumentNode.SelectNodes("//table[@class='wikitable']"))
            {
                var weaponName = string.Empty;
                try
                {
                    var weaponStances = new List<Models.WeaponStance>();

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
                        
                        weaponStances.Add(new Models.WeaponStance(weaponStanceType, phase, damage, parry, range));
                    }

                    result.Add(new Models.Weapon(weaponName, weaponStances, false, false, int.Parse(loadValue), int.Parse(durabilityValue)));
                }
                catch (Exception e)
                {
                    logFeed.Add(new Util.LogEntry(Util.LogEntryType.Error,
                        $"Werte für die Waffe \"{weaponName}\" konnten nicht von \"{url}\" gelesen werden.{Environment.NewLine}Fehler:{e}"));
                }
            }

            logFeed.Add(new Util.LogEntry(Util.LogEntryType.Success,
                $"Waffen hinzugefügt [{string.Join(", ", result.Select(weapon => weapon.Name))}]"));
            return result;
        }
        
        private string CleanUpString(string value)
        {
            return value.Replace("\n", "").Replace("\r", "").Trim();
        }

        private IEnumerable<Models.TalentModel> ParseTalentsFromUrls(Dictionary<Models.SkillModelType, string> urls,
            ObservableCollection<Util.LogEntry> logFeed)
        {
            var talents = new List<Models.TalentModel>();
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

        private List<Models.TalentModel> ParseTalentsFromUrl(Models.SkillModelType modelType, string url,
            ObservableCollection<Util.LogEntry> logFeed)
        {
            var talents = new List<Models.TalentModel>();
            var doc = Util.WikiHelper.LoadDocumentFromUrl(url, logFeed);
            if (doc == null)
                return talents;

            var descriptions =  GetTalentDescriptions(doc);

            var table = doc.DocumentNode.SelectSingleNode("//table[@class='wikitable']");
            if (table == null)
            {
                logFeed.Add(new Util.LogEntry(Util.LogEntryType.Warning, $"Keine Tabelle mit Werten auf \"{url}\" gefunden"));
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

                    var requirements = new Dictionary<Models.SkillModelType, int>();

                    foreach (var requirement in requirementsRawValue.Split(',').Select(s => s.Trim()))
                    {
                        var strings = requirement.Split(' ');
                        var skill = MappingStringToSkillType(strings[0], logFeed);
                        if (skill == Models.SkillModelType.Unbekannt)
                        {
                            logFeed.Add(new Util.LogEntry(Util.LogEntryType.Error,
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
                        logFeed.Add(new Util.LogEntry(Util.LogEntryType.Warning, $"Talent \"{name}\" hat keine Kurzbeschreibung"));
                    }

                    var desc = string.Empty;
                    if (!descriptions.ContainsKey(name))
                    {
                        logFeed.Add(new Util.LogEntry(Util.LogEntryType.Warning, $"Keine Beschreibung zu \"{name}\" gefunden {url}"));
                    }
                    else
                    {
                        desc = descriptions[name];
                        if (string.IsNullOrWhiteSpace(desc))
                        {
                            logFeed.Add(new Util.LogEntry(Util.LogEntryType.Warning, $"Nur eine leere Beschreibung zu \"{name}\" gefunden {url}"));
                        }
                    }

                    talents.Add(new Models.TalentModel(modelType, name, shortDescription, desc, requirements, difficulty, activeUse,
                        phaseValueMod));
                }
                catch (Exception exception)
                {
                    logFeed.Add(new Util.LogEntry(Util.LogEntryType.Error,
                        $"Talent \"{name}\" kann nicht von \"{url}\" gelesen werden.{Environment.NewLine}Fehler:{exception}"));
                }
            }

            logFeed.Add(new Util.LogEntry(Util.LogEntryType.Success,
                $"Talente für Fertigkeit \"{modelType}\" hinzugefügt [{string.Join(", ", talents.Select(model => model.Name))}]"));
            return talents;
        }

        private int? ParseStringToDifficultyForTalent(string value, string talentName,
            ObservableCollection<Util.LogEntry> logFeed, string url)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                logFeed.Add(new Util.LogEntry(Util.LogEntryType.Error,
                    $"Schwierigkeit \"{value}\" kann von {talentName} \"{url}\" nicht gelesen werden. Die Schwierigkeit wird als konfigurierbar hinterlegt."));
                return null;
            }

            //user can configure the value
            if (value.Equals("speziell"))
                return null;

            var parsed = int.TryParse(value, out int parsedValue);
            if (!parsed)
            {
                logFeed.Add(new Util.LogEntry(Util.LogEntryType.Error,
                    $"Schwierigkeit \"{value}\" kann von {talentName} \"{url}\" nicht in eine Zahl konvertiert werden. Die Schwierigkeit wird als konfigurierbar hinterlegt."));
                return null;
            }

            return parsedValue;
        }

        private Models.SkillModelType MappingStringToSkillType(string value, ObservableCollection<Util.LogEntry> logFeed)
        {
            if (Enum.TryParse(value, out Models.SkillModelType castValue))
                return castValue;

            if (value.Equals("Armbrüste"))
                return Models.SkillModelType.Armbrueste;

            if (value.Equals("Körperbeherrschung"))
                return Models.SkillModelType.Koerperbeherrschung;

            if (value.Equals("Bögen"))
                return Models.SkillModelType.Boegen;

            if (value.Equals("Stäbe"))
                return Models.SkillModelType.StaebeSpeere;

            if (value.Equals("Zweihänder"))
                return Models.SkillModelType.Zweihaender;

            logFeed.Add(new Util.LogEntry(Util.LogEntryType.Error, $"Keine Fertigkeit für den Wert \"{value}\" hinterlegt"));
            return Models.SkillModelType.Unbekannt;
        }

        private Models.Enum.SkillGroupModelType MappingStringToSkillGroupType(string value, ObservableCollection<Util.LogEntry> logFeed)
        {
            if (Enum.TryParse(value, out Models.Enum.SkillGroupModelType castValue))
                return castValue;

            if (value.Equals("Weben"))
                return Models.Enum.SkillGroupModelType.Webkunst;

            logFeed.Add(new Util.LogEntry(Util.LogEntryType.Error, $"Keine Fertigkeitskategorie für den Wert \"{value}\" hinterlegt"));
            return Models.Enum.SkillGroupModelType.Unbekannt;
        }

        private bool MapToActiveUse(string value, ObservableCollection<Util.LogEntry> logFeed)
        {
            if (value.Equals("passiv"))
                return false;
            if (value.Equals("aktiv"))
                return true;

            logFeed.Add(new Util.LogEntry(Util.LogEntryType.Error, $"Keinen Einsatz für den Wert \"{value}\" hinterlegt"));
            return false;
        }
        
        private IEnumerable<Models.MasteryModel> ParseMasteriesFromUrls(Dictionary<Models.Enum.SkillGroupModelType, string> urls,
            ObservableCollection<Util.LogEntry> logFeed)
        {
            var talents = new List<Models.MasteryModel>();
            foreach (var item in urls)
            {
                talents.AddRange(ParseMasteriesFromUrl(item.Key, item.Value, logFeed));
            }

            return talents;
        }

        private List<Models.MasteryModel> ParseMasteriesFromUrl(Models.Enum.SkillGroupModelType modelType, string url,
            ObservableCollection<Util.LogEntry> logFeed)
        {
            var talents = new List<Models.MasteryModel>();

            var doc = Util.WikiHelper.LoadDocumentFromUrl(url, logFeed);
            if (doc == null)
                return talents;

            var descriptions = GetTalentDescriptions(doc);


            var table = doc.DocumentNode.SelectSingleNode("//table[@class='wikitable']");
            if (table == null)
            {
                logFeed.Add(new Util.LogEntry(Util.LogEntryType.Warning, $"Keine Tabelle mit Werten auf \"{url}\" gefunden"));
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

                    var requirements = new Dictionary<Models.Enum.SkillGroupModelType, int>();

                    foreach (var requirement in requirementsRawValue.Split(',').Select(s => s.Trim()))
                    {
                        var strings = requirement.Split(' ');
                        var skill = MappingStringToSkillGroupType(strings[0], logFeed);
                        if (skill == Models.Enum.SkillGroupModelType.Unbekannt)
                        {
                            logFeed.Add(new Util.LogEntry(Util.LogEntryType.Error,
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
                        logFeed.Add(new Util.LogEntry(Util.LogEntryType.Warning,
                            $"Meisterschaft \"{name}\" hat keine Kurzbeschreibung"));

                    var desc = string.Empty;
                    if (!descriptions.ContainsKey(name))
                    {
                        logFeed.Add(new Util.LogEntry(Util.LogEntryType.Warning, $"Keine Beschreibung zu \"{name}\" gefunden {url}"));
                    }
                    else
                    {
                        desc = descriptions[name];
                        if (string.IsNullOrWhiteSpace(desc))
                        {
                            logFeed.Add(new Util.LogEntry(Util.LogEntryType.Warning, $"Nur eine leere Beschreibung zu \"{name}\" gefunden {url}"));
                        }
                    }

                    talents.Add(new Models.MasteryModel(modelType, name, shortDescription, desc, requirements, difficulty, activeUse, phaseValueMod));
                }
                catch (Exception exception)
                {
                    logFeed.Add(new Util.LogEntry(Util.LogEntryType.Error,
                        $"Talent \"{name}\" kann nicht von \"{url}\" gelesen werden.{Environment.NewLine}Fehler:{exception}"));
                }
            }

            logFeed.Add(new Util.LogEntry(Util.LogEntryType.Success,
                $"Talente für Fertigkeit \"{modelType}\" hinzugefügt [{string.Join(", ", talents.Select(model => model.Name))}]"));
            return talents;
        }

    }
}