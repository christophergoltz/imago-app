using System;
using System.Collections.Generic;
using System.Text;

namespace ImagoApp.Manager
{
    public static class StyleResourceManager
    {
        public static T TryGetValue<T>(string key)
        {
            Xamarin.Forms.Application.Current.Resources.TryGetValue(key, out var value);
            if (value != null)
                return (T) value;
            
            return default(T);
        }

        public static void SetValue(string key, object value)
        {
            //check main resources
            if (Xamarin.Forms.Application.Current.Resources.ContainsKey(key))
            {
                Xamarin.Forms.Application.Current.Resources[key] = value;
                return;
            }

            foreach (var dictionary in Xamarin.Forms.Application.Current.Resources.MergedDictionaries)
            {
                foreach (var VARIABLE in dictionary)
                {
                    
                }

                if (dictionary.ContainsKey(key))
                {
                    dictionary[key] = value;
                    return;
                }
            }
        }
    }
}
