using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JodelAPI.Json.Request
{
    internal class JsonRequestPostsLocationCombo : JsonRequest
    {
        #region Fields and Properties

        public string Lat { get; set; }
        public string Lng { get; set; }
        public bool Stickies { get; set; }
        public bool Home { get; set; }

        #endregion

        #region Methods

        public override string ToString()
        {
            return @"lat:      " + Lat + "@\n" +
                   @"lng:      " + Lng + "@\n" +
                   @"stickers: " + Stickies + @"\n" +
                   @"home:     " + Home;
        }

        #endregion
    }
}
