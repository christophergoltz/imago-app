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
using ImagoApp.Infrastructure.Repositories;
using ImagoApp.Shared.Enums;
using ImagoApp.Util;
using ImagoApp.Views;
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
        private readonly IWikiService _wikiService;
        private readonly string _appdataFolder;
        private readonly IFileService _fileService;
        private readonly string _logFileName = "wiki_parse.log";

        public ObservableCollection<Character> Characters { get; private set; }

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
            IWikiService wikiService,
            string appdataFolder, IFileService fileService)
        {
            VersionTracking.Track();
            Version = VersionTracking.CurrentVersion;
            DatabaseInfoViewModel = new DatabaseInfoViewModel();
            Characters = new ObservableCollection<Character>();

            _serviceLocator = serviceLocator;
            _characterService = characterService;
            _wikiParseService = wikiParseService;
            _wikiDataService = wikiDataService;
            _ruleService = ruleService;
            _characterCreationService = characterCreationService;
            _wikiService = wikiService;
            _appdataFolder = appdataFolder;
            _fileService = fileService;

            Task.Run(() =>
            {
                RefreshData(false);
                CheckWikiData();
            });
        }

        private ICommand _openAppDataFolderCommand;

        public ICommand OpenAppDataFolderCommand => _openAppDataFolderCommand ?? (_openAppDataFolderCommand = new Command(() =>
        {
            _fileService.OpenFolder(_appdataFolder);
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
                Events.Add(le);
            }
        }

        private ICommand _openChangeLogCommand;
        public ICommand OpenChangeLogCommand => _openChangeLogCommand ?? (_openChangeLogCommand = new Command(() =>
        {
            Launcher.OpenAsync(_wikiService.GetChangelogUrl());
        }));

        private ICommand _parseWikiCommand;

        public ICommand ParseWikiCommand => _parseWikiCommand ?? (_parseWikiCommand = new Command(async () =>
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
                    await Task.Delay(150);

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
                        Title = "Daten wurde aus dem Wiki gelesen",
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
        }));

        private ICommand _openCharacterCommand;
        public ICommand OpenCharacterCommand => _openCharacterCommand ?? (_openCharacterCommand = new Command<Character>(entity =>
        {
            Task.Run(async () =>
            {
                using (UserDialogs.Instance.Loading("Character wird geladen.."))
                {
                    await Task.Delay(250);
                    await OpenCharacter(entity, false);
                    await Task.Delay(250);
                }
            });
        }));
        
        private async Task OpenCharacter(Character character, bool editMode)
        {
            var characterViewModel = new CharacterViewModel(character, _ruleService);
            App.CurrentCharacterViewModel = characterViewModel;

            try
            {
                //create all required dependencies
                var characterInfoPageViewModel = new CharacterInfoPageViewModel(characterViewModel, _serviceLocator.RuleService());
                var wikiPageViewModel = new WikiPageViewModel(characterViewModel);
                var skillPageViewModel = new SkillPageViewModel(characterViewModel, _serviceLocator.WikiService(),
                    _serviceLocator.WikiDataService(), _serviceLocator.RuleService());
                skillPageViewModel.OpenWikiPageRequested += (sender, s) => { wikiPageViewModel.OpenWikiPage(s); };
                var statusPageViewModel = new StatusPageViewModel(characterViewModel, _serviceLocator.WikiDataService());
                var inventoryViewModel = new InventoryViewModel(characterViewModel);
                var appShellViewModel = new AppShellViewModel(_characterService, characterInfoPageViewModel, skillPageViewModel, statusPageViewModel, inventoryViewModel, wikiPageViewModel)
                {
                    EditMode = editMode
                };

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
                    using (UserDialogs.Instance.Loading("Neuer Charakter wird erstellt.."))
                    {
                        await Task.Delay(250);
                        var newChar = _characterCreationService.CreateNewCharacter();
                        newChar.Name = "";
                        newChar.RaceType = RaceType.Mensch;
                        newChar.Version = Version;
                        
                        _characterService.AddCharacter(newChar);
                        RefreshData(true);

                        await OpenCharacter(newChar, true);
                        await Task.Delay(250);
                    }
                });
            }));

        private ICommand _generateTestCharacterCommand;
        private string _version;
        
        public ICommand GenerateTestCharacterCommand => _generateTestCharacterCommand ?? (_generateTestCharacterCommand = new Command(() =>
            {
                Task.Run(async () =>
                {
                    using (UserDialogs.Instance.Loading("Testcharacter wird geladen.."))
                    {
                        await Task.Delay(250);
                        var newChar = _characterCreationService.CreateExampleCharacter();
                        newChar.Name = "Testspieler";
                        newChar.RaceType = RaceType.Mensch;
                        newChar.CreatedBy = "System";
                        newChar.Owner = "Testuser";
                        newChar.Version = Version;
                        
                        _characterService.AddCharacter(newChar);
                        RefreshData(true);

                        await OpenCharacter(newChar, false);
                        await Task.Delay(250);
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