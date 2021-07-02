using System.Windows.Input;
using Xamarin.Forms;

namespace ImagoApp.ViewModels
{
    public class StatusPageViewModel : Util.BindableBase
    {
        private readonly Repository.WrappingDatabase.IMeleeWeaponRepository _meleeWeaponRepository;
        private readonly Repository.WrappingDatabase.IRangedWeaponRepository _rangedWeaponRepository;
        private readonly Repository.WrappingDatabase.ISpecialWeaponRepository _specialWeaponRepository;
        private readonly Repository.WrappingDatabase.IShieldRepository _shieldRepository;
        private WeaponDetailViewModel _weaponDetailViewModel;
        public CharacterViewModel CharacterViewModel { get; }
        
        public WeaponDetailViewModel WeaponDetailViewModel
        {
            get => _weaponDetailViewModel;
            set => SetProperty(ref _weaponDetailViewModel, value);
        }

        public WeaponListViewModel WeaponListViewModel { get; set; }

        public ICommand OpenWeaponCommand { get; set; }

        public StatusPageViewModel(CharacterViewModel characterViewModel, Repository.WrappingDatabase.IArmorRepository armorRepository,
            Repository.WrappingDatabase.IMeleeWeaponRepository meleeWeaponRepository, Repository.WrappingDatabase.IRangedWeaponRepository rangedWeaponRepository, Repository.WrappingDatabase.ISpecialWeaponRepository specialWeaponRepository,
            Repository.WrappingDatabase.IShieldRepository shieldRepository)
        {
            _meleeWeaponRepository = meleeWeaponRepository;
            _rangedWeaponRepository = rangedWeaponRepository;
            _specialWeaponRepository = specialWeaponRepository;
            _shieldRepository = shieldRepository;
            CharacterViewModel = characterViewModel;
            

            KopfViewModel = new BodyPartArmorListViewModel(characterViewModel, armorRepository, CharacterViewModel.Character.BodyParts[Models.Enum.BodyPartType.Kopf]);
            TorsoViewModel = new BodyPartArmorListViewModel(characterViewModel, armorRepository, CharacterViewModel.Character.BodyParts[Models.Enum.BodyPartType.Torso]);
            ArmLinksViewModel = new BodyPartArmorListViewModel(characterViewModel, armorRepository, CharacterViewModel.Character.BodyParts[Models.Enum.BodyPartType.ArmLinks]);
            ArmRechtsViewModel = new BodyPartArmorListViewModel(characterViewModel, armorRepository, CharacterViewModel.Character.BodyParts[Models.Enum.BodyPartType.ArmRechts]);
            BeinLinksViewModel = new BodyPartArmorListViewModel(characterViewModel, armorRepository, CharacterViewModel.Character.BodyParts[Models.Enum.BodyPartType.BeinLinks]);
            BeinRechtsViewModel = new BodyPartArmorListViewModel(characterViewModel, armorRepository, CharacterViewModel.Character.BodyParts[Models.Enum.BodyPartType.BeinRechts]);

            WeaponListViewModel = new WeaponListViewModel(characterViewModel, _meleeWeaponRepository,
                _rangedWeaponRepository, _specialWeaponRepository, _shieldRepository);
            WeaponListViewModel.OpenWeaponRequested += (sender, weapon) => OpenWeaponCommand?.Execute(weapon);

            OpenWeaponCommand = new Command<Models.Weapon>(weapon =>
            {
                var vm = new WeaponDetailViewModel(weapon, CharacterViewModel);
                vm.CloseRequested += (sender, args) => WeaponDetailViewModel = null;
                vm.RemoveWeaponRequested += (sender, args) =>
                {
                    CharacterViewModel.Character.Weapons.Remove(weapon);
                    CharacterViewModel.RecalculateHandicapAttributes();
                    WeaponDetailViewModel = null;
                };
                WeaponDetailViewModel = vm;
            });
        }

        public BodyPartArmorListViewModel KopfViewModel { get; set; }
        public BodyPartArmorListViewModel TorsoViewModel { get; set; }
        public BodyPartArmorListViewModel ArmLinksViewModel { get; set; }
        public BodyPartArmorListViewModel ArmRechtsViewModel { get; set; }
        public BodyPartArmorListViewModel BeinLinksViewModel { get; set; }
        public BodyPartArmorListViewModel BeinRechtsViewModel { get; set; }
    }
}
