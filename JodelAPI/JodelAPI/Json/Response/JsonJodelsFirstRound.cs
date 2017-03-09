using System.Collections.Generic;

namespace JodelAPI.Json.Response
{
    internal class JsonJodelsFirstRound
    {
        public class LocCoordinates : JsonPostJodels.LocCoordinates { }

        public class Location : JsonPostJodels.Location { }

        public class ImageHeaders : JsonPostJodels.ImageHeaders { }

        public class Recent : JsonPostJodels.Post { }

        public class LocCoordinates2 : JsonPostJodels.LocCoordinates { }

        public class Location2 : JsonPostJodels.Location { }

        public class ImageHeaders2 : JsonPostJodels.ImageHeaders { }

        public class Replied : JsonPostJodels.Post { }

        public class LocCoordinates3 : JsonPostJodels.LocCoordinates { }

        public class Location3 : JsonPostJodels.Location { }

        public class Voted : JsonPostJodels.Post { }

        public class RootObject
        {
            public List<Recent> recent { get; set; }
            public List<Replied> replied { get; set; }
            public List<Voted> voted { get; set; }
            public int max { get; set; }
        }
    }
}