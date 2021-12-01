using System;
using System.Collections.Generic;
using System.Text;
using ImagoApp.Application;
using ImagoApp.Application.Models;
using ImagoApp.Application.Services;

namespace ImagoApp.ViewModels
{
    public class MasteryViewModel : BindableBase
    {
        public event EventHandler MasteryValueChanged; 
        private readonly CharacterViewModel _characterViewModel;

        public MasteryViewModel(List<MasteryModel> masteries, CharacterViewModel characterViewModel)
        {
            _characterViewModel = characterViewModel;
            InitializeMasteries(masteries);
        }

        private List<MasteryListItemViewModel> _masteries;
        public List<MasteryListItemViewModel> Masteries
        {
            get => _masteries;
            set => SetProperty(ref _masteries, value);
        }

        public int GetDifficultyValue()
        {
            var result = 0;

            foreach (var mastery in Masteries)
            {
                if (!mastery.Available)
                    continue;

                if (mastery.Mastery.ActiveUse == false || mastery.Mastery.ActiveUse && mastery.InUse)
                    result += mastery.Mastery.Difficulty ?? mastery.DifficultyOverride ?? 0;
            }

            return result;
        }

        private void InitializeMasteries(List<MasteryModel> masteryModels)
        {
            var masteries = new List<MasteryListItemViewModel>();
            foreach (var mastery in masteryModels)
            {
                var avaiable = _characterViewModel.CheckMasteryRequirement(mastery.Requirements);

                var vm = new MasteryListItemViewModel(mastery)
                {
                    Available = avaiable
                };
                vm.TalentValueChanged += (sender, args) => MasteryValueChanged?.Invoke(this, EventArgs.Empty);
                masteries.Add(vm);
            }

            Masteries = masteries;
        }
    }
}
