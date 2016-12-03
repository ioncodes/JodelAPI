using JodelAPI.Json;

namespace JodelAPI.Objects
{
    public class ChannelJodel : Jodels
    {
        internal ChannelJodel(JsonJodels.Recent jodelJson) : base(jodelJson)
        {
        }

        public bool IsOwn { get; set; }
    }
}