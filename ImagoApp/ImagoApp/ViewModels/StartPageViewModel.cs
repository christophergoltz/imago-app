using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ImagoApp.ViewModels
{
    public class StartPageViewModel : Util.BindableBase
    {
        private readonly AppShellViewModel _appShellViewModel;
        private readonly Repository.WrappingDatabase.ICharacterRepository _characterRepository;
        private readonly Services.ICharacterService _characterService;
        private readonly Services.IWikiParseService _wikiParseService;
        private readonly Repository.WrappingDatabase.IMeleeWeaponRepository _meleeWeaponRepository;
        private readonly Repository.WrappingDatabase.IRangedWeaponRepository _rangedWeaponRepository;
        private readonly Repository.WrappingDatabase.IArmorRepository _armorRepository;
        private readonly Repository.WrappingDatabase.ITalentRepository _talentRepository;
        private readonly Repository.WrappingDatabase.ISpecialWeaponRepository _specialWeaponRepository;
        private readonly Repository.WrappingDatabase.IShieldRepository _shieldRepository;
        private readonly Repository.WrappingDatabase.IMasteryRepository _masteryRepository;
        private readonly Repository.IRuleRepository _ruleRepository;
        private Dictionary<Models.Enum.TableInfoType, Models.TableInfoModel> _tableInfos;
        private ObservableCollection<Models.Entity.CharacterEntity> _characters;

        public ObservableCollection<Models.Entity.CharacterEntity> Characters
        {
            get => _characters;
            set => SetProperty(ref _characters, value);
        }


        public StartPageViewModel(AppShellViewModel appShellViewModel,
            Repository.WrappingDatabase.ICharacterRepository characterRepository,
            Services.ICharacterService characterService,
            Services.IWikiParseService wikiParseService,
            Repository.WrappingDatabase.IMeleeWeaponRepository meleeWeaponRepository,
            Repository.WrappingDatabase.IRangedWeaponRepository rangedWeaponRepository,
            Repository.WrappingDatabase.IArmorRepository armorRepository,
            Repository.WrappingDatabase.ITalentRepository talentRepository,
            Repository.WrappingDatabase.ISpecialWeaponRepository specialWeaponRepository,
            Repository.WrappingDatabase.IShieldRepository shieldRepository,
            Repository.WrappingDatabase.IMasteryRepository masteryRepository,
            Repository.IRuleRepository ruleRepository)
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
        }


        private ICommand _parseWikiCommand;
        public ICommand ParseWikiCommand => _parseWikiCommand ?? (_parseWikiCommand = new Command(async () =>
        {
            WikiParseLog.Clear();

            foreach (var tableInfoModel in TableInfos.Values)
            {
                if (tableInfoModel.Type == Models.Enum.TableInfoType.Character)
                    continue;

                try
                {
                    tableInfoModel.State = Models.Enum.TableInfoState.Loading;
                    await Task.Delay(200);
                    var result = await _wikiParseService.RefreshWikiData(tableInfoModel.Type, WikiParseLog);
                    if (result == null)
                    {
                        tableInfoModel.State = Models.Enum.TableInfoState.Error;
                        await Task.Delay(200);
                        continue;
                    }

                    if (result.Value == 0)
                    {
                        tableInfoModel.State = Models.Enum.TableInfoState.NoData;
                        await Task.Delay(200);
                        continue;
                    }

                    tableInfoModel.State = Models.Enum.TableInfoState.Okay;
                    await Task.Delay(200);
                }
                catch (Exception e)
                {
                    tableInfoModel.State = Models.Enum.TableInfoState.Error;
                    await Task.Delay(200);
                    Debug.WriteLine(e);
                }

            }

            await RefreshDatabaseInfos();
        }));

        private ICommand _openCharacterCommand;
        public ICommand OpenCharacterCommand => _openCharacterCommand ?? (_openCharacterCommand = new Command<Models.Entity.CharacterEntity>(entity =>
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

        private async Task OpenCharacter(Models.Entity.CharacterEntity characterEntity, bool editMode)
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

            await Device.InvokeOnMainThreadAsync(() =>
            {
               Application.Current.MainPage = new AppShell();
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
                        newChar.RaceType = Models.Enum.RaceType.Mensch;
                        newChar.Id = newGuid;
                        newChar.CreatedAt = DateTime.Now;
                        newChar.LastModifiedAt = DateTime.Now;

                        var currentAppVersion = VersionTracking.CurrentVersion;
                        var entity = new Models.Entity.CharacterEntity()
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
                        newChar.RaceType = Models.Enum.RaceType.Mensch;
                        newChar.CreatedBy = "System";
                        newChar.Owner = "Testuser";
                        newChar.Id = newGuid;

                        var currentAppVersion = VersionTracking.CurrentVersion;
                        await _characterRepository.AddItemRawAsync(new Models.Entity.CharacterEntity()
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

        public ObservableCollection<Util.LogEntry> WikiParseLog { get; set; } = new ObservableCollection<Util.LogEntry>();

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

            var allTableTypes = (Models.Enum.TableInfoType[])Enum.GetValues(typeof(Models.Enum.TableInfoType));
            var list = allTableTypes.Select(tableInfoType => new Models.TableInfoModel(tableInfoType)).ToList();
            TableInfos = list.ToDictionary(model => model.Type, model => model);

            await RefreshDatabaseInfos();
            await RefreshCharacterList();
        }

        private async Task RefreshCharacterList()
        {
            var characterEntities = await _characterRepository.GetAllItemsRawAsync();
            Characters = new ObservableCollection<Models.Entity.CharacterEntity>(characterEntities.OrderByDescending(entity => entity.LastModifiedAt));
        }

        private async Task RefreshDatabaseInfos()
        {
            foreach (var tableInfo in TableInfos.Values)
            {
                switch (tableInfo.Type)
                {
                    case Models.Enum.TableInfoType.Armor:
                        tableInfo.Count = await _armorRepository.GetItemsCount();
                        tableInfo.TimeStamp = _armorRepository.GetLastChangedDate();
                        break;
                    case Models.Enum.TableInfoType.MeleeWeapons:
                        tableInfo.Count = await _meleeWeaponRepository.GetItemsCount();
                        tableInfo.TimeStamp = _meleeWeaponRepository.GetLastChangedDate();
                        break;
                    case Models.Enum.TableInfoType.RangedWeapons:
                        tableInfo.Count = await _rangedWeaponRepository.GetItemsCount();
                        tableInfo.TimeStamp = _rangedWeaponRepository.GetLastChangedDate();
                        break;
                    case Models.Enum.TableInfoType.SpecialWeapons:
                        tableInfo.Count = await _specialWeaponRepository.GetItemsCount();
                        tableInfo.TimeStamp = _specialWeaponRepository.GetLastChangedDate();
                        break;
                    case Models.Enum.TableInfoType.Shields:
                        tableInfo.Count = await _shieldRepository.GetItemsCount();
                        tableInfo.TimeStamp = _shieldRepository.GetLastChangedDate();
                        break;
                    case Models.Enum.TableInfoType.Talents:
                        tableInfo.Count = await _talentRepository.GetItemsCount();
                        tableInfo.TimeStamp = _talentRepository.GetLastChangedDate();
                        break;
                    case Models.Enum.TableInfoType.Masteries:
                        tableInfo.Count = await _masteryRepository.GetItemsCount();
                        tableInfo.TimeStamp = _masteryRepository.GetLastChangedDate();
                        break;
                    case Models.Enum.TableInfoType.Character:
                        tableInfo.Count = await _characterRepository.GetItemsCount();
                        tableInfo.TimeStamp = _characterRepository.GetLastChangedDate();
                        break;
                }

                if (tableInfo.Count == 0)
                    tableInfo.State = Models.Enum.TableInfoState.NoData;
                else
                    tableInfo.State = Models.Enum.TableInfoState.Okay;

                if(tableInfo.TimeStamp == null)
                    tableInfo.State = Models.Enum.TableInfoState.NoDatebaseFile;
            }
        }

        public Dictionary<Models.Enum.TableInfoType, Models.TableInfoModel> TableInfos
        {
            get => _tableInfos;
            set => SetProperty(ref _tableInfos, value);
        }


    }
}