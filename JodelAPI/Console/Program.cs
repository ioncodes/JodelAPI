using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JodelAPI;
using JodelAPI.Objects;

namespace JConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var location = Location.GetCoordinates("Baden, Aargau, Schweiz");
            string accessToken = Account.GenerateAccessToken(location.Latitude, location.Longitude, "CH", "Baden").AccessToken;
            User user = new User(accessToken, location.Latitude, location.Longitude, "CH", "Baden");
            Jodel jodel = new Jodel(user);
            var jodels = jodel.GetAllJodels();
            Console.WriteLine(jodels[4].Message);
            //var t = jodel.GetAllJodelsAsync();
           
            //Console.WriteLine(jodels1[1].Message);
            Console.Read();
        }
    }
}
