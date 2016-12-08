using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JodelAPI.Json.Request
{
    public class JsonRequestGenerateAccessToken : JsonRequest
    {
        public string device_uid { get; set; }
        public Location location { get; set; }
        public string client_id { get; set; }

        public class Location
        {
            public string City { get; set; }
            public double loc_accuracy { get; set; }
            public Coordiantes loc_coordinates { get; set; }
            public string country { get; set; }

            public class Coordiantes
            {
                public double lat { get; set; }
                public double lng { get; set; }
            }
        }
    }
}
