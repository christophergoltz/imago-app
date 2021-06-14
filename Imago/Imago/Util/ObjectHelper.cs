using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Imago.Util
{
    public static  class ObjectHelper
    {
        public static T DeepCopy<T>(this T other)
        {
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(other));
        }
    }
}
