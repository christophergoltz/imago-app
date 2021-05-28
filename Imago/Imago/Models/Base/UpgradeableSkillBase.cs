namespace Imago.Models.Base
{

    public abstract class UpgradeableSkillBase : SkillBase
    {
        private int _experience;
        private int _increaseValue;

        public int IncreaseValue
        {
            get => _increaseValue;
            set => SetProperty(ref _increaseValue , value);
        }

        public int Experience
        {
            get => _experience;
            set => SetProperty(ref _experience , value);
        }
    }
}
