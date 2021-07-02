using Newtonsoft.Json;

namespace ImagoApp.Models.Base
{
    public abstract class CalculableBase : Util.BindableBase
    {
        private double _finalValue;

        [JsonIgnore]
        public double FinalValue
        {
            get => _finalValue;
            set => SetProperty(ref _finalValue, value);
        }
    }
}
