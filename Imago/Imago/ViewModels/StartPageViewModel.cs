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
using Xamarin.Forms;

namespace Imago.ViewModels
{
    public class StartPageViewModel : BindableBase
    {
        private readonly ICharacterRepository _characterRepository;
        private readonly IWikiParseService _wikiParseService;
        private readonly IMeleeWeaponRepository _meleeWeaponRepository;
        private readonly IRangedWeaponRepository _rangedWeaponRepository;
        private readonly IArmorRepository _armorRepository;
        private readonly ITalentRepository _talentRepository;
        private readonly ISpecialWeaponRepository _specialWeaponRepository;
        private readonly IShieldRepository _shieldRepository;
        private readonly IMasteryRepository _masteryRepository;
        private Dictionary<TableInfoType, TableInfoModel> _tableInfos;

        public ICommand ParseDataFromWikiCommand { get; }

        public StartPageViewModel(ICharacterRepository characterRepository,
            IWikiParseService wikiParseService,
            IMeleeWeaponRepository meleeWeaponRepository,
            IRangedWeaponRepository rangedWeaponRepository,
            IArmorRepository armorRepository,
            ITalentRepository talentRepository,
            ISpecialWeaponRepository specialWeaponRepository,
            IShieldRepository shieldRepository,
            IMasteryRepository masteryRepository)
        {
            _characterRepository = characterRepository;
            _wikiParseService = wikiParseService;
            _meleeWeaponRepository = meleeWeaponRepository;
            _rangedWeaponRepository = rangedWeaponRepository;
            _armorRepository = armorRepository;
            _talentRepository = talentRepository;
            _specialWeaponRepository = specialWeaponRepository;
            _shieldRepository = shieldRepository;
            _masteryRepository = masteryRepository;
            TestCharacterCommand = new Command(OnTestCharacterClicked);

            ParseDataFromWikiCommand = new Command(async () =>
            {
                WikiParseLog.Clear();

                foreach (var tableInfoModel in TableInfos.Values)
                {
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
            });

#pragma warning disable 4014
            InitLocalDatabase(); //needs to be executed in background
#pragma warning restore 4014
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

        public Command TestCharacterCommand { get; }

        private async void OnTestCharacterClicked(object obj)
        {
            var newChar = _characterRepository.CreateNewCharacter();

            newChar.Name = "Testspieler";
            newChar.RaceType = RaceType.Mensch;
            newChar.CreatedBy = "System";
            newChar.Owner = "Testuser";

            //unlock flyout
            if (Application.Current.MainPage is AppShell app)
                app.FlyoutBehavior = FlyoutBehavior.Locked;

            App.CurrentCharacter = newChar;
            await Shell.Current.GoToAsync($"//{nameof(CharacterInfoPage)}");
        }
    }
}