using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using Autofac;
using ImagoApp.Application;
using ImagoApp.Application.Models;
using ImagoApp.Application.Services;
using ImagoApp.Infrastructure.Database;
using ImagoApp.Infrastructure.Entities;
using ImagoApp.Infrastructure.Repositories;
using ImagoApp.Manager;
using ImagoApp.Shared;
using ImagoApp.Styles.Themes;
using ImagoApp.Util;
using ImagoApp.ViewModels.Page;
using ImagoApp.Views;
using ImagoApp.Views.CustomControls;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ImagoApp.ViewModels
{
    public class StartPageViewModel : BindableBase
    {
        private readonly ICharacterService _characterService;
        private readonly IWikiParseService _wikiParseService;
        private readonly IWikiDataService _wikiDataService;
        private readonly ICharacterCreationService _characterCreationService;
        private readonly IFileRepository _fileRepository;
        private readonly ILocalFileService _localfileService;
        private readonly ICharacterProvider _characterProvider;
        private readonly IAttributeCalculationService _attributeCalculationService;
        private readonly IWikiService _wikiService;
        private readonly ICharacterDatabaseConnection _characterDatabaseConnection;
        private readonly string _logFileName = "wiki_parse.log";

        public ObservableCollection<CharacterPreview> Characters { get; private set; }

        public CharacterPreview SelectedCharacter
        {
            get => _selectedCharacter;
            set
            {
                SetProperty(ref _selectedCharacter, value);
            }
        }

        public string Version
        {
            get => _version;
            set => SetProperty(ref _version, value);
        }

        public DatabaseInfoViewModel DatabaseInfoViewModel { get; set; }

        public StartPageViewModel(ICharacterService characterService,
            IWikiParseService wikiParseService,
            IWikiDataService wikiDataService,
            ICharacterCreationService characterCreationService,
            IFileRepository fileRepository,
            ILocalFileService localfileService,
            ICharacterProvider characterProvider,
            IAttributeCalculationService attributeCalculationService,
            IWikiService wikiService,
            ICharacterDatabaseConnection characterDatabaseConnection)
        {
            VersionTracking.Track();
            Version = new Version(VersionTracking.CurrentVersion).ToString(3);
            DatabaseInfoViewModel = new DatabaseInfoViewModel();
            Characters = new ObservableCollection<CharacterPreview>();

            _characterService = characterService;
            _wikiParseService = wikiParseService;
            _wikiDataService = wikiDataService;
            _characterCreationService = characterCreationService;
            _fileRepository = fileRepository;
            _localfileService = localfileService;
            _characterProvider = characterProvider;
            _attributeCalculationService = attributeCalculationService;
            _wikiService = wikiService;
            _characterDatabaseConnection = characterDatabaseConnection;

            EnsurePreferences();

            Task.Run(() =>
            {
                RefreshData(true, true, false);
                CheckWikiData();

                if (VersionTracking.IsFirstLaunchForCurrentBuild || VersionTracking.IsFirstLaunchForCurrentVersion)
                    AlertNewVersion();
            });
        }

        private ICommand _refreshCharactersCommand;

        public ICommand RefreshCharactersCommand => _refreshCharactersCommand ?? (_refreshCharactersCommand = new Command(() =>
        {
            RefreshData(false, true, false);
        }));

        private const string FontSizePreferenceKey = "FonzScale";
        private void EnsurePreferences()
        {
            if (!Preferences.ContainsKey(FontSizePreferenceKey))
            {
                Preferences.Set(FontSizePreferenceKey, 100);
            }
            else
            {
                //apply setting
                var diff = Preferences.Get(FontSizePreferenceKey, 100) - 100;
                var t = diff / 10;
                StyleResourceManager.ChangeGlobalFontSize(t);
            }
        }
    
        private void AlertNewVersion()
        {
            UserDialogs.Instance.Confirm(new ConfirmConfig
            {
                Message = "Die neue Version wurde erfolgreich Heruntergeladen und installiert",
                Title = $"Neue Version {Version}",
                CancelText = "OK",
                OkText = "Changelog öffnen",
                OnAction = result =>
                {
                    if (result)
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            OpenChangeLogCommand?.Execute(null);
                        });
                    }
                }
            });
        }

        public string FontScale => $"{Preferences.Get(FontSizePreferenceKey, 100)}%";

        private ICommand _increaseFontSizeCommand;
        public ICommand IncreaseFontSizeCommand => _increaseFontSizeCommand ?? (_increaseFontSizeCommand = new Command(() =>
        {
            Analytics.TrackEvent("Increase font size");
            var value = Preferences.Get(FontSizePreferenceKey, 100);
            Preferences.Set(FontSizePreferenceKey, value + 10);
            StyleResourceManager.ChangeGlobalFontSize(1);
            OnPropertyChanged(nameof(FontScale));
        }));

        private ICommand _decreaseFontSizeCommand;
        public ICommand DecreaseFontSizeCommand => _decreaseFontSizeCommand ?? (_decreaseFontSizeCommand = new Command(() =>
        {
            Analytics.TrackEvent("Decrease font size");
            var value = Preferences.Get(FontSizePreferenceKey, 100);
            Preferences.Set(FontSizePreferenceKey, value - 10);
            StyleResourceManager.ChangeGlobalFontSize(-1);
            OnPropertyChanged(nameof(FontScale));
        }));

        private ICommand _openImportantNotesCommand;
        public ICommand OpenImportantNotesCommand => _openImportantNotesCommand ?? (_openImportantNotesCommand = new Command(() =>
        {
            try
            {
                Analytics.TrackEvent("Open important notes");
                Launcher.OpenAsync(WikiConstants.ImportantNotesUrl);
            }
            catch (Exception exception)
            {
                App.ErrorManager.TrackException(exception);
            }
        }));

        private ICommand _openRoadmapCommand;
        public ICommand OpenRoadmapCommand => _openRoadmapCommand ?? (_openRoadmapCommand = new Command(() =>
        {
            try
            {
                Analytics.TrackEvent("Open roadmap");
                Launcher.OpenAsync(WikiConstants.RoadmapUrl);
            }
            catch (Exception exception)
            {
                App.ErrorManager.TrackException(exception);
            }
        }));

        private ICommand _openAppDataFolderCommand;
        public ICommand OpenAppDataFolderCommand => _openAppDataFolderCommand ?? (_openAppDataFolderCommand = new Command(() =>
        {
            try
            {
                Analytics.TrackEvent("Open appdata folder");
                _localfileService.OpenFolder(_fileRepository.GetApplicationFolder());
            }
            catch (Exception exception)
            {
                App.ErrorManager.TrackException(exception);
            }
        }));

        private void CheckWikiData()
        {
            var count =
                _wikiDataService.GetArmorWikiDataItemCount() +
                _wikiDataService.GetWeaponWikiDataItemCount() +
                _wikiDataService.GetTalentWikiDataItemCount() +
                _wikiDataService.GetMasteryWikiDataItemCount();

            var dbInfo = _wikiDataService.GetDatabaseInfo();

            if (count == 0 || dbInfo.LastWriteTime < DateTime.Now.AddDays(-30))
            {
                //wikidata is empty or older than 30 days
                Device.BeginInvokeOnMainThread(() =>
                {
                    ParseWikiCommand?.Execute(null);
                });
            }
        }

        private class CollectionSink : ILogEventSink
        {
            public ICollection<LogEvent> Events { get; } = new List<LogEvent>();

            public void Emit(LogEvent le)
            {
                if (le.Level >= LogEventLevel.Error)
                    Crashes.TrackError(le.Exception);

                Events.Add(le);
            }
        }

        private ICommand _openChangeLogCommand;

        public ICommand OpenChangeLogCommand => _openChangeLogCommand ?? (_openChangeLogCommand = new Command(() =>
        {
            try
            {
                Analytics.TrackEvent("Open changelog");
                Launcher.OpenAsync(WikiConstants.ChangelogUrl);
            }
            catch (Exception exception)
            {
                App.ErrorManager.TrackException(exception);
            }
        }));

        private ICommand _parseWikiCommand;

        public ICommand ParseWikiCommand => _parseWikiCommand ?? (_parseWikiCommand = new Command(async () =>
        {
            try
            {
                Analytics.TrackEvent("Parse wiki data");
                var logFile = Path.Combine(_fileRepository.GetApplicationFolder(), _logFileName);
                if (File.Exists(logFile))
                    File.Delete(logFile);

                var logEventSink = new CollectionSink();

                using (var logger = new LoggerConfiguration().WriteTo.Debug()
                    .WriteTo.File(logFile)
                    .WriteTo.Sink(logEventSink)
                    .CreateLogger())
                {
                    using (var progressDialog = UserDialogs.Instance.Progress(""))
                    {
                        var parseDialogTitlePrefix = "Daten werden aus dem Wiki geladen.." + Environment.NewLine + Environment.NewLine;

                        progressDialog.Title = parseDialogTitlePrefix + "Rüstungen werden geladen";
                        await Task.Delay(500);

                        var armorCount = _wikiParseService.RefreshArmorFromWiki(logger);
                        progressDialog.Title = parseDialogTitlePrefix + "Waffen werden geladen";
                        progressDialog.PercentComplete = 20;
                        await Task.Delay(50);

                        var weaponCount = _wikiParseService.RefreshWeaponsFromWiki(logger);
                        progressDialog.Title = parseDialogTitlePrefix + "Künste werden geladen";
                        progressDialog.PercentComplete = 40;
                        await Task.Delay(50);

                        var weaveTalentCount = _wikiParseService.RefreshWeaveTalentsFromWiki(logger);
                        progressDialog.Title = parseDialogTitlePrefix + "Webkünste werden geladen";
                        progressDialog.PercentComplete = 60;
                        await Task.Delay(50);

                        var talentCount = _wikiParseService.RefreshTalentsFromWiki(logger);
                        progressDialog.Title = parseDialogTitlePrefix + "Meisterschaften werden geladen";
                        progressDialog.PercentComplete = 80;
                        await Task.Delay(50);

                        var masteryCount = _wikiParseService.RefreshMasteriesFromWiki(logger);
                        progressDialog.Title = parseDialogTitlePrefix + "Wird abgeschlossen";
                        progressDialog.PercentComplete = 100;

                        var msg = $"{Environment.NewLine}Status:" +
                                  $"{Environment.NewLine}      Warnungen: {logEventSink.Events.Count(_ => _.Level == LogEventLevel.Warning)}" +
                                  $"{Environment.NewLine}      Fehler: {logEventSink.Events.Count(_ => _.Level == LogEventLevel.Error)}" +
                                  $"{Environment.NewLine}      Kritisch: {logEventSink.Events.Count(_ => _.Level == LogEventLevel.Fatal)}" +
                                  $"{Environment.NewLine}" +
                                  $"{Environment.NewLine}" +
                                  $"Gefundene Daten:" +
                                  $"{Environment.NewLine}      Rüstungen: {armorCount?.ToString()}" +
                                  $"{Environment.NewLine}      Waffen: {weaponCount?.ToString()}" +
                                  $"{Environment.NewLine}      Künste: {talentCount?.ToString()}" +
                                  $"{Environment.NewLine}      Webkünste: {weaveTalentCount?.ToString()}" +
                                  $"{Environment.NewLine}      Meisterschaften: {masteryCount?.ToString()}";

                        UserDialogs.Instance.Confirm(new ConfirmConfig
                        {
                            Message = msg,
                            Title = "Daten wurden aus dem Wiki geladen",
                            OkText = "OK",
                            CancelText = "Logdatei öffnen",
                            OnAction = result =>
                            {
                                if (!result)
                                {
                                    var target = new ReadOnlyFile(Path.Combine(_fileRepository.GetApplicationFolder(), _logFileName));
                                    var request = new OpenFileRequest { File = target };
                                    Launcher.OpenAsync(request);
                                }
                            }
                        });
                    }
                }

                RefreshData(true, false, false);
            }
            catch (Exception exception)
            {
                App.ErrorManager.TrackException(exception);
            }
        }));

        public CharacterModel GetCharacter(CharacterPreview characterPreview)
        {
            return _characterService.GetItem(characterPreview.Guid);
        }

        public async Task OpenCharacter(CharacterModel characterModel, bool editMode)
        {
            _characterProvider.SetCurrentCharacter(characterModel, editMode);
            var characterViewModel = App.Container.Resolve<CharacterViewModel>();

            try
            {
                //create all required dependencies
                var characterInfoPageViewModel = new CharacterInfoPageViewModel(characterViewModel);
                var wikiPageViewModel = new WikiPageViewModel(characterViewModel);
                var skillPageViewModel = new SkillPageViewModel(characterViewModel, _wikiService, _wikiDataService);
                var equipmentPageViewModel = new EquipmentPageViewModel(characterViewModel, _wikiDataService);
                var dicePageViewModel = new DicePageViewModel(characterViewModel, _wikiService, _wikiDataService);
                var appShellViewModel = new AppShellViewModel(characterViewModel, characterInfoPageViewModel, skillPageViewModel,
                    equipmentPageViewModel, wikiPageViewModel, dicePageViewModel, _characterProvider);

                //notify the main menu that editmode may have changed
                appShellViewModel.RaiseEditModeChanged();

                equipmentPageViewModel.OpenWikiPageRequested += (sender, url) => OpenWikiPage(url);
                skillPageViewModel.OpenWikiPageRequested += (sender, url) => OpenWikiPage(url);
                skillPageViewModel.DiceRollRequested += (sender, value) => OpenDicePage(value.type, value.value);
               
                void OpenWikiPage(string url)
                {
                    Analytics.TrackEvent("Open WikiPage", new Dictionary<string, string>()
                    {
                        {"url", url}
                    });

                    appShellViewModel.RaiseSwitchPageRequested(typeof(WikiPage));
                    wikiPageViewModel.OpenWikiPage(url);
                }

                void OpenDicePage(DiceSearchModelType type,object value)
                {
                    Analytics.TrackEvent("Open DiceRoll", new Dictionary<string, string>()
                    {
                        {"type", value.ToString()}
                    });

                    appShellViewModel.RaiseSwitchPageRequested(typeof(DicePage));
                    dicePageViewModel.SetSelection(type,value);
                }

                _attributeCalculationService.RecalculateAllAttributes(characterModel.Attributes, characterModel.SkillGroups);
                characterViewModel.CalculateInitialValues();

                var appShell = new AppShell(appShellViewModel);

                await Device.InvokeOnMainThreadAsync(() =>
                {
                    App.StartPage = (StartPage)Xamarin.Forms.Application.Current.MainPage;
                    Xamarin.Forms.Application.Current.MainPage = appShell;
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private ICommand _createNewCharacterCommand;

        public ICommand CreateNewCharacterCommand => _createNewCharacterCommand ?? (_createNewCharacterCommand =
            new Command(() =>
            {
                Task.Run(async () =>
                {
                    Analytics.TrackEvent("Create new character");

                    try
                    {
                        int? attributePoints = null;
                        int? skillPoints = null;

                        await Device.InvokeOnMainThreadAsync(async () =>
                        {
                            var attributePromptResult = await UserDialogs.Instance.PromptAsync(new PromptConfig
                            {
                                Title = "Charaktererschaffung - Erfahrungspunkte für Attribute",
                                Text = "940",
                                Message = "(Dieser Wert kann später noch geändert werden)",
                                OkText = "OK",
                                CancelText = "Abbrechen",
                                InputType = InputType.Number
                            });

                            if (!attributePromptResult.Ok || string.IsNullOrWhiteSpace(attributePromptResult.Value))
                                return;

                            attributePoints = int.Parse(attributePromptResult.Value);

                            var skillPromptResult = await UserDialogs.Instance.PromptAsync(new PromptConfig
                            {
                                Title = "Charaktererschaffung - Erfahrungspunkte für Fertigkeiten und Vor-/Nachteile",
                                Text = "1350",
                                Message = "(Dieser Wert kann später noch geändert werden)",
                                OkText = "OK",
                                CancelText = "Abbrechen",
                                InputType = InputType.Number
                            });

                            if (!skillPromptResult.Ok || string.IsNullOrWhiteSpace(skillPromptResult.Value))
                                return;

                            skillPoints = int.Parse(skillPromptResult.Value);

                        });

                        if (attributePoints == null || skillPoints == null)
                            return;

                        using (UserDialogs.Instance.Loading("Neuer Charakter wird erstellt.."))
                        {
                            await Task.Delay(250);
                            var newChar = _characterCreationService.CreateNewCharacter();
                            newChar.Name = _characterCreationService.GetRandomName();
                            newChar.Version = Version;
                            newChar.CharacterCreationAttributePoints = attributePoints.Value;
                            newChar.CharacterCreationSkillPoints = skillPoints.Value;

                            _characterService.AddCharacter(newChar);
                            RefreshData(false, true, false);

                            await OpenCharacter(newChar, true);
                            await Task.Delay(250);
                        }
                    }
                    catch (Exception e)
                    {
                        App.ErrorManager.TrackException(e);
                    }
                });
            }));

        private string _version;
        private ICommand _editCharacterJsonCommand;

        public ICommand EditCharacterJsonCommand => _editCharacterJsonCommand ?? (_editCharacterJsonCommand = new Command(() =>
        {
            if (SelectedCharacter == null)
                return;

            Task.Run(async () =>
            {
                Analytics.TrackEvent("Edit Character JSON");

                try
                {
                    var json = _characterService.GetCharacterJson(SelectedCharacter.Guid);

                    await Device.InvokeOnMainThreadAsync(async () =>
                    {
                        await Clipboard.SetTextAsync(json);
                    });

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        UserDialogs.Instance.Prompt(new PromptConfig
                        {
                            IsCancellable = true,
                            OkText = "Speichern", CancelText = "Abbrechen",
                            Title = $"\"{SelectedCharacter.Name}\" bearbeiten (JSON)",
                            Message = $"{Environment.NewLine}Das direkte Bearbeiten des Charakters kann bei fehlerhafter Benutzung dazu führen, dass die Daten unwiederruflich verloren gehen!{Environment.NewLine}",
                            InputType = InputType.Name,
                            Text = json,
                            OnAction = result =>
                            {
                                if (result.Ok == false)
                                    return;

                                CharacterEntity entity;
                                try
                                {
                                    entity = JsonConvert.DeserializeObject<CharacterEntity>(result.Text);
                                }
                                catch (Exception e)
                                {
                                    UserDialogs.Instance.Alert(
                                        $"Das eingegebene JSON konnte nicht gelesen werden{Environment.NewLine}{Environment.NewLine}Fehler: {e}",
                                        "Charakter speichern (JSON)", "OK");
                                    return;
                                }

                                var importResult = _characterService.SaveCharacter(entity);
                                if (importResult)
                                    RefreshData(false, true, true);
                                else
                                    UserDialogs.Instance.Alert($"Das eingegebene JSON konnte nicht gespeichert werden",
                                        "OK");
                            }
                        });
                    });
                }
                catch (Exception e)
                {
                    App.ErrorManager.TrackException(e, SelectedCharacter.Name);
                }
            });
        }));

        private ICommand _restoreBackupCommand;
        public ICommand RestoreBackupCommand => _restoreBackupCommand ?? (_restoreBackupCommand = new Command(() =>
        {
            Analytics.TrackEvent("Restore character");

            Device.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    var tmp = App.TempFolder;

                    //check if temp folder exisits
                    if (!Directory.Exists(tmp))
                        Directory.CreateDirectory(tmp);

                    var oldTmpFiles = Directory.GetFiles(tmp);
                    if (oldTmpFiles.Any())
                    {
                        foreach (var file in oldTmpFiles)
                            File.Delete(file);
                    }

                    //select file
                    var backupFile = await _localfileService.OpenAndCopyFileToFolder(tmp);
                    if (backupFile == null)
                        return;

                    //try read
                    CharacterPreview characterPreviewToRestore;
                    try
                    {
                        characterPreviewToRestore = _characterService.GetCharacterPreview(backupFile);

                        if (characterPreviewToRestore == null)
                            throw new ArgumentNullException(nameof(characterPreviewToRestore));
                    }
                    catch (Exception e)
                    {
                        App.ErrorManager.TrackExceptionSilent(e);
                        UserDialogs.Instance.Alert("Das Backup konnte nicht gelesen werden." +
                            $"{Environment.NewLine}{Environment.NewLine}Fehler:{Environment.NewLine}{e}", "Charakter aus Backup laden",
                            "OK");
                        return;
                    }

                    //check to replace existing
                    var existingCharacter = _characterService.GetCharacterPreview(characterPreviewToRestore.Guid);
                    if (existingCharacter != null)
                    {
                        if (characterPreviewToRestore.LastEdit == existingCharacter.LastEdit
                            && characterPreviewToRestore.Version == existingCharacter.Version)
                        {
                            UserDialogs.Instance.Alert("Das Backup ist identisch zum Speicherstand.", $"\"{characterPreviewToRestore.Name}\" aus Backup laden",
                                "OK");

                            return;
                        }

                        var backupFileInfo = "";
                        if (existingCharacter.LastEdit > characterPreviewToRestore.LastEdit)
                            backupFileInfo = "(Älter)";
                        else if (existingCharacter.LastEdit < characterPreviewToRestore.LastEdit)
                            backupFileInfo = "(Neuer)";

                        //ask to override
                        var result =
                        await UserDialogs.Instance.ConfirmAsync(
                            $"{Environment.NewLine}Soll der bestehende Speicherstand überschrieben werden?" +
                            $"{Environment.NewLine}{Environment.NewLine}Speicherstand:" +
                            $"{Environment.NewLine}Letzte Änderung: {existingCharacter.LastEdit}" +
                            $"{Environment.NewLine}Version: {existingCharacter.Version}" +
                            $"{Environment.NewLine}{Environment.NewLine}Backup-Datei {backupFileInfo}:" +
                            $"{Environment.NewLine}Letzte Änderung: {characterPreviewToRestore.LastEdit}" +
                            $"{Environment.NewLine}Version: {characterPreviewToRestore.Version}" +
                            $"{Environment.NewLine}{Environment.NewLine}Das Überschreiben des Charakters ist endgültig und kann nicht rückgägnig gemacht werden!",
                            $"\"{characterPreviewToRestore.Name}\" aus Backup laden", "Speicherstand überschreiben", "Abbrechen");

                        if (!result)
                            return;
                    }

                    //replace file in characters folder
                    _characterDatabaseConnection.ImportCharacterBackup(backupFile);

                    //refresh
                    RefreshData(false, true, false);
                }
                catch (Exception e)
                {
                    App.ErrorManager.TrackException(e, SelectedCharacter.Name);
                }
            });
        }));

        private ICommand _createBackupCommand;
        public ICommand CreateBackupCommand => _createBackupCommand ?? (_createBackupCommand = new Command(() =>
        {
            if (SelectedCharacter == null)
                return;

            Analytics.TrackEvent("Backup character");

            Device.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    var dbFile = _characterDatabaseConnection.GetCharacterDatabaseFile(SelectedCharacter.Guid);
                    await _localfileService.SaveFile(dbFile);

                    _characterService.UpdateLastBackup(SelectedCharacter.Guid);
                    RefreshData(false, true, false);
                }
                catch (Exception e)
                {
                    App.ErrorManager.TrackException(e, SelectedCharacter.Name);
                }
            });
        }));

        private ICommand _deleteCharacterCommand;
        private CharacterPreview _selectedCharacter;

        public ICommand DeleteCharacterCommand => _deleteCharacterCommand ?? (_deleteCharacterCommand = new Command(() =>
        {
            if (SelectedCharacter == null)
                return;

            Task.Run(async () =>
            {
                Analytics.TrackEvent("Try delete character");

                try
                {
                    await Device.InvokeOnMainThreadAsync(async () =>
                    {
                        var result = await UserDialogs.Instance.PromptAsync(new PromptConfig()
                        {
                            CancelText = "Abbrechen",
                            OkText = "Endgültig löschen",
                            Title = "Charakter löschen",
                            Message =
                                $"Das löschen ist endgültig und kann nicht wieder rückgängig gemacht werden{Environment.NewLine}{Environment.NewLine}" +
                                $"Zum löschen Trage bitte unten den Namen des Charakter \"{SelectedCharacter.Name}\" nochmal ein",
                            InputType = InputType.Name
                        });

                        if (result.Ok)
                        {
                            if (SelectedCharacter.Name?.Equals(result.Text) ?? false)
                            {
                                Analytics.TrackEvent("Deleting character");
                                _characterService.Delete(SelectedCharacter.Guid);
                                RefreshData(false, true, false);
                            }
                            else
                            {
                                UserDialogs.Instance.Alert(
                                    $"Der eingegebene Name stimmt nicht mit \"{SelectedCharacter.Name}\" überein",
                                    "Charakter löschen", "OK");
                            }
                        }
                    });
                }
                catch (Exception e)
                {
                    App.ErrorManager.TrackException(e, SelectedCharacter.Name);
                }
            });
        }));

        public void RefreshData(bool refreshWikiData, bool refreshCharacterList, bool resetCurrentCharacter)
        {
            if (resetCurrentCharacter)
                _characterProvider.ClearCurrentCharacter();

            if (refreshCharacterList)
            {
                var characters = _characterService.GetAllQuick();

                Device.BeginInvokeOnMainThread(() =>
                {
                    Characters.Clear();
                    foreach (var character in characters.OrderByDescending(entity => entity.LastEdit))
                    {
                        Characters.Add(character);
                    }

                    if (Characters.Any())
                        SelectedCharacter = Characters.First();
                });
            }

            if (refreshWikiData)
            {
                DatabaseInfoViewModel.ArmorTemplateCount = _wikiDataService.GetArmorWikiDataItemCount();
                DatabaseInfoViewModel.WeaponTemplateCount = _wikiDataService.GetWeaponWikiDataItemCount();
                DatabaseInfoViewModel.TalentTemplateCount = _wikiDataService.GetTalentWikiDataItemCount();
                DatabaseInfoViewModel.WeaveTalentTemplateCount = _wikiDataService.GetWeaveTalentWikiDataItemCount();
                DatabaseInfoViewModel.MasteryTemplateCount = _wikiDataService.GetMasteryWikiDataItemCount();
                DatabaseInfoViewModel.WikiDatabaseInfo = _wikiDataService.GetDatabaseInfo();
            }
        }
    }
}