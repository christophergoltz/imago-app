using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
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
        private readonly ICharacterRepository _characterRepository;
        private readonly ICharacterService _characterService;
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

        public ICommand ParseDataFromWikiCommand { get; }
        public ICommand OpenCharacterCommand { get; }
        public ICommand TestCharacterCommand { get; }
        public ICommand NewCharacterCommand { get; }

        public StartPageViewModel(ICharacterRepository characterRepository,
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

            _characterRepository = characterRepository;
            _characterService = characterService;
            _meleeWeaponRepository = meleeWeaponRepository;
            _rangedWeaponRepository = rangedWeaponRepository;
            _armorRepository = armorRepository;
            _talentRepository = talentRepository;
            _specialWeaponRepository = specialWeaponRepository;
            _shieldRepository = shieldRepository;
            _masteryRepository = masteryRepository;
            _ruleRepository = ruleRepository;
            
            TestCharacterCommand = new Command(async () =>
            {
                var newGuid = Guid.NewGuid();

                var newChar = _characterRepository.CreateExampleCharacter();
                newChar.Name = "Testspieler";
                newChar.RaceType = RaceType.Mensch;
                newChar.CreatedBy = "System";
                newChar.Owner = "Testuser";
                newChar.Id = newGuid;
                
                var x = VersionTracking.CurrentVersion;

                Version xx = Version.Parse(x);

                var versionString = xx.ToString();
                await _characterRepository.AddItemRawAsync(new CharacterEntity()
                {
                    CreatedAt = DateTime.Now,
                    Value = newChar,
                    Name = newChar.Name,
                    LastModifiedAt = DateTime.Now,
                    Id = newGuid,
                    Version = versionString
                });
                 await RefreshCharacterList();
            });

            ParseDataFromWikiCommand = new Command(async () =>
            {
                WikiParseLog.Clear();

                foreach (var tableInfoModel in TableInfos.Values)
                {
                    if(tableInfoModel.Type == TableInfoType.Character)
                        continue;
                    
                    try
                    {
                        tableInfoModel.State = TableInfoState.Loading;
                        await Task.Delay(200);
                        var result = await wikiParseService.RefreshWikiData(tableInfoModel.Type, WikiParseLog);
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
            });

#pragma warning disable 4014
            InitLocalDatabase(); //needs to be executed in background
#pragma warning restore 4014

            OpenCharacterCommand = new Command<CharacterEntity>(async entity =>
            {
                var c = entity.Value;
                var vm = new CharacterViewModel(c, _ruleRepository);
                _characterService.SetCurrentCharacter(vm);
                await Shell.Current.GoToAsync($"//{nameof(CharacterInfoPage)}");
            });

            NewCharacterCommand = new Command(async () =>
            {
                var newGuid = Guid.NewGuid();

                var newChar = _characterRepository.CreateNewCharacter();
                newChar.Name = newGuid.ToString();
                newChar.RaceType = RaceType.Mensch;
                newChar.Id = newGuid;

                var x = VersionTracking.CurrentVersion;

                Version xx = Version.Parse(x);

                var versionString = xx.ToString();

                var entity = new CharacterEntity()
                {
                    CreatedAt = DateTime.Now,
                    Value = newChar,
                    Name = newChar.Name,
                    LastModifiedAt = DateTime.Now,
                    Id = newGuid,
                    Version = versionString
                };
                await _characterRepository.AddItemRawAsync(entity);
                await RefreshCharacterList();


                //use different open method
                var c = entity.Value;
                var vm = new CharacterViewModel(c, _ruleRepository);
                _characterService.SetCurrentCharacter(vm);
                await Shell.Current.GoToAsync($"//{nameof(CharacterInfoPage)}");

                Element ce = Shell.Current.CurrentPage;

                while (true)
                {
                    if (ce is AppShell shell)
                    {
                        if (shell.BindingContext is AppShellViewModel appShellViewModel)
                        {
                            appShellViewModel.EditMode = true;
                            break;
                        }
                    }
                    else
                    {
                        ce = ce.Parent;
                    }
                }


            });
        }

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