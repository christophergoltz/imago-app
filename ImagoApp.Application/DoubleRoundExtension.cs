using System;

namespace ImagoApp.Application
{
    public static class DoubleRoundExtension
    {
        public static int GetRoundedValue(this double value)
        {
            return (int)Math.Round(value, MidpointRounding.AwayFromZero);
        }

        public static int GetRoundedValue(this decimal value)
        {
            return (int)Math.Round(value, MidpointRounding.AwayFromZero);
        }
    }
}
