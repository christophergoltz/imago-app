﻿using System;
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
        private readonly ViewModelLocator _viewModelLocator;
        private readonly ICharacterService _characterService;
        private readonly IWikiParseService _wikiParseService;
        private readonly IWikiDataService _wikiDataService;
        private readonly IRuleService _ruleService;
        private readonly ICharacterCreationService _characterCreationService;
        private readonly IWikiService _wikiService;
        private readonly string _logFolder;
        private readonly IGithubUpdateService _githubUpdateService;
        private readonly string _logFileName = "wiki_parse.log";
        private ObservableCollection<Character> _characters;

        public ObservableCollection<Character> Characters
        {
            get => _characters;
            set => SetProperty(ref _characters, value);
        }

        public string Version
        {
            get => _version;
            set => SetProperty(ref _version , value);
        }

        public DatabaseInfoViewModel DatabaseInfoViewModel { get; set; }

        public StartPageViewModel(ViewModelLocator viewModelLocator, 
            ICharacterService characterService,
            IWikiParseService wikiParseService,
            IWikiDataService wikiDataService,
            IRuleService ruleService,
            ICharacterCreationService characterCreationService,
            IWikiService wikiService,
            string logFolder,
            IGithubUpdateService githubUpdateService)
        {
            VersionTracking.Track();
            Version = VersionTracking.CurrentVersion;
            DatabaseInfoViewModel = new DatabaseInfoViewModel();
            UpdateButtonText = "Updates werden abgerufen..";

            _viewModelLocator = viewModelLocator;
            _characterService = characterService;
            _wikiParseService = wikiParseService;
            _wikiDataService = wikiDataService;
            _ruleService = ruleService;
            _characterCreationService = characterCreationService;
            _wikiService = wikiService;
            _logFolder = logFolder;
            _githubUpdateService = githubUpdateService;

            Task.Run(() =>
            {
                RefreshDatabaseInfos();
                RefreshCharacterList();
                CheckForUpdate();
            });
        }

        private ICommand _updateToLatestVersionCommand;

        public ICommand UpdateToLatestVersionCommand => _updateToLatestVersionCommand ?? (_updateToLatestVersionCommand = new Command(() =>
        {
            //todo call updater
        }, () => _updateAvailable));

        private bool _updateAvailable = false;

        public string UpdateButtonText
        {
            get => _updateButtonText;
            set => SetProperty(ref _updateButtonText, value);
        }

        private void CheckForUpdate()
        {
            var latestRelease = _githubUpdateService.GetLatestRelease();
            if (latestRelease > new Version(Version))
            {
                //update avaliable
                UpdateButtonText = latestRelease + " verfügbar";
                _updateAvailable = true;
            }
            else
            {
                UpdateButtonText = "aktuell";
                _updateAvailable = false;
            }

            Device.BeginInvokeOnMainThread(() => ((Command)UpdateToLatestVersionCommand).ChangeCanExecute());
        }

        private void IncreaseProgressPercentage(IProgressDialog progressDialog, ref int current, int total)
        {
            current++;
            _percent = (int) ((double) current / total * 100);
            progressDialog.PercentComplete = _percent;
        }

        private string GetProgressDialog(string currentTitle, CollectionSink collectionSink, int? armorCount = null, int? weaponCount = null, int? talentCount = null, int? masteryCount = null)
        {
            return $"Daten werden aus dem Wiki gelesen.." +
                   $"{Environment.NewLine}" +
                   $"--------------------------------------------------------" +
                   $"{Environment.NewLine}" +
                   $"{Environment.NewLine}" +
                   $"{currentTitle}" +
                   $"{Environment.NewLine}" +
                   $"{Environment.NewLine}" +
                   $"Status:" +
                   $"{Environment.NewLine}      Warnungen: {collectionSink.Events.Count(_=>_.Level == LogEventLevel.Warning)}" +
                   $"{Environment.NewLine}      Fehler: {collectionSink.Events.Count(_ => _.Level == LogEventLevel.Error)}" +
                   $"{Environment.NewLine}      Kritisch: {collectionSink.Events.Count(_ => _.Level == LogEventLevel.Fatal)}" +
                   $"{Environment.NewLine}" +
                   $"{Environment.NewLine}" +
                   $"Gefundene Daten:" +
                   $"{Environment.NewLine}      Rüstungen: {armorCount?.ToString()}" +
                   $"{Environment.NewLine}      Waffen: {weaponCount?.ToString()}" +
                   $"{Environment.NewLine}      Talente: {talentCount?.ToString()}" +
                   $"{Environment.NewLine}      Meisterschaften: {masteryCount?.ToString()}" +
                   $"{Environment.NewLine}";
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
            var totalActionCount = 4;
            var currentActionCount = 0;

            //         var logFile = Path.Combine(_logFolder, $"wiki_parse_{DateTime.Now:dd.MM.yyyy_HH.mm}.log");
            var logFile = Path.Combine(_logFolder, $"wiki_parse.log");
            if (File.Exists(logFile))
                File.Delete(logFile);

            var col = new CollectionSink();


            using (var logger = new LoggerConfiguration().WriteTo.Debug()
                .WriteTo.File(logFile)
                .WriteTo.Sink(col)
                .CreateLogger())
            {
                using (var progressDialog = UserDialogs.Instance.Progress(""))
                {
                    progressDialog.Title = GetProgressDialog("Rüstungen werden geladen", col);
                    await Task.Delay(50);

                    var armorCount = _wikiParseService.RefreshArmorFromWiki(logger);
                    IncreaseProgressPercentage(progressDialog, ref currentActionCount, totalActionCount);
                    progressDialog.Title = GetProgressDialog("Waffen werden geladen", col, armorCount);
                    await Task.Delay(50);

                    var weaponCount = _wikiParseService.RefreshWeaponsFromWiki(logger);
                    IncreaseProgressPercentage(progressDialog, ref currentActionCount, totalActionCount);
                    progressDialog.Title = GetProgressDialog("Talente werden geladen", col, armorCount, weaponCount);
                    await Task.Delay(50);

                    var talentCount = _wikiParseService.RefreshTalentsFromWiki(logger);
                    IncreaseProgressPercentage(progressDialog, ref currentActionCount, totalActionCount);
                    progressDialog.Title = GetProgressDialog("Meisterschaften werden geladen", col, armorCount,
                        weaponCount,
                        talentCount);
                    await Task.Delay(50);

                    var masteryCount = _wikiParseService.RefreshMasteriesFromWiki(logger);
                    IncreaseProgressPercentage(progressDialog, ref currentActionCount, totalActionCount);
                    progressDialog.Title = GetProgressDialog("Wird abgeschlossen", col, armorCount, weaponCount,
                        talentCount, masteryCount);
                    await Task.Delay(1500);
                }
            }

            ((Command)OpenLogFileCommand).ChangeCanExecute();

            RefreshDatabaseInfos();
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

        private ICommand _openLogFileCommand;

        public ICommand OpenLogFileCommand => _openLogFileCommand ?? (_openLogFileCommand = new Command(() =>
        {
            var target = new ReadOnlyFile(Path.Combine(_logFolder, _logFileName));
            var request = new OpenFileRequest { File = target };
            Launcher.OpenAsync(request);
        }, () => File.Exists(Path.Combine(_logFolder, _logFileName))));

        private async Task OpenCharacter(Character character, bool editMode)
        {
            var viewModel = new CharacterViewModel(character, _ruleService);
            App.CurrentCharacterViewModel = viewModel;

            try
            {

                //create all required dependencies
                var c = new CharacterInfoPageViewModel(viewModel, _viewModelLocator.RuleService());
                var i = new WikiPageViewModel();
                var t = new SkillPageViewModel(viewModel, _viewModelLocator.WikiService(),
                    _viewModelLocator.WikiDataService(), _viewModelLocator.RuleService());
                t.OpenWikiPageRequested += (sender, s) => { i.OpenWikiPage(s); };
                var z = new StatusPageViewModel(viewModel, _viewModelLocator.WikiDataService());
                var u = new InventoryViewModel(viewModel);
                var vm = new AppShellViewModel(_characterService, c, t, z, u, i)
                {
                    EditMode = editMode
                };

                var shell = new AppShell(vm);

                await Device.InvokeOnMainThreadAsync(() =>
                {
                    App.StartPage = (StartPage) Xamarin.Forms.Application.Current.MainPage;
                    Xamarin.Forms.Application.Current.MainPage = shell;
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
                        RefreshCharacterList();

                        await OpenCharacter(newChar, true);
                        await Task.Delay(250);
                    }
                });
            }));

        private ICommand _generateTestCharacterCommand;
        private string _version;
        private int _percent;
        private string _updateButtonText;

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
                        RefreshCharacterList();

                        await OpenCharacter(newChar, false);
                        await Task.Delay(250);
                    }
                });
            }));
        
        private void RefreshCharacterList()
        {
            var characters = _characterService.GetAll();
            Characters = new ObservableCollection<Character>(characters.OrderByDescending(entity => entity.LastEdit));
        }

        private void RefreshDatabaseInfos()
        {
            DatabaseInfoViewModel.ArmorTemplateCount = _wikiDataService.GetArmorWikiDataItemCount();
            DatabaseInfoViewModel.WeaponTemplateCount = _wikiDataService.GetWeaponWikiDataItemCount();
            DatabaseInfoViewModel.TalentTemplateCount = _wikiDataService.GetTalentWikiDataItemCount();
            DatabaseInfoViewModel.MasteryTemplateCount = _wikiDataService.GetMasteryWikiDataItemCount();
            DatabaseInfoViewModel.WikiDatabaseInfo = _wikiDataService.GetDatabaseInfo();
            DatabaseInfoViewModel.CharacterDatabaseInfo = _characterService.GetDatabaseInfo();
        }
    }
}