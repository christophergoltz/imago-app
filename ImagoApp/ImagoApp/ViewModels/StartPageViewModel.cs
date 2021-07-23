using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using ImagoApp.Application;
using ImagoApp.Application.Models;
using ImagoApp.Application.Services;
using ImagoApp.Manager;
using ImagoApp.Util;
using ImagoApp.Views;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ImagoApp.ViewModels
{
    public class StartPageViewModel : BindableBase
    {
        private readonly ServiceLocator _serviceLocator;
        private readonly ICharacterService _characterService;
        private readonly IWikiParseService _wikiParseService;
        private readonly IWikiDataService _wikiDataService;
        private readonly IRuleService _ruleService;
        private readonly ICharacterCreationService _characterCreationService;
        private readonly string _appdataFolder;
        private readonly IFileService _fileService;
        private readonly string _logFileName = "wiki_parse.log";

        public ObservableCollection<CharacterModel> Characters { get; private set; }

        public string Version
        {
            get => _version;
            set => SetProperty(ref _version , value);
        }

        public DatabaseInfoViewModel DatabaseInfoViewModel { get; set; }

        public StartPageViewModel(ServiceLocator serviceLocator, 
            ICharacterService characterService,
            IWikiParseService wikiParseService,
            IWikiDataService wikiDataService,
            IRuleService ruleService,
            ICharacterCreationService characterCreationService,
            string appdataFolder, IFileService fileService)
        {
            VersionTracking.Track();
            Version = new Version(VersionTracking.CurrentVersion).ToString(3);
            DatabaseInfoViewModel = new DatabaseInfoViewModel();
            Characters = new ObservableCollection<CharacterModel>();

            _serviceLocator = serviceLocator;
            _characterService = characterService;
            _wikiParseService = wikiParseService;
            _wikiDataService = wikiDataService;
            _ruleService = ruleService;
            _characterCreationService = characterCreationService;
            _appdataFolder = appdataFolder;
            _fileService = fileService;

            EnsurePreferences();

            Task.Run(() =>
            {
                RefreshData(false);
                CheckWikiData();

                if(VersionTracking.IsFirstLaunchForCurrentBuild || VersionTracking.IsFirstLaunchForCurrentVersion)
                    AlertNewVersion();
            });
        }

        private const string FontSizePreferenceKey = "FonzScale";
        private void EnsurePreferences()
        {
            bool hasKey = Preferences.ContainsKey(FontSizePreferenceKey);
            if (!hasKey)
            {
                Preferences.Set(FontSizePreferenceKey, 100);
            }
            else
            {
                //apply setting
                var diff =  Preferences.Get(FontSizePreferenceKey, 100) - 100;
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
            var value =Preferences.Get(FontSizePreferenceKey, 100);
            Preferences.Set(FontSizePreferenceKey, value + 10);
            StyleResourceManager.ChangeGlobalFontSize(1);
            OnPropertyChanged(nameof(FontScale));
        }));

        private ICommand _decreaseFontSizeCommand;
        public ICommand DecreaseFontSizeCommand => _decreaseFontSizeCommand ?? (_decreaseFontSizeCommand = new Command(() =>
        {
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
                _fileService.OpenFolder(_appdataFolder);
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
                var logFile = Path.Combine(_appdataFolder, $"wiki_parse.log");
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
                        progressDialog.Title = "Rüstungen werden geladen";
                        await Task.Delay(500);

                        var armorCount = _wikiParseService.RefreshArmorFromWiki(logger);
                        progressDialog.Title = "Waffen werden geladen";
                        progressDialog.PercentComplete = 25;
                        await Task.Delay(50);

                        var weaponCount = _wikiParseService.RefreshWeaponsFromWiki(logger);
                        progressDialog.Title = "Talente werden geladen";
                        progressDialog.PercentComplete = 50;
                        await Task.Delay(50);

                        var talentCount = _wikiParseService.RefreshTalentsFromWiki(logger);
                        progressDialog.Title = "Meisterschaften werden geladen";
                        progressDialog.PercentComplete = 75;
                        await Task.Delay(50);

                        var masteryCount = _wikiParseService.RefreshMasteriesFromWiki(logger);
                        progressDialog.Title = "Wird abgeschlossen";
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
                                  $"{Environment.NewLine}      Talente: {talentCount?.ToString()}" +
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
                                    var target = new ReadOnlyFile(Path.Combine(_appdataFolder, _logFileName));
                                    var request = new OpenFileRequest {File = target};
                                    Launcher.OpenAsync(request);
                                }
                            }
                        });
                    }
                }

                RefreshData(false);
            }
            catch (Exception exception)
            {
                App.ErrorManager.TrackException(exception);
            }
        }));
        
        public async Task OpenCharacter(CharacterModel characterModel, bool editMode)
        {
            var characterViewModel = new CharacterViewModel(characterModel, _ruleService)
            {
                EditMode = editMode
            };

            App.CurrentCharacterViewModel = characterViewModel;

            try
            {
                //create all required dependencies
                var characterInfoPageViewModel = new CharacterInfoPageViewModel(characterViewModel, _serviceLocator.RuleService());
                var wikiPageViewModel = new WikiPageViewModel(characterViewModel);
                var skillPageViewModel = new SkillPageViewModel(characterViewModel, _serviceLocator.WikiService(), _serviceLocator.WikiDataService(), _serviceLocator.RuleService());
                var statusPageViewModel = new StatusPageViewModel(characterViewModel, _serviceLocator.WikiDataService());
                var inventoryViewModel = new InventoryViewModel(characterViewModel);
                var appShellViewModel = new AppShellViewModel(characterViewModel, characterInfoPageViewModel, skillPageViewModel,
                    statusPageViewModel, inventoryViewModel, wikiPageViewModel);

                //notify the main menu that editmode may have changed
                appShellViewModel.RaiseEditModeChanged();

                skillPageViewModel.OpenWikiPageRequested += (sender, url) => OpenWikiPage(url);
                statusPageViewModel.OpenWikiPageRequested += (sender, url) => OpenWikiPage(url);
                inventoryViewModel.OpenWikiPageRequested += (sender, url) => OpenWikiPage(url);

                void OpenWikiPage(string url)
                {
                    Analytics.TrackEvent("Open WikiPage", new Dictionary<string, string>()
                    {
                        {"url", url}
                    });

                    appShellViewModel.RaiseWikiPageOpenRequested();
                    wikiPageViewModel.OpenWikiPage(url);
                }
                
                var appShell = new AppShell(appShellViewModel);

                await Device.InvokeOnMainThreadAsync(() =>
                {
                    App.StartPage = (StartPage) Xamarin.Forms.Application.Current.MainPage;
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
                    try
                    {
                        using (UserDialogs.Instance.Loading("Neuer Charakter wird erstellt.."))
                        {
                            await Task.Delay(250);
                            var newChar = _characterCreationService.CreateNewCharacter();
                            newChar.Name = "";
                            newChar.Version = Version;

                            _characterService.AddCharacter(newChar);
                            RefreshData(true);

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

        private ICommand _generateTestCharacterCommand;
        private string _version;
        
        public ICommand GenerateTestCharacterCommand => _generateTestCharacterCommand ?? (_generateTestCharacterCommand = new Command(() =>
            {
                Task.Run(async () =>
                {
                    try
                    {
                        using (UserDialogs.Instance.Loading("Testcharacter wird geladen.."))
                        {
                            await Task.Delay(250);
                            var newChar = _characterCreationService.CreateExampleCharacter();
                            newChar.Name = "Testspieler";
                            newChar.CreatedBy = "System";
                            newChar.Owner = "Testuser";
                            newChar.Version = Version;

                            _characterService.AddCharacter(newChar);
                            RefreshData(true);

                            await OpenCharacter(newChar, false);
                            await Task.Delay(250);
                        }
                    }
                    catch (Exception e)
                    {
                        App.ErrorManager.TrackException(e);
                    }
                });
            }));
        
        public void RefreshData(bool resetCurrentCharacter)
        {
            if (resetCurrentCharacter)
                App.CurrentCharacterViewModel = null;

            var characters = _characterService.GetAll();

            Device.BeginInvokeOnMainThread(() =>
            {
                Characters.Clear();
                foreach (var character in characters.OrderByDescending(entity => entity.LastEdit))
                {
                    Characters.Add(character);
                }
            });

            DatabaseInfoViewModel.ArmorTemplateCount = _wikiDataService.GetArmorWikiDataItemCount();
            DatabaseInfoViewModel.WeaponTemplateCount = _wikiDataService.GetWeaponWikiDataItemCount();
            DatabaseInfoViewModel.TalentTemplateCount = _wikiDataService.GetTalentWikiDataItemCount();
            DatabaseInfoViewModel.MasteryTemplateCount = _wikiDataService.GetMasteryWikiDataItemCount();
            DatabaseInfoViewModel.WikiDatabaseInfo = _wikiDataService.GetDatabaseInfo();
            DatabaseInfoViewModel.CharacterDatabaseInfo = _characterService.GetDatabaseInfo();
        }
    }
}