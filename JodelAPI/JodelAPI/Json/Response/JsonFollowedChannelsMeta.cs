using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JodelAPI.Json.Response
{
    internal class JsonFollowedChannelsMeta
    {
        public class Channel
        {
            public string channel { get; set; }
            public int followers { get; set; }
            public bool sponsored { get; set; }
            public bool unread { get; set; }
        }

        public class RootObject
        {
            public List<Channel> channels { get; set; }
        }
    }
}
