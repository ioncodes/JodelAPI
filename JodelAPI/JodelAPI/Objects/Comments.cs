namespace JodelAPI.Objects
{
    public class Comments
    {
        // post_id, message, user_handle, vote_count

        public string PostId { get; set; }

        public string Message { get; set; }

        public string UserHandle { get; set; }

        public int VoteCount { get; set; }
    }
}