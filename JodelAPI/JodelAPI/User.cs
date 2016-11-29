using System.Collections.Generic;

namespace JodelAPI
{
    public class User
    {
        public string AccessToken;
        public string Latitude;
        public string Longitude;
        public string CountryCode;
        public string City;
        public string GoogleApiToken;
        public UserConfig Config;

        public User(string accessToken, string latitude, string longitude, string countryCode, string city, string googleApiToken = "")
        {
            AccessToken = accessToken;
            Latitude = latitude;
            Longitude = longitude;
            CountryCode = countryCode;
            City = city;
            GoogleApiToken = googleApiToken;
        }

        public class UserConfig
        {
            public bool Moderator { get; set; }
            public object UserType { get; set; }
            public List<Experiment> Experiments { get; set; }
            public List<Jodel.Channel> FollowedChannels { get; set; }
            public List<string> FollowedHashtags { get; set; }
            public int ChannelsFollowLimit { get; set; }
            public bool TripleFeedEnabled { get; set; }
            public string HomeName { get; set; }
            public bool HomeSet { get; set; }
            public string Location { get; set; }
            public bool Verified { get; set; }

        }

        public class Experiment
        {
            public string Name { get; set; }
            public string Group { get; set; }
            public List<string> Features { get; set; }
        }
    }
}