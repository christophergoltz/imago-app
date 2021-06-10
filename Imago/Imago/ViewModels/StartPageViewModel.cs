using System;
using System.Collections.Generic;
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
        private Dictionary<TableInfoType, TableInfoModel> _tableInfos;

        public ICommand ParseDataFromWikiCommand { get; }

        public StartPageViewModel(ICharacterRepository characterRepository,
            IWikiParseService wikiParseService,
            IMeleeWeaponRepository meleeWeaponRepository,
            IRangedWeaponRepository rangedWeaponRepository,
            IArmorRepository armorRepository)
        {
            _characterRepository = characterRepository;
            _wikiParseService = wikiParseService;
            _meleeWeaponRepository = meleeWeaponRepository;
            _rangedWeaponRepository = rangedWeaponRepository;
            _armorRepository = armorRepository;
            TestCharacterCommand = new Command(OnTestCharacterClicked);

            ParseDataFromWikiCommand = new Command(async () =>
            {
                foreach (var tableInfoModel in TableInfos.Values)
                {
                    try
                    {
                        tableInfoModel.State = TableInfoState.Loading;
                        var result = await _wikiParseService.RefreshWikiData(tableInfoModel.Type);
                        if (result == null)
                        {
                            tableInfoModel.State = TableInfoState.Error;
                            continue;
                        }

                        if (result.Value == 0)
                        {
                            tableInfoModel.State = TableInfoState.NoData;
                            continue;
                        }
                        
                        tableInfoModel.State = TableInfoState.Okay;
                    }
                    catch (Exception e)
                    {
                        tableInfoModel.State = TableInfoState.Error;
                        Debug.WriteLine(e);
                //        throw;
                    }

                }

                await RefreshDatabaseInfos();
            });

#pragma warning disable 4014
            InitLocalDatabase(); //needs to be executed in background
#pragma warning restore 4014
        }

        private async Task InitLocalDatabase()
        {
            try
            {
                await _rangedWeaponRepository.EnsureTables();
                await _meleeWeaponRepository.EnsureTables();
                await _armorRepository.EnsureTables();
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