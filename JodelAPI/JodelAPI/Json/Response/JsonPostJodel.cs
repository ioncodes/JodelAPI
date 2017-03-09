using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JodelAPI.Json.Response
{
    internal class JsonPostJodel
    {
        public class RootObject
        {
            #region Fields and Properties

            public long created_at { get; set; }
            public string post_id { get; set; }

            #endregion
        }
    }
}
