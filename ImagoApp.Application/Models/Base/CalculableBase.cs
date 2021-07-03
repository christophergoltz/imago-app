using ImagoApp.Util;
using Newtonsoft.Json;

namespace ImagoApp.Application.Models.Base
{
    public abstract class CalculableBase : BindableBase
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
