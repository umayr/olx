using System;
using olx.Models;

namespace olx
{
    class Program
    {
        static void Main()
        {

            foreach (var category in Ad.Categories)
            {
                DateTime startTime = DateTime.Now;
                var records = (new Fetch()).Begin(category);
                if (!records.HasValue)
                    continue;
                Console.WriteLine("Fetched {0} records in {1} seconds in {2}. {3}",
                    records, (DateTime.Now - startTime).TotalSeconds, category, Environment.NewLine);
            }

            Console.WriteLine("Yay! You just cloned all of OLX's data. \nPress any key to exit.");

        }



    }
}
