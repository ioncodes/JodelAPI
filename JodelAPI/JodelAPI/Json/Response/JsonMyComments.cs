using System.Collections.Generic;

namespace JodelAPI.Json.Response
{
    internal class JsonMyComments
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

        public class ImageHeaders
        {
            public string Host { get; set; }
            public string Authorization { get; set; }
        }

        public class Post
        {
            public string post_id { get; set; }
            public string created_at { get; set; }
            public string message { get; set; }
            public int discovered_by { get; set; }
            public string updated_at { get; set; }
            public string post_own { get; set; }
            public int discovered { get; set; }
            public string voted { get; set; }
            public int distance { get; set; }
            public int child_count { get; set; }
            public List<object> children { get; set; }
            public int vote_count { get; set; }
            public string color { get; set; }
            public Location location { get; set; }
            public List<object> tags { get; set; }
            public string user_handle { get; set; }
            public string image_url { get; set; }
            public ImageHeaders image_headers { get; set; }
            public string thumbnail_url { get; set; }
        }

        public class RootObject
        {
            public List<Post> posts { get; set; }
            public int max { get; set; }
        }
    }
}