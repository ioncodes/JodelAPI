using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JodelAPI;

namespace Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            API.AccessToken = "f863e61d-3548-4d6d-bba7-15253bdc23f6";
            API.Latitude = "52.30";
            API.Longitude = "13.25";
            API.City = "Berlin";
            API.CountryCode = "DE";

            Console.WriteLine("Getting Flagged Jodels and flaging jodel:");
            var list = API.GetModerationQueue();
            Console.WriteLine(list[0].Message);
            API.FlagJodel(list[0].TaskId, API.Decision.Allow);
            Console.WriteLine("\nGetting coords and setting them as current location:");
            var coords = API.GetCoordinates("Berliner 2, Deutschland");
            Console.WriteLine("Latitude: " + coords.Latitude + ", Longitude: " + coords.Longitude);
            Console.WriteLine(API.CalcDistance(coords, API.GetCoordinates("hamburg, Deutschland"), API.Unit.Meters).ToString(CultureInfo.InvariantCulture));
            Console.Read();
        }
    }
}
