using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JodelAPI.Json.Request
{
    internal class JsonRequestPostJodel : JsonRequest
    {
        #region Fields and Properties

        public string ancestor { get; set; } = null;
        public string color { get; set; }
        public Location location { get; set; } = new Location();
        public string message { get; set; }
        public bool to_home { get; set; }

        #endregion

        #region internal classes

        internal class Location
        {
            #region Fields and Properties

            public string city { get; set; }
            public string country { get; set; }
            public double loc_accuracy { get; set; } = 0.0;
            public Position loc_coordinates { get; set; } = new Position();
            public string name { get; set; }

            #endregion
        }

        internal class Position
        {
            #region Fields and Properties

            public double lat { get; set; }
            public double lng { get; set; }

            #endregion
        }

        #endregion
    }
}
