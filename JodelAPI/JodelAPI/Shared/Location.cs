using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using JodelAPI.Json.Response;
using Newtonsoft.Json;

namespace JodelAPI.Shared
{
    public class Location
    {
        #region Fields and Properties

        public string Place { get; private set; }

        public double Longitude { get; private set; }

        public double Latitude { get; private set; }

        #endregion

        #region Constructor

        internal Location(string place)
        {
            this.Place = place;

            FindCoordinates();
        }

        #endregion

        #region Methods

        public void SetNewPlace(string place)
        {
            this.Place = place;
            FindCoordinates();
        }

        public void SetNewPlace(double lat,double lng)
        {
            Latitude = lat;
            Longitude = lng;
        }

        private void FindCoordinates()
        {
            string api = "https://maps.googleapis.com/maps/api/geocode/json?address=" + Place.Replace(" ", "+");

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

            Latitude = gCoords.results[0].geometry.location.lat;
            Longitude = gCoords.results[0].geometry.location.lng;
        }

        #endregion
    }
}
