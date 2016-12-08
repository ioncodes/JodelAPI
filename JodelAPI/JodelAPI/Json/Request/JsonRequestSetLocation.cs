using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JodelAPI.Json.Request
{
    internal class JsonRequestSetLocation : JsonRequest
    {
        #region Fields and Properties

        public Location location { get; set; } = new Location();

        #endregion

        #region class

        public class Location
        {
            #region Fields and Properties

            public string city { get; set; }
            public string country { get; set; }
            public double loc_accuracy { get; set; }
            public Coordinates loc_coordinates { get; set; } = new Coordinates();
            public string name { get; set; }
            #endregion
        }

        public class Coordinates
        {
            #region Fields and Properties

            public double lat { get; set; }
            public double lng { get; set; }

            #endregion
        }

        #endregion
    }
}
