using ImagoApp.Application.Services;
using ImagoApp.Shared.Enums;

namespace ImagoApp.Application.Models.Base
{
    public abstract class DependentBaseModel : IncreasableBaseModel
    {
        public DependentBaseModel(IncreaseType increaseType) : base(increaseType)
        {
            
        }

        private int _baseValue;
        
        public int BaseValue
        {
            get => _baseValue;
            set => SetProperty(ref _baseValue, value);
        }
    }
}
