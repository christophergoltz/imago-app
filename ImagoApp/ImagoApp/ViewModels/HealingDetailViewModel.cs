using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ImagoApp.Application;
using ImagoApp.Application.Constants;
using ImagoApp.Application.Models;
using ImagoApp.Shared.Enums;
using Xamarin.Forms;

namespace ImagoApp.ViewModels
{
    public class HealingDetailViewModel : BindableBase
    {
        public CharacterViewModel CharacterViewModel
        {
            get => _characterViewModel;
            set => SetProperty(ref _characterViewModel, value);
        }

        public event EventHandler CloseRequested;

        public HealingDetailViewModel(CharacterViewModel characterViewModel)
        {
            CharacterViewModel = characterViewModel;
            CloseCommand = new Command(() => { CloseRequested?.Invoke(this, EventArgs.Empty); });
            DamagedBodyParts = CharacterViewModel.CharacterModel.BodyParts
                .Where(bodypart => bodypart.CurrentHitpointsPercentage < 1).ToList();

            if (DamagedBodyParts.Any())
                SelectedBodyPartModel = DamagedBodyParts.First();
        }

        public List<BodyPartModel> DamagedBodyParts
        {
            get => _damagedBodyParts;
            set => SetProperty(ref _damagedBodyParts, value);
        }

        public ICommand CloseCommand { get; set; }

        public BodyPartModel SelectedBodyPartModel
        {
            get => _selectedBodyPartModel;
            set
            {
                SetProperty(ref _selectedBodyPartModel, value);
                OnPropertyChanged(nameof(MissingHitpointsModification));
                TreatmentBonus = 0;
                BodyPartDestroyed = false;
                RecalculateFinalTreatmentValue();
            }
        }

        private int _finalHealingValue;

        public int MissingHitpointsModification => SelectedBodyPartModel?.MissingHitpoints ?? 0 * 5;

        private int _modification;
        private CharacterViewModel _characterViewModel;
        private List<BodyPartModel> _damagedBodyParts;
        private BodyPartModel _selectedBodyPartModel;
        private bool _bodyPartDestroyed;
        private int _treatmentBonus;


        public int Modification
        {
            get => _modification;
            set
            {
                SetProperty(ref _modification, value);
                RecalculateFinalTreatmentValue();
            }
        }

        public int TreatmentBonus
        {
            get => _treatmentBonus;
            set
            {
                SetProperty(ref _treatmentBonus, value);
                RecalculateFinalTreatmentValue();
            }
        }

        public bool BodyPartDestroyed
        {
            get => _bodyPartDestroyed;
            set
            {
                SetProperty(ref _bodyPartDestroyed, value);
                RecalculateFinalTreatmentValue();
            }
        }

        public int FinalHealingValue
        {
            get => _finalHealingValue;
            set => SetProperty(ref _finalHealingValue, value);
        }

        private void RecalculateFinalTreatmentValue()
        {
            var constFw = CharacterViewModel.KonstitutionAttribute.FinalValue;

            constFw -= MissingHitpointsModification;

            if (BodyPartDestroyed)
                constFw -= 10;

            constFw += TreatmentBonus;

            constFw += Modification;

            FinalHealingValue = constFw.GetRoundedValue();
        }

    }
}