using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Imago.Database;
using Imago.Models;
using Imago.Models.Entity;
using Imago.Models.Enum;
using Imago.Repository;
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
        private readonly IWrappingRepository<Weapon, WeaponEntity> _meleeWeaponRepository;
        private readonly IWrappingRepository<Weapon, WeaponEntity> _rangedWeaponRepository;
        private readonly IWrappingRepository<ArmorSet, ArmorSetEntity> _armorRepository;
        private Dictionary<TableInfoType, TableInfoModel> _tableInfos;

        public ICommand ParseDataFromWikiCommand { get; }

        public StartPageViewModel(ICharacterRepository characterRepository, 
            IWikiParseService wikiParseService, 
            IWrappingRepository<Weapon, WeaponEntity> meleeWeaponRepository,
            IWrappingRepository<Weapon, WeaponEntity> rangedWeaponRepository,
            IWrappingRepository<ArmorSet, ArmorSetEntity> armorRepository)
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
                    tableInfoModel.State = TableInfoState.Loading;
                    await _wikiParseService.RefreshWikiData(tableInfoModel.Type);
                    tableInfoModel.State = TableInfoState.Okay;
                }

                await GetAllTableInfos();
            });

            GetAllTableInfos();
        }

        private async Task GetAllTableInfos()
        {
            try
            {
                foreach (var tableInfoType in ((TableInfoType[])Enum.GetValues(typeof(TableInfoType))))
                {
                    switch (tableInfoType)
                    {
                        case TableInfoType.Armor:
                            break;
                        case TableInfoType.MeleeWeapons:
                            break;
                        case TableInfoType.RangedWeapons:
                            break;
                        case TableInfoType.SpecialWeapons:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }


                var database = await DatabaseInfoRepository.Instance;
                var data = await database.GetAllTableInfos();
                TableInfos = data.ToDictionary(model => model.Type, model => model);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
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
