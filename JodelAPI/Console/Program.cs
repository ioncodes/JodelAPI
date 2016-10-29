using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JodelAPI;

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
            Console.WriteLine(jodels[1].Message);
            Console.Read();
        }
    }
}
