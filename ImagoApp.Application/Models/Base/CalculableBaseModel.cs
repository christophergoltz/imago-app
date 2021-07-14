using Newtonsoft.Json;

namespace ImagoApp.Application.Models.Base
{
    public abstract class CalculableBaseModel : BindableBase
    {
        private double _finalValue;
        
        public double FinalValue
        {
            get => _finalValue;
            set => SetProperty(ref _finalValue, value);
        }
    }
}
