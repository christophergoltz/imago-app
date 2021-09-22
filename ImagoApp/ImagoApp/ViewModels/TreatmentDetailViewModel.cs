using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ImagoApp.Application;
using ImagoApp.Application.Models;
using ImagoApp.Shared.Enums;
using Xamarin.Forms;

namespace ImagoApp.ViewModels
{
    public class TreatmentDetailViewModel : BindableBase
    {
        private readonly CharacterViewModel _characterViewModel;
        public event EventHandler CloseRequested;

        public TreatmentDetailViewModel(CharacterViewModel characterViewModel)
        {
            _characterViewModel = characterViewModel;
            CloseCommand = new Command(() =>
            {
                CloseRequested?.Invoke(this, EventArgs.Empty);
            });

            TreatmentSkills = new List<SkillModel>()
            {
                characterViewModel.CharacterModel.SkillGroups.SelectMany(skillGroup => skillGroup.Skills).First(skill => skill.Type == SkillModelType.Wundscher),
                characterViewModel.CharacterModel.SkillGroups.SelectMany(skillGroup => skillGroup.Skills).First(skill => skill.Type == SkillModelType.Heiler)
            };

            Task.Run(InitializeHealingView);

            Device.BeginInvokeOnMainThread(() =>
            {
                SelectedTreatmentSkillModel = TreatmentSkills.First();
                SelectedTreatmentToolLevel = "-";
                SelectedTreatmentMaterialLevel = "-";
            });
        }

        public ICommand CloseCommand { get; set; }

        private List<HandicapListViewItemViewModel> _handicaps;
        private int _finalTreatmentValue;

        public List<HandicapListViewItemViewModel> Handicaps
        {
            get => _handicaps;
            set => SetProperty(ref _handicaps, value);
        }

        private List<HandicapListViewItemViewModel> _damageStatus;
        public List<HandicapListViewItemViewModel> DamageStatus
        {
            get => _damageStatus;
            set => SetProperty(ref _damageStatus, value);
        }

        private SkillModel _selectedTreatmentSkillModel;
        public SkillModel SelectedTreatmentSkillModel
        {
            get => _selectedTreatmentSkillModel;
            set
            {
                SetProperty(ref _selectedTreatmentSkillModel, value);
                RecalculateFinalTreatmentValue();
            }
        }

        private List<SkillModel> _treatmentSkills;
        public List<SkillModel> TreatmentSkills
        {
            get => _treatmentSkills;
            set => SetProperty(ref _treatmentSkills, value);
        }

        public string SelectedTreatmentMaterialLevel
        {
            get => _selectedTreatmentMaterialLevel;
            set
            {
                SetProperty(ref _selectedTreatmentMaterialLevel, value);
                RecalculateFinalTreatmentValue();
            }
        }

        public string SelectedTreatmentToolLevel
        {
            get => _selectedTreatmentToolLevel;
            set
            {
                SetProperty(ref _selectedTreatmentToolLevel, value);
                RecalculateFinalTreatmentValue();
            }
        }
        private int _treatmentBonus;
        private string _selectedTreatmentToolLevel;
        private string _selectedTreatmentMaterialLevel;

        private int _modification;


        public int Modification
        {
            get => _modification;
            set
            {
                SetProperty(ref _modification, value);
                RecalculateFinalTreatmentValue();
            }
        }

        private static readonly List<(DerivedAttributeType Type, string Text, string IconSource)> HandicapDefinition =
            new List<(DerivedAttributeType Type, string Text, string IconSource)>()
            {
                (DerivedAttributeType.BehinderungKampf, "Kampf", "Images/swords.png"),
                (DerivedAttributeType.BehinderungAbenteuer, "Abenteuer / Reise", "Images/inventar.png"),
                (DerivedAttributeType.BehinderungGesamt, "Gesamt", null),
                (DerivedAttributeType.Unknown, "Ignorieren", null)
            };

        public int FinalTreatmentValue
        {
            get => _finalTreatmentValue;
            set => SetProperty(ref _finalTreatmentValue, value);
        }

        public int TreatmentBonus
        {
            get => _treatmentBonus;
            set => SetProperty(ref _treatmentBonus, value);
        }

        private void RecalculateFinalTreatmentValue()
        {
            if (SelectedTreatmentSkillModel == null)
                return;

            var result = SelectedTreatmentSkillModel.FinalValue.GetRoundedValue();

            //handicap
            if (Handicaps != null)
            {
                foreach (var handicap in Handicaps)
                {
                    if (handicap.IsChecked)
                        result -= handicap.HandiCapValue ?? 0;
                }
            }

            //damage status
            if (DamageStatus != null)
            {
                foreach (var damageStatus in DamageStatus)
                {
                    if (damageStatus.IsChecked)
                        result -= damageStatus.HandiCapValue ?? 0;
                }
            }

            //modification
            result += Modification;

            //material
            if (!string.IsNullOrWhiteSpace(SelectedTreatmentMaterialLevel) && !string.IsNullOrWhiteSpace(SelectedTreatmentToolLevel) && SelectedTreatmentMaterialLevel != "-" && SelectedTreatmentToolLevel != "-")
            {
                var material = int.Parse(SelectedTreatmentMaterialLevel);
                var tool = int.Parse(SelectedTreatmentToolLevel);

                var level = Math.Min(material, tool);

                TreatmentBonus = level * 10;
            }
            else
            {
                TreatmentBonus = 0;
            }

            FinalTreatmentValue = result;
        }

        private void InitializeHealingView()
        {
            //handicap
            var handicaps = new List<HandicapListViewItemViewModel>();
            foreach (var (type, text, iconSource) in HandicapDefinition)
            {
                var handicapValue = type == DerivedAttributeType.Unknown
                    ? (int?)null
                    : _characterViewModel.DerivedAttributes.First(attribute => attribute.Type == type).FinalValue.GetRoundedValue();

                //todo converter
                var vm = new HandicapListViewItemViewModel(type, false, handicapValue, iconSource,
                    text);
                vm.HandicapValueChanged += (sender, args) => RecalculateFinalTreatmentValue();

                if (vm.Type == DerivedAttributeType.BehinderungAbenteuer)
                    vm.IsChecked = true;

                handicaps.Add(vm);
            }

            Handicaps = handicaps;

            //damage status
            var damageStatus = new List<HandicapListViewItemViewModel>();
            var normal = new HandicapListViewItemViewModel(DerivedAttributeType.Unknown, true, 0, null, "Normal");
            normal.HandicapValueChanged += (sender, args) => RecalculateFinalTreatmentValue();
            var failed = new HandicapListViewItemViewModel(DerivedAttributeType.Unknown, false, 30, null, "Ausgefallen");
            failed.HandicapValueChanged += (sender, args) => RecalculateFinalTreatmentValue();
            var destroyed = new HandicapListViewItemViewModel(DerivedAttributeType.Unknown, false, 45, null, "Zerstört");
            destroyed.HandicapValueChanged += (sender, args) => RecalculateFinalTreatmentValue();

            damageStatus.Add(normal);
            damageStatus.Add(failed);
            damageStatus.Add(destroyed);

            DamageStatus = damageStatus;


            RecalculateFinalTreatmentValue();
        }




    }
}
