using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JodelAPI.Json.Request
{
    class JsonRequestRefreshAccessToken : JsonRequest
    {
        public string refresh_token { get; set; }
    }
}
