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
using ImagoApp.Infrastructure.Entities;
using ImagoApp.Infrastructure.Repositories;
using ImagoApp.Services;
using ImagoApp.Shared.Enums;
using ImagoApp.Util;
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
            ICharacterCreationService characterCreationService)
        {
            VersionTracking.Track();
            Version = VersionTracking.CurrentVersion;

            _appShellViewModel = appShellViewModel;
            _characterService = characterService;
            _wikiParseService = wikiParseService;
            _wikiDataService = wikiDataService;
            _ruleService = ruleService;
            _characterCreationService = characterCreationService;

            Task.Run(async () =>
            {
                try
                {
                    await _wikiDataService.Initialize();
                    await _characterService.Initialize();
                }
                catch (Exception e)
                {
                    //todo
                    Debug.WriteLine(e);
                    throw;
                }

                await RefreshDatabaseInfos();
                await RefreshCharacterList();

            });
        }

        private int GetPercentage(int current, int total)
        {
            return (int)((double)current / total) * 100;
        }

        private ICommand _parseWikiCommand;
        public ICommand ParseWikiCommand => _parseWikiCommand ?? (_parseWikiCommand = new Command(async () =>
        {
            var totalActionCount = 4;
            var currentActionCount = 0;

            using (IProgressDialog progressDialog =
                UserDialogs.Instance.Progress("Daten werden aus dem Wiki gelesen.."))
            {
                progressDialog.PercentComplete = GetPercentage(currentActionCount, totalActionCount);


                await Task.Delay(5000);

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
                        newChar.Id = Guid.NewGuid();
                        newChar.CreatedAt = DateTime.Now;
                        newChar.LastModifiedAt = DateTime.Now;
                        newChar.Version = Version;
                        
                        await _characterService.AddCharacter(newChar);
                        await RefreshCharacterList();

                        await OpenCharacter(newChar, true);
                        await Task.Delay(250);
                    }
                });
            }));

        private ICommand _generateTestCharacterCommand;
        private (int ItemCount, FileInfo FileInfo) _wikiDatabaseInfo;
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
                        newChar.Id = Guid.NewGuid();
                        newChar.Version = Version;
                        
                        await _characterService.AddCharacter(newChar);
                        await RefreshCharacterList();
                        await Task.Delay(250);
                    }
                });
            }));
        
        private async Task RefreshCharacterList()
        {
            var characters = await _characterService.GetAll();
            Characters = new ObservableCollection<Character>(characters.OrderByDescending(entity => entity.LastModifiedAt));
        }

        private async Task RefreshDatabaseInfos()
        {
            var info = await _wikiDataService.GetDatabaseInfo();
            WikiDatabaseInfo = info;
        }
    }
}