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
            API.AccessToken = "6bad61c2-adc6-48e2-81aa-8f59c08f1775";
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
            Console.WriteLine("Reporting Jodel");
            var jodels = API.GetAllJodels();
            API.ReportJodel(jodels[0].PostId, API.Reason.Mobbing);
            Console.WriteLine("Getting my Jodels");
            var myJodels = API.GetMyJodels();
            Console.WriteLine(myJodels[0].Message);
            Console.Read();
        }
    }
}
