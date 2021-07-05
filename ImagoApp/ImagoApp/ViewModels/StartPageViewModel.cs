using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using ImagoApp.Application.Models;
using ImagoApp.Application.Services;
using ImagoApp.Services;
using ImagoApp.Shared.Enums;
using ImagoApp.Util;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ImagoApp.ViewModels
{
    public class StartPageViewModel : BindableBase
    {
        private readonly AppShellViewModel _appShellViewModel;
        private readonly ICharacterService _characterService;
        private readonly IWikiParseService _wikiParseService;
        private readonly IWikiDataService _wikiDataService;
        private readonly IRuleService _ruleService;
        private readonly ICharacterCreationService _characterCreationService;
        private readonly string _logFolder;
        private ObservableCollection<Character> _characters;

        public ObservableCollection<Character> Characters
        {
            get => _characters;
            set => SetProperty(ref _characters, value);
        }

        public (int ItemCount, FileInfo FileInfo) WikiDatabaseInfo
        {
            get => _wikiDatabaseInfo;
            set => SetProperty(ref _wikiDatabaseInfo, value);
        }

        public string Version
        {
            get => _version;
            set => SetProperty(ref _version , value);
        }

        public StartPageViewModel(AppShellViewModel appShellViewModel,
            ICharacterService characterService,
            IWikiParseService wikiParseService,
            IWikiDataService wikiDataService,
            IRuleService ruleService,
            ICharacterCreationService characterCreationService,
            string logFolder)
        {
            VersionTracking.Track();
            Version = VersionTracking.CurrentVersion;

            _appShellViewModel = appShellViewModel;
            _characterService = characterService;
            _wikiParseService = wikiParseService;
            _wikiDataService = wikiDataService;
            _ruleService = ruleService;
            _characterCreationService = characterCreationService;
            _logFolder = logFolder;

            Task.Run(async () =>
            {
                await RefreshDatabaseInfos();
                await RefreshCharacterList();
            });
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

        private ICommand _parseWikiCommand;

        public ICommand ParseWikiCommand => _parseWikiCommand ?? (_parseWikiCommand = new Command(async () =>
        {
            var totalActionCount = 4;
            var currentActionCount = 0;

            var logFile = Path.Combine(_logFolder, $"wiki_parse_{DateTime.Now:dd.MM.yyyy_HH.mm}.log");

            var col = new CollectionSink();

            var logger = new LoggerConfiguration()
                .WriteTo.Debug()
                .WriteTo.File(logFile)
                .WriteTo.Sink(col)
                .CreateLogger();

            using (var progressDialog = UserDialogs.Instance.Progress(""))
            {
                progressDialog.Title = GetProgressDialog("Rüstungen werden geladen", col);
                await Task.Delay(50);

                var armorCount = await _wikiParseService.RefreshArmorFromWiki(logger);
                IncreaseProgressPercentage(progressDialog, ref currentActionCount, totalActionCount);
                progressDialog.Title = GetProgressDialog("Waffen werden geladen", col, armorCount);
                await Task.Delay(50);

                var weaponCount = await _wikiParseService.RefreshWeaponsFromWiki(logger);
                IncreaseProgressPercentage(progressDialog, ref currentActionCount, totalActionCount);
                progressDialog.Title = GetProgressDialog("Talente werden geladen", col, armorCount, weaponCount);
                await Task.Delay(50);

                var talentCount = await _wikiParseService.RefreshTalentsFromWiki(logger);
                IncreaseProgressPercentage(progressDialog, ref currentActionCount, totalActionCount);
                progressDialog.Title = GetProgressDialog("Meisterschaften werden geladen", col, armorCount, weaponCount,
                    talentCount);
                await Task.Delay(50);

                var masteryCount = await _wikiParseService.RefreshMasteriesFromWiki(logger);
                IncreaseProgressPercentage(progressDialog, ref currentActionCount, totalActionCount);
                progressDialog.Title = GetProgressDialog("Wird abgeschlossen", col, armorCount, weaponCount,
                    talentCount, masteryCount);
                await Task.Delay(1500);
            }

            await RefreshDatabaseInfos();
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
            var vm = new CharacterViewModel(character, _ruleService);
            App.CurrentCharacterViewModel = vm;

            if (editMode)
            {
                _appShellViewModel.EditMode = true;
            }

            await Device.InvokeOnMainThreadAsync(() =>
            {
                Xamarin.Forms.Application.Current.MainPage = new AppShell();
            });
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
                        newChar.Guid = Guid.NewGuid();
                        newChar.CreatedAt = DateTime.Now;
                        newChar.Version = Version;
                        
                        _characterService.AddCharacter(newChar);
                        await RefreshCharacterList();

                        await OpenCharacter(newChar, true);
                        await Task.Delay(250);
                    }
                });
            }));

        private ICommand _generateTestCharacterCommand;
        private (int ItemCount, FileInfo FileInfo) _wikiDatabaseInfo;
        private string _version;
        private int _percent;

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
                        newChar.Guid = Guid.NewGuid();
                        newChar.Version = Version;
                        
                        _characterService.AddCharacter(newChar);
                        await RefreshCharacterList();
                        await Task.Delay(250);
                    }
                });
            }));
        
        private async Task RefreshCharacterList()
        {
            var characters = _characterService.GetAll();
            Characters = new ObservableCollection<Character>(characters.OrderByDescending(entity => entity.LastEdit));
        }

        private async Task RefreshDatabaseInfos()
        {
            var info = _wikiDataService.GetDatabaseInfo();
            WikiDatabaseInfo = info;
        }
    }
}