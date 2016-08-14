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
            API.Latitude = "47.4618072509873";
            API.Longitude = "8.329452514657341";
            API.City = "Berlin";
            API.CountryCode = "DE";

            Console.Read();

            var jodels = API.GetAllJodels();

            Console.WriteLine("Getting Flagged Jodels and flaging jodel:");
            var list = API.GetModerationQueue();
            Console.WriteLine(list[0].Message);
            API.FlagJodel(list[0].TaskId, API.Decision.Allow);
            Console.WriteLine("\nGetting coords and setting them as current location:");
            var coords = API.GetCoordinates("Berliner 2, Deutschland");
            Console.WriteLine("Latitude: " + coords.Latitude + ", Longitude: " + coords.Longitude);
            Console.WriteLine(API.CalcDistance(coords, API.GetCoordinates("hamburg, Deutschland"), API.Unit.Meters).ToString(CultureInfo.InvariantCulture));
            // Console.WriteLine("Reporting Jodel");
            // API.ReportJodel(jodels[0].PostId, API.Reason.Mobbing);
            Console.WriteLine("Getting my Jodels");
            var myJodels = API.GetMyJodels();
            Console.WriteLine(myJodels[0].Message);
            Console.WriteLine("Getting my comments");
            var myComments = API.GetMyComments();
            Console.WriteLine(myComments[0].Message);
            Console.WriteLine("Getting my votes");
            var myVotes = API.GetMyVotes();
            Console.WriteLine("Jodel: " + myVotes[0].Message);
            Console.WriteLine("Upvoting Jodel");
            API.Upvote(jodels[0].PostId);
            Console.WriteLine("Sorting Jodel [TOP]");
            var sorted = API.Sort(jodels, API.SortMethod.Top);
            Console.WriteLine("1# Top Jodel: " + sorted[0].Message);
            Console.WriteLine("Sorting Jodel [COMMENTS]");
            sorted = API.Sort(jodels, API.SortMethod.MostCommented);
            Console.WriteLine("1# Commented Jodel: " + sorted[0].Message);

            Console.Read();
            Console.Read();
        }
    }
}
