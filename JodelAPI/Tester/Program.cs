using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using JodelAPI;

namespace Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            Account.AccessToken = "b8ff0042-00f0-4437-a8f1-b2fece566a74";
            Account.Latitude = "47.4618072509873";
            Account.Longitude = "8.329452514657341";
            Account.City = "Berlin";
            Account.CountryCode = "DE";

            Console.Read();

            var jodels = Jodel.GetAllJodels();

            //Console.WriteLine("Getting Flagged Jodels and flaging jodel:");
            //var list = API.GetModerationQueue();
            //Console.WriteLine(list[0].Message);
            //API.FlagJodel(list[0].TaskId, API.Decision.Allow);
            Console.WriteLine("\nGetting coords and setting them as current location:");
            var coords = Location.GetCoordinates("Berliner 2, Deutschland");
            Console.WriteLine("Latitude: " + coords.Latitude + ", Longitude: " + coords.Longitude);
            Console.WriteLine(Location.CalcDistance(coords, Location.GetCoordinates("hamburg, Deutschland"), Location.Unit.Meters).ToString(CultureInfo.InvariantCulture));
            // Console.WriteLine("Reporting Jodel");
            // API.ReportJodel(jodels[0].PostId, API.Reason.Mobbing);comcod
            Console.WriteLine("Getting my Jodels");
            var myJodels = Jodel.GetMyJodels();
            Console.WriteLine(myJodels[0].Message);
            Console.WriteLine("Getting my comments");
            var myComments = Jodel.GetMyComments();
            Console.WriteLine(myComments[0].Message);
            Console.WriteLine("Getting my votes");
            var myVotes = Jodel.GetMyVotes();
            Console.WriteLine("Jodel: " + myVotes[0].Message);
            //Console.WriteLine("Upvoting Jodel");
            //API.Upvote(jodels[0].PostId);
            Console.WriteLine("Sorting Jodel [TOP]");
            var sorted = Jodel.Sort(jodels, Jodel.SortMethod.Top);
            Console.WriteLine("1# Top Jodel: " + sorted[0].Message);
            Console.WriteLine("Sorting Jodel [COMMENTS]");
            sorted = Jodel.Sort(jodels, Jodel.SortMethod.MostCommented);
            Console.WriteLine("1# Commented Jodel: " + sorted[0].Message);
            //Console.WriteLine("Posting Jodel...");
            //Jodel.PostJodel("Test", Jodel.PostColor.Blue);
            //Console.WriteLine("Pinning Jodel...");
            //Jodel.PinJodel(jodels[0].PostId);
            Console.WriteLine("Get my pinned Jodels...");
            var pinned = Jodel.GetMyPins();
            Console.WriteLine(pinned[0].Message);
            Console.WriteLine("Following channel");
            Jodel.Channels.FollowChannel("jhj");

            Console.Read();
            Console.Read();
        }
    }
}
