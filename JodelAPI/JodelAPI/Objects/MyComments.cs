namespace JodelAPI.Objects
{
    public class MyComments
    {
        public string PostId { get; set; }

        public string Message { get; set; }

        public string UserHandle { get; set; }

        public int VoteCount { get; set; }

        public string HexColor { get; set; }

        public bool IsOwn { get; set; }

        public string Latitude { get; set; }

        public string Longitude { get; set; }

        public string LocationName { get; set; }
    }
}