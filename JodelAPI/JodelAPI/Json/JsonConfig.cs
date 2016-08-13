using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JodelAPI.Json
{
    class JsonConfig
    {
        public class Experiment
        {
            public string name { get; set; }
            public string group { get; set; }
            public List<string> features { get; set; }
        }

        public class RootObject
        {
            public bool moderator { get; set; }
            public object user_type { get; set; }
            public List<Experiment> experiments { get; set; }
            public List<object> followed_channels { get; set; }
            public int channels_follow_limit { get; set; }
            public bool triple_feed_enabled { get; set; }
        }
    }
}