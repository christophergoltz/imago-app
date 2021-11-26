using System;
using System.Collections.Generic;
using System.Text;
using ImagoApp.Application;
using ImagoApp.Application.Models;

namespace ImagoApp.ViewModels
{
    public class TalentViewModel : BindableBase
    {
        private readonly CharacterViewModel _characterViewModel;
        public event EventHandler TalentValueChanged;

        public TalentViewModel(List<TalentModel> talents, CharacterViewModel characterViewModel)
        {
            _characterViewModel = characterViewModel;
            InitializeTalents(talents);
        }

        private List<TalentListItemViewModel> _talents;
        public List<TalentListItemViewModel> Talents
        {
            get => _talents;
            set => SetProperty(ref _talents, value);
        }

        public int GetDifficultyValue()
        {
            var result = 0;

            foreach (var talent in Talents)
            {
                if (!talent.Available)
                    continue;

                if (talent.Talent.ActiveUse == false || talent.Talent.ActiveUse && talent.InUse)
                    result += talent.Talent.Difficulty ?? talent.DifficultyOverride ?? 0;
            }

            return result;
        }

        private void InitializeTalents(List<TalentModel> talents)
        {
            //talents
            var talentViewModels = new List<TalentListItemViewModel>();
            foreach (var talent in talents)
            {
                var avaiable = _characterViewModel.CheckTalentRequirement(talent.Requirements);

                var vm = new TalentListItemViewModel(talent)
                {
                    Available = avaiable
                };
                vm.TalentValueChanged += (sender, args) => TalentValueChanged?.Invoke(this, EventArgs.Empty);
                talentViewModels.Add(vm);
            }

            Talents = talentViewModels;
        }
    }
}
