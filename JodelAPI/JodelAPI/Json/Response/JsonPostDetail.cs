using System.Collections.Generic;

namespace JodelAPI.Json.Response
{
    internal class JsonPostDetail
    {
        public class LocCoordinates : JsonPostJodels.LocCoordinates { }

        public class Location : JsonPostJodels.Location { }

        public class ImageHeaders : JsonPostJodels.ImageHeaders { }

        public class Post : JsonPostJodels.Post
        {
            public string ancestor;
            public bool from_home;
            public int parent_creator;
            public string parent_id;
            public bool pinned;
            public string replier_mark;
        }

        public class RootObject
        {
            public Post details { get; set; }
            public string next { get; set; }
            public int remaining { get; set; }
            public List<Post> replies { get; set; }
        }
    }
}