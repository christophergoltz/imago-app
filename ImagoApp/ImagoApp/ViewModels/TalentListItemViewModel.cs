using System;

namespace ImagoApp.ViewModels
{
    public class TalentListItemViewModel : Util.BindableBase
    {
        private bool _inUse;
        private int? _difficultyOverride;
        public event EventHandler TalentValueChanged;

        public Models.TalentBase Talent { get; }

        public TalentListItemViewModel(Models.TalentBase talent)
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
