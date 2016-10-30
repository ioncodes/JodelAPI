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
            Console.WriteLine(jodels[1].Message);
            var jodels1 = GetAsync(jodel);
            Console.WriteLine(jodels1.Result[1].Message);
            Console.Read();
        }

        static async Task<List<Jodels>> GetAsync(Jodel jodel)
        {
            var t = await jodel.GetAllJodelsAsync();
            return t;
        }
    }
}
