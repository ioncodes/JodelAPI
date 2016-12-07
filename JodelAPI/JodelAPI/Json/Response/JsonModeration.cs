using System.Collections.Generic;

namespace JodelAPI.Json.Response
{
    internal class JsonModeration
    {
        public class Queue
        {
            public string post_id { get; set; }
            public string message { get; set; }
            public int vote_count { get; set; }
            public string color { get; set; }
            public string user_handle { get; set; }
            public int task_id { get; set; }
            public int flag_count { get; set; }
            public string parent_id { get; set; }
            public int flag_reason { get; set; }
        }

        public class RootObject
        {
            public List<Queue> posts { get; set; }
        }
    }
}