using System;

namespace olx.Helpers
{
    class Util
    {
        public static string GetClassXPath(string className)
        {
            return String.Format("//*[contains(concat(\" \", normalize-space(@class), \" \"), \" {0} \")]", className);
        }
        public static string GetIdXPath(string id)
        {
            return String.Format("//*[contains(concat(\" \", normalize-space(@id), \" \"), \" {0} \")]", id);
        }
        private static readonly Random Random = new Random((int)DateTime.Now.Ticks);

        public static long LongBetween(long maxValue, long minValue)
        {
            return (long)Math.Round(Random.NextDouble() * (maxValue - minValue - 1)) + minValue;
        }

        public static string GetUrl(string category, int pageNumber = 1)
        {
            return String.Format(Properties.Settings.Default.BaseUrl + "{0}/?search%5Bphotos%5D=false&page={1}", category, pageNumber);
        }

    }
}