using System;
using System.Collections.Generic;
using System.Text;
using Imago.Models;
using Imago.Util;

namespace Imago.ViewModels
{
    public class TalentListItemViewModel : BindableBase
    {
        private bool _inUse;
        private int? _difficultyOverride;
        public event EventHandler TalentValueChanged;

        public TalentBase Talent { get; }

        public TalentListItemViewModel(TalentBase talent)
        {
            Talent = talent;
        }

        public bool Available
        {
            get => _available;
            set => SetProperty(ref _available, value);
        }
        private bool _available;

        public bool InUse
        {
            get => _inUse;
            set
            {
                SetProperty(ref _inUse, value);
                TalentValueChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public int? DifficultyOverride
        {
            get => _difficultyOverride;
            set
            {
                SetProperty(ref _difficultyOverride, value);
                TalentValueChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
