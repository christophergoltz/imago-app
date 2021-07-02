using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using Imago.Database;
using Imago.Models;
using Imago.Models.Entity;
using Imago.Models.Enum;
using Imago.Repository;
using Imago.Repository.WrappingDatabase;
using Imago.Services;
using Imago.Util;
using Imago.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Imago.ViewModels
{
    public class StartPageViewModel : BindableBase
    {
        private readonly AppShellViewModel _appShellViewModel;
        private readonly ICharacterRepository _characterRepository;
        private readonly ICharacterService _characterService;
        private readonly IWikiParseService _wikiParseService;
        private readonly IMeleeWeaponRepository _meleeWeaponRepository;
        private readonly IRangedWeaponRepository _rangedWeaponRepository;
        private readonly IArmorRepository _armorRepository;
        private readonly ITalentRepository _talentRepository;
        private readonly ISpecialWeaponRepository _specialWeaponRepository;
        private readonly IShieldRepository _shieldRepository;
        private readonly IMasteryRepository _masteryRepository;
        private readonly IRuleRepository _ruleRepository;
        private Dictionary<TableInfoType, TableInfoModel> _tableInfos;
        private ObservableCollection<CharacterEntity> _characters;

        public ObservableCollection<CharacterEntity> Characters
        {
            get => _characters;
            set => SetProperty(ref _characters, value);
        }


        public StartPageViewModel(AppShellViewModel appShellViewModel,
            ICharacterRepository characterRepository,
            ICharacterService characterService,
            IWikiParseService wikiParseService,
            IMeleeWeaponRepository meleeWeaponRepository,
            IRangedWeaponRepository rangedWeaponRepository,
            IArmorRepository armorRepository,
            ITalentRepository talentRepository,
            ISpecialWeaponRepository specialWeaponRepository,
            IShieldRepository shieldRepository,
            IMasteryRepository masteryRepository,
            IRuleRepository ruleRepository)
        {
            VersionTracking.Track();

            _appShellViewModel = appShellViewModel;
            _characterRepository = characterRepository;
            _characterService = characterService;
            _wikiParseService = wikiParseService;
            _meleeWeaponRepository = meleeWeaponRepository;
            _rangedWeaponRepository = rangedWeaponRepository;
            _armorRepository = armorRepository;
            _talentRepository = talentRepository;
            _specialWeaponRepository = specialWeaponRepository;
            _shieldRepository = shieldRepository;
            _masteryRepository = masteryRepository;
            _ruleRepository = ruleRepository;


#pragma warning disable 4014
            InitLocalDatabase(); //needs to be executed in background
#pragma warning restore 4014

            _appShellViewModel.CharacterListReloadRequested += (sender, args) =>
            {
                RefreshCharacterList();
            };
        }


        private ICommand _parseWikiCommand;
        public ICommand ParseWikiCommand => _parseWikiCommand ?? (_parseWikiCommand = new Command(async () =>
        {
            WikiParseLog.Clear();

            foreach (var tableInfoModel in TableInfos.Values)
            {
                if (tableInfoModel.Type == TableInfoType.Character)
                    continue;

                try
                {
                    tableInfoModel.State = TableInfoState.Loading;
                    await Task.Delay(200);
                    var result = await _wikiParseService.RefreshWikiData(tableInfoModel.Type, WikiParseLog);
                    if (result == null)
                    {
                        tableInfoModel.State = TableInfoState.Error;
                        await Task.Delay(200);
                        continue;
                    }

                    if (result.Value == 0)
                    {
                        tableInfoModel.State = TableInfoState.NoData;
                        await Task.Delay(200);
                        continue;
                    }

                    tableInfoModel.State = TableInfoState.Okay;
                    await Task.Delay(200);
                }
                catch (Exception e)
                {
                    tableInfoModel.State = TableInfoState.Error;
                    await Task.Delay(200);
                    Debug.WriteLine(e);
                }

            }

            await RefreshDatabaseInfos();
        }));

        private ICommand _openCharacterCommand;
        public ICommand OpenCharacterCommand => _openCharacterCommand ?? (_openCharacterCommand = new Command<CharacterEntity>(entity =>
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

        private async Task OpenCharacter(CharacterEntity characterEntity, bool editMode)
        {
            var character = characterEntity.Value;
            character.Name = characterEntity.Name;
            character.Id = characterEntity.Id;
            character.Version = characterEntity.Version;
            character.CreatedAt = characterEntity.CreatedAt;
            character.LastModifiedAt = characterEntity.LastModifiedAt;

            var vm = new CharacterViewModel(character, _ruleRepository);
            _characterService.SetCurrentCharacter(vm);

            if (editMode)
            {
                //use different open method
                //Element ce = Shell.Current.CurrentPage;

                //while (true)
                //{
                //    if (ce is AppShell shell)
                //    {
                //        if (shell.BindingContext is AppShellViewModel appShellViewModel)
                //        {
                //            appShellViewModel.EditMode = true;
                //            break;
                //        }
                //    }
                //    else
                //    {
                //        ce = ce.Parent;
                //    }
                //}
                _appShellViewModel.EditMode = true;
            }

            await Device.InvokeOnMainThreadAsync(async () =>
            {
                await Shell.Current.GoToAsync($"//{nameof(CharacterInfoPage)}");
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
                        var newGuid = Guid.NewGuid();

                        var newChar = _characterRepository.CreateNewCharacter();
                        newChar.Name = newGuid.ToString().Substring(0, 4);
                        newChar.RaceType = RaceType.Mensch;
                        newChar.Id = newGuid;
                        newChar.CreatedAt = DateTime.Now;
                        newChar.LastModifiedAt = DateTime.Now;

                        var currentAppVersion = VersionTracking.CurrentVersion;
                        var entity = new CharacterEntity()
                        {
                            CreatedAt = newChar.CreatedAt,
                            Value = newChar,
                            Name = newChar.Name,
                            LastModifiedAt = newChar.LastModifiedAt,
                            Id = newGuid,
                            Version = currentAppVersion
                        };
                        await _characterRepository.AddItemRawAsync(entity);
                        await RefreshCharacterList();

                        await OpenCharacter(entity, true);
                        await Task.Delay(250);
                    }
                });
            }));

        private ICommand _generateTestCharacterCommand;
        public ICommand GenerateTestCharacterCommand => _generateTestCharacterCommand ?? (_generateTestCharacterCommand = new Command(() =>
            {
                Task.Run(async () =>
                {
                    using (UserDialogs.Instance.Loading("Testcharacter wird geladen.."))
                    {
                        await Task.Delay(250);
                        var newGuid = Guid.NewGuid();

                        var newChar = _characterRepository.CreateExampleCharacter();
                        newChar.Name = "Test - " + newGuid.ToString().Substring(0,4);
                        newChar.RaceType = RaceType.Mensch;
                        newChar.CreatedBy = "System";
                        newChar.Owner = "Testuser";
                        newChar.Id = newGuid;

                        var currentAppVersion = VersionTracking.CurrentVersion;
                        await _characterRepository.AddItemRawAsync(new CharacterEntity()
                        {
                            CreatedAt = DateTime.Now,
                            Value = newChar,
                            Name = newChar.Name,
                            LastModifiedAt = DateTime.Now,
                            Id = newGuid,
                            Version = currentAppVersion
                        });
                        await RefreshCharacterList();
                        await Task.Delay(250);
                    }
                });
            }));

        public ObservableCollection<LogEntry> WikiParseLog { get; set; } = new ObservableCollection<LogEntry>();

        private async Task InitLocalDatabase()
        {
            try
            {
                await _rangedWeaponRepository.EnsureTables();
                await _meleeWeaponRepository.EnsureTables();
                await _armorRepository.EnsureTables();
                await _talentRepository.EnsureTables();
                await _specialWeaponRepository.EnsureTables();
                await _shieldRepository.EnsureTables();
                await _masteryRepository.EnsureTables();
                await _characterRepository.EnsureTables();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                throw;
            }

            var allTableTypes = (TableInfoType[])Enum.GetValues(typeof(TableInfoType));
            var list = allTableTypes.Select(tableInfoType => new TableInfoModel(tableInfoType)).ToList();
            TableInfos = list.ToDictionary(model => model.Type, model => model);

            await RefreshDatabaseInfos();
            await RefreshCharacterList();
        }

        private async Task RefreshCharacterList()
        {
            var characterEntities = await _characterRepository.GetAllItemsRawAsync();
            Characters = new ObservableCollection<CharacterEntity>(characterEntities.OrderByDescending(entity => entity.LastModifiedAt));
        }

        private async Task RefreshDatabaseInfos()
        {
            foreach (var tableInfo in TableInfos.Values)
            {
                switch (tableInfo.Type)
                {
                    case TableInfoType.Armor:
                        tableInfo.Count = await _armorRepository.GetItemsCount();
                        tableInfo.TimeStamp = _armorRepository.GetLastChangedDate();
                        break;
                    case TableInfoType.MeleeWeapons:
                        tableInfo.Count = await _meleeWeaponRepository.GetItemsCount();
                        tableInfo.TimeStamp = _meleeWeaponRepository.GetLastChangedDate();
                        break;
                    case TableInfoType.RangedWeapons:
                        tableInfo.Count = await _rangedWeaponRepository.GetItemsCount();
                        tableInfo.TimeStamp = _rangedWeaponRepository.GetLastChangedDate();
                        break;
                    case TableInfoType.SpecialWeapons:
                        tableInfo.Count = await _specialWeaponRepository.GetItemsCount();
                        tableInfo.TimeStamp = _specialWeaponRepository.GetLastChangedDate();
                        break;
                    case TableInfoType.Shields:
                        tableInfo.Count = await _shieldRepository.GetItemsCount();
                        tableInfo.TimeStamp = _shieldRepository.GetLastChangedDate();
                        break;
                    case TableInfoType.Talents:
                        tableInfo.Count = await _talentRepository.GetItemsCount();
                        tableInfo.TimeStamp = _talentRepository.GetLastChangedDate();
                        break;
                    case TableInfoType.Masteries:
                        tableInfo.Count = await _masteryRepository.GetItemsCount();
                        tableInfo.TimeStamp = _masteryRepository.GetLastChangedDate();
                        break;
                    case TableInfoType.Character:
                        tableInfo.Count = await _characterRepository.GetItemsCount();
                        tableInfo.TimeStamp = _characterRepository.GetLastChangedDate();
                        break;
                }

                if (tableInfo.Count == 0)
                    tableInfo.State = TableInfoState.NoData;
                else
                    tableInfo.State = TableInfoState.Okay;

                if(tableInfo.TimeStamp == null)
                    tableInfo.State = TableInfoState.NoDatebaseFile;
            }
        }

        public Dictionary<TableInfoType, TableInfoModel> TableInfos
        {
            get => _tableInfos;
            set => SetProperty(ref _tableInfos, value);
        }


    }
}