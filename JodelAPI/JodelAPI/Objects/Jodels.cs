using System;
using JodelAPI.Json;

namespace JodelAPI.Objects
{
    public class Jodels
    {
        internal Jodels(JsonJodels.Recent jodelJson)
        {
            string imageUrl = "";
            bool isUrl = false;
            if (!string.IsNullOrWhiteSpace(jodelJson.image_url))
            {
                imageUrl = "http:" + jodelJson.image_url;
                isUrl = true;
            }

            PostId = jodelJson.post_id;
            Message = jodelJson.message;
            HexColor = jodelJson.color;
            IsImage = isUrl;
            ImageUrl = imageUrl;
            VoteCount = jodelJson.vote_count;
            LocationName = jodelJson.location.name;
            CommentsCount = jodelJson.child_count ?? 0;
            ChildCount = jodelJson.child_count ?? 0;
            CreatedAt = DateTime.ParseExact(jodelJson.created_at.Replace("Z", "").Replace("T", " "), "yyyy-MM-dd HH:mm:ss.fff", null);
            UpdatedAt = DateTime.ParseExact(jodelJson.updated_at.Replace("Z", "").Replace("T", " "), "yyyy-MM-dd HH:mm:ss.fff", null);
            Distance = jodelJson.distance;
            IsNotificationEnabled = jodelJson.notifications_enabled;
            PinCount = jodelJson.pin_count;
            PostOwn = jodelJson.post_own;
            UserHandle = jodelJson.user_handle;
            IsOwn = jodelJson.post_own.Equals("own");
        }

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

        public string ImageUrl { get; set; }

        public bool IsOwn { get; set; }
    }
}