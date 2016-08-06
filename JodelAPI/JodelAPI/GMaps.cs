using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace JodelAPI
{
    static class GMaps
    {
        public static string[] ToCoordinates(this string address)
        {
            string api = "https://maps.googleapis.com/maps/api/geocode/json?address=" + address.Replace(" ", "+");

            WebClient client = new WebClient
            {
                Encoding = Encoding.UTF8
            };
            string stringJson = client.DownloadString(api);

            var gCoords = JsonConvert.DeserializeObject<JsonGCoordinates.RootObject>(stringJson);

            switch (gCoords.status)
            {
                case "ZERO_RESULTS":
                    throw new LocationNotFoundException("Location has not been found.");
                case "REQUEST_DENIED":
                    throw new GoogleApiTokenInvalidException("Invalid Google API Token.");
            }

            return new[]
            {
                gCoords.results[0].geometry.location.lat.ToString(CultureInfo.InvariantCulture),
                gCoords.results[0].geometry.location.lng.ToString(CultureInfo.InvariantCulture)
            };
        }
    }
}
