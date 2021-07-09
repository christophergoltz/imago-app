using Newtonsoft.Json;

namespace ImagoApp.Util
{
    public static  class ObjectHelper
    {
        public static T DeepCopy<T>(this T other)
        {
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(other));
        }
    }
}
