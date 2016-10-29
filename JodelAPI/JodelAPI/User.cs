using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JodelAPI
{
    public class User
    {
        public string AccessToken { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string CountryCode { get; set; }
        public string City { get; set; }
        public string GoogleApiToken { get; set; }
    }
}
