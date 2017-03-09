using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JodelAPI.Json.Response
{
    internal class JsonCaptcha
    {
        public class RootObject
        {
            public string key { get; set; }
            public string image_url { get; set; }
            public int image_size { get; set; }
        }
    }
}
