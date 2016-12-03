using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using JodelAPI.Json;
using Newtonsoft.Json;

namespace JodelAPI
{
    public static class GMaps
    {
        /// <summary>
        /// Gets a Location from an address string using Google Maps API
        /// </summary>
        /// <param name="address">Address search string</param>
        /// <returns>
        /// [0] Latitude of location
        /// [1] Longitude of location
        /// [2] Locality
        /// [3] Country
        /// </returns>
        public static string[] GetLocationFromAddress(this string address)
        {
            string api = "https://maps.googleapis.com/maps/api/geocode/json?address=" + address.Replace(" ", "+");

            WebClient client = new WebClient
            {
                Encoding = Encoding.UTF8
            };
            string stringJson = client.DownloadString(api);

            var gCoords = JsonConvert.DeserializeObject<JsonGCoordinates.RootObject>(stringJson);

            if (gCoords.status == "ZERO_RESULTS")
            {
                throw new LocationNotFoundException("Location has not been found.");
            }

            return new[]
            {
                gCoords.results[0].geometry.location.lat.ToString(CultureInfo.InvariantCulture),
                gCoords.results[0].geometry.location.lng.ToString(CultureInfo.InvariantCulture),
                gCoords.results[0].address_components.First(a => a.types[0] == "locality" && a.types[1] == "political")?.short_name,
                gCoords.results[0].address_components.First(a => a.types[0] == "country" && a.types[1] == "political")?.short_name
            };
        }
    }
}