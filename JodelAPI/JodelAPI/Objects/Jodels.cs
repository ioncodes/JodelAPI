using System;

namespace JodelAPI.Objects
{
    public class Jodels
    {
        public string PostId { get; set; }

        public string Message { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public int PinCount { get; set; }

        public string HexColor { get; set; }

        public bool IsNotificationEnabled { get; set; }

        public string PostOwn { get; set; }

        public int Distance { get; set; }

        public int ChildCount { get; set; }

        public bool IsImage { get; set; }

        public int VoteCount { get; set; }

        public int CommentsCount { get; set; }

        public string LocationName { get; set; }

        public string UserHandle { get; set; }
    }
}