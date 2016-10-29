using System.Collections.Generic;

namespace JodelAPI.Json
{
    internal class JsonRecommendedChannels
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