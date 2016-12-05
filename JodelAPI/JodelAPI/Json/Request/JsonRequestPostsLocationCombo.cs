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

        #endregion

        #region Methods

        public override string ToString()
        {
            return @"lat:      " + Lat + "@\r\n" +
                   @"lng:      " + Lng + "@\r\n" +
                   @"stickers: true\r\n" +
                   @"home:     false";
        }

        #endregion
    }
}
