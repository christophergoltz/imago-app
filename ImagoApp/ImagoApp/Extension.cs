using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ImagoApp
{
    public static  class Extension
    {
        public static bool In<T>(this T obj, params T[] args)
        {
            return args.Contains(obj);
        }
    }
}
