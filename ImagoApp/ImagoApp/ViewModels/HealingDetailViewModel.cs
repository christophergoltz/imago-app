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
        private readonly CharacterViewModel _characterViewModel;
        public event EventHandler CloseRequested;

        public HealingDetailViewModel(CharacterViewModel characterViewModel)
        {
            _characterViewModel = characterViewModel;
            CloseCommand = new Command(() => { CloseRequested?.Invoke(this, EventArgs.Empty); });

            Task.Run(InitializeHealingView);

            Device.BeginInvokeOnMainThread(() => { });
        }

        public ICommand CloseCommand { get; set; }


        private int _finalHealingValue;


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

        private void InitializeHealingView()
        {

        }


        public int FinalHealingValue
        {
            get => _finalHealingValue;
            set => SetProperty(ref _finalHealingValue, value);
        }

        private void RecalculateFinalTreatmentValue()
        {
            //todo konst FW
            //todo - Mod
        }

    }
}