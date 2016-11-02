using System;

namespace JodelAPI.Objects
{
    public class Comments
    {
        // post_id, message, user_handle, vote_count

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