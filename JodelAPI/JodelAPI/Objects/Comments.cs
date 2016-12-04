using System;
using JodelAPI.Json;

namespace JodelAPI.Objects
{
    public class Comments
    {
        internal Comments(JsonComments.Child jComment)
        {
            string imageUrl = "";
            bool isUrl = false;

            if (!string.IsNullOrWhiteSpace(jComment.image_url))
            {
                imageUrl = "http:" + jComment.image_url;
                isUrl = true;
            }

            Message = jComment.message;
            PostId = jComment.post_id;
            CreatedAt = DateTime.ParseExact(jComment.created_at.Replace("Z", "").Replace("T", " "),
                "yyyy-MM-dd HH:mm:ss.fff", null);
            UpdatedAt = DateTime.ParseExact(jComment.updated_at.Replace("Z", "").Replace("T", " "),
                "yyyy-MM-dd HH:mm:ss.fff", null);
            IsImage = isUrl;
            ImageUrl = imageUrl;
            UserHandle = jComment.user_handle;
            VoteCount = jComment.vote_count;
        }

        public string PostId { get; set; }

        public string Message { get; set; }

        public string UserHandle { get; set; }

        public int VoteCount { get; set; }

        public DateTime CreatedAt { get; set; }
 
    	public DateTime UpdatedAt { get; set; }
 
 		public bool IsImage { get; set; }
 
		public string ImageUrl { get; set; }
    }
}