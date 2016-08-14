using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JodelAPI.Json
{
    class JsonRefreshTokens
    {
        public class RootObject
        {
            public string access_token { get; set; }
            public string token_type { get; set; }
            public int expires_in { get; set; }
            public int expiration_date { get; set; }
        }
    }
}
