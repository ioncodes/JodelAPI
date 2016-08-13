using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JodelAPI.Objects
{
    public class Tokens
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public int ExpireTimestamp { get; set; }
    }
}
