using System.Collections.Generic;

namespace JodelAPI.Json
{
    internal class JsonMyJodels
    {
        public class LocCoordinates
        {
            public int lat { get; set; }
            public int lng { get; set; }
        }

        public class Location
        {
            public string name { get; set; }
            public LocCoordinates loc_coordinates { get; set; }
        }

        public class Post : JsonJodels.Recent
        {
            public string voted { get; set; }
        }

        public class RootObject
        {
            public List<Post> posts { get; set; }
            public int max { get; set; }
        }
    }
}