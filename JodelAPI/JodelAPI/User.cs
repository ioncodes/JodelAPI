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

        public User(string accessToken, string latitude, string longitude, string countryCode, string city, string googleApiToken = "")
        {
            AccessToken = accessToken;
            Latitude = latitude;
            Longitude = longitude;
            CountryCode = countryCode;
            City = city;
            GoogleApiToken = googleApiToken;
        }
    }
}