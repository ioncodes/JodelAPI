using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JodelAPI.Json
{
    class JsonRecommendedChannels
    {

        public class Recommended
        {
            public string channel { get; set; }
            public int followers { get; set; }
        }

        public class RootObject
        {
            public List<Recommended> recommended { get; set; }
        }
    }
}
