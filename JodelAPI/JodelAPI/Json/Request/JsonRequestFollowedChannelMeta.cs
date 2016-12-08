using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace JodelAPI.Json.Request
{
    internal class JsonRequestFollowedChannelMeta : JsonRequest
    {
        #region Fields and Properties

        public Dictionary<string, int> Values { get; set; }

        #endregion

        #region Constructor

        public JsonRequestFollowedChannelMeta()
        {
            Values = new Dictionary<string, int>();
        }

        #endregion

        #region Methods

        public override string ToString()
        {
            return JsonConvert.SerializeObject(Values);
        }

        #endregion
    }
}
