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
        public CharacterViewModel CharacterViewModel { get; private set; }
        public event EventHandler CloseRequested;

        public TreatmentDetailViewModel(CharacterViewModel characterViewModel)
        {
            CharacterViewModel = characterViewModel;
            CharacterViewModel.LoadoutViewModel.LoadoutValueChanged += (sender, args) => RecalculateFinalTreatmentValue();

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
        
        private int _finalTreatmentValue;

        public List<BodyPartDamageStateModel> BodyPartDamageStates
        {
            get => _bodyPartDamageStates;
            set => SetProperty(ref _bodyPartDamageStates, value);
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
        private List<BodyPartDamageStateModel> _bodyPartDamageStates;
        private int _bodyPartDamageStatusMalus;


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
                (DerivedAttributeType.BehinderungKampf, "Kampf", "Images/kampf.png"),
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

        public int BodyPartDamageStatusMalus
        {
            get => _bodyPartDamageStatusMalus;
            set => SetProperty(ref _bodyPartDamageStatusMalus, value);
        }

        private void RecalculateFinalTreatmentValue()
        {
            if (SelectedTreatmentSkillModel == null)
                return;

            var result = SelectedTreatmentSkillModel.FinalValue.GetRoundedValue();

            //handicap
            result -= CharacterViewModel.LoadoutViewModel.GetLoadoutValue();
       
            BodyPartDamageStatusMalus = 0;

            //damage status
            if (BodyPartDamageStates != null)
            {
                foreach (var damageStatus in BodyPartDamageStates)
                {
                    switch (damageStatus.StateType)
                    {
                        case BodyPartDamageStateType.Damaged:
                            BodyPartDamageStatusMalus += 30;
                            continue;
                        case BodyPartDamageStateType.Broken:
                            BodyPartDamageStatusMalus += 45;
                            continue;
                        case BodyPartDamageStateType.Normal:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(BodyPartDamageStateType));
                    }
                }
            }

            result -= BodyPartDamageStatusMalus;

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
            //damage status
            var bodyPartDamageStates = new List<BodyPartDamageStateModel>()
            {
                new BodyPartDamageStateModel(BodyPartType.Kopf),
                new BodyPartDamageStateModel(BodyPartType.Torso),
                new BodyPartDamageStateModel(BodyPartType.ArmLinks),
                new BodyPartDamageStateModel(BodyPartType.ArmRechts),
                new BodyPartDamageStateModel(BodyPartType.BeinLinks),
                new BodyPartDamageStateModel(BodyPartType.BeinRechts)
            };

            foreach (var bodyPartDamageState in bodyPartDamageStates)
            {
                bodyPartDamageState.PropertyChanged += (sender, args) => RecalculateFinalTreatmentValue();
            }

            BodyPartDamageStates = bodyPartDamageStates;

            RecalculateFinalTreatmentValue();
        }
    }
}