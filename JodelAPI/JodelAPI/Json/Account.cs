using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using JodelAPI.Json;
using JodelAPI.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JodelAPI
{
    public class Account
    {
        private static User _user;

        internal Account(User user)
        {
            _user = user;
        }

        /// <summary>
        ///     Gets the karma.
        /// </summary>
        /// <returns>System.Int32.</returns>
        public int GetKarma()
        {
            string resp;
            using (var client = new MyWebClient())
            {
                resp = client.DownloadString(Constants.LinkGetKarma.ToLink());
            }
            string result = resp.Substring(resp.LastIndexOf(':') + 1);
            return Convert.ToInt32(result.Replace("}", "").Replace("\"", ""));
        }

        /// <summary>
        ///     Generates an access token.
        /// </summary>
        /// <returns>System.String.</returns>
        public static Tokens GenerateAccessToken()
        {
            DateTime dt = DateTime.UtcNow;
            string jsonString;
            string deviceUid = Helpers.Sha256(Helpers.RandomString(5, true));

            string stringifiedPayload = @"POST%api.go-tellm.com%443%/api/v2/users/%%" + $"{dt:s}Z" +
                                        @"%%{""device_uid"": """ + deviceUid + @""", ""location"": {""City"": """ + _user.City +
                                        @""", ""loc_accuracy"": 0, ""loc_coordinates"": {""lat"": " + _user.Latitude +
                                        @", ""lng"": " + _user.Longitude + @"}, ""country"": """ + _user.CountryCode + @"""}, " +
                                        @"""client_id"": """ + Constants.ClientId + @"""}";

            string payload = @"{""device_uid"": """ + deviceUid + @""", ""location"": {""City"": """ + _user.City +
                             @""", ""loc_accuracy"": 0, ""loc_coordinates"": " + @"{""lat"": " + _user.Latitude +
                             @", ""lng"": " + _user.Longitude + @"}, ""country"": """ + _user.CountryCode +
                             @"""}, ""client_id"": """ + Constants.ClientId + @"""}";

            var keyByte = Encoding.UTF8.GetBytes(Constants.Key);
            using (var hmacsha1 = new HMACSHA1(keyByte))
            {
                hmacsha1.ComputeHash(Encoding.UTF8.GetBytes(stringifiedPayload));

                using (var client = new MyWebClient())
                {
                    client.Headers.Add(Constants.Header.ToHeader(stringifiedPayload, DateTime.UtcNow));
                    client.Encoding = Encoding.UTF8;
                    jsonString = client.UploadString(Constants.LinkGenAt, payload);
                }
            }

            JsonTokens.RootObject objTokens = JsonConvert.DeserializeObject<JsonTokens.RootObject>(jsonString);

            return new Tokens
            {
                AccessToken = objTokens.access_token,
                RefreshToken = objTokens.refresh_token,
                ExpireTimestamp = objTokens.expiration_date
            };
        }

        public static Tokens GenerateAccessToken(string latitude, string longitude, string city, string countrycode)
        {
            DateTime dt = DateTime.UtcNow;
            string jsonString;
            string deviceUid = Helpers.Sha256(Helpers.RandomString(5, true));

            string stringifiedPayload = @"POST%api.go-tellm.com%443%/api/v2/users/%%" + $"{dt:s}Z" +
                                        @"%%{""device_uid"": """ + deviceUid + @""", ""location"": {""City"": """ + city +
                                        @""", ""loc_accuracy"": 0, ""loc_coordinates"": {""lat"": " + latitude +
                                        @", ""lng"": " + longitude + @"}, ""country"": """ + countrycode + @"""}, " +
                                        @"""client_id"": """ + Constants.ClientId + @"""}";

            string payload = @"{""device_uid"": """ + deviceUid + @""", ""location"": {""City"": """ + city +
                             @""", ""loc_accuracy"": 0, ""loc_coordinates"": " + @"{""lat"": " + latitude +
                             @", ""lng"": " + longitude + @"}, ""country"": """ + countrycode +
                             @"""}, ""client_id"": """ + Constants.ClientId + @"""}";

            var keyByte = Encoding.UTF8.GetBytes(Constants.Key);
            using (var hmacsha1 = new HMACSHA1(keyByte))
            {
                hmacsha1.ComputeHash(Encoding.UTF8.GetBytes(stringifiedPayload));

                using (var client = new MyWebClient())
                {
                    client.Headers.Add(Constants.Header.ToHeader(stringifiedPayload, DateTime.UtcNow));
                    client.Encoding = Encoding.UTF8;
                    jsonString = client.UploadString(Constants.LinkGenAt, payload);
                }
            }
            JsonTokens.RootObject objTokens = JsonConvert.DeserializeObject<JsonTokens.RootObject>(jsonString);

            return new Tokens
            {
                AccessToken = objTokens.access_token,
                RefreshToken = objTokens.refresh_token,
                ExpireTimestamp = objTokens.expiration_date
            };
        }

        public static Tokens GenerateAccessToken(User user)
        {
            DateTime dt = DateTime.UtcNow;
            string jsonString;
            string deviceUid = Helpers.Sha256(Helpers.RandomString(5, true));

            string stringifiedPayload = @"POST%api.go-tellm.com%443%/api/v2/users/%%" + $"{dt:s}Z" +
                                        @"%%{""device_uid"": """ + deviceUid + @""", ""location"": {""City"": """ + user.City +
                                        @""", ""loc_accuracy"": 0, ""loc_coordinates"": {""lat"": " + user.Latitude +
                                        @", ""lng"": " + user.Longitude + @"}, ""country"": """ + user.CountryCode + @"""}, " +
                                        @"""client_id"": """ + Constants.ClientId + @"""}";

            string payload = @"{""device_uid"": """ + deviceUid + @""", ""location"": {""City"": """ + user.City +
                             @""", ""loc_accuracy"": 0, ""loc_coordinates"": " + @"{""lat"": " + user.Latitude +
                             @", ""lng"": " + user.Longitude + @"}, ""country"": """ + user.CountryCode +
                             @"""}, ""client_id"": """ + Constants.ClientId + @"""}";

            var keyByte = Encoding.UTF8.GetBytes(Constants.Key);
            using (var hmacsha1 = new HMACSHA1(keyByte))
            {
                hmacsha1.ComputeHash(Encoding.UTF8.GetBytes(stringifiedPayload));

                using (var client = new MyWebClient())
                {
                    client.Headers.Add(Constants.Header.ToHeader(stringifiedPayload, DateTime.UtcNow));
                    client.Encoding = Encoding.UTF8;
                    jsonString = client.UploadString(Constants.LinkGenAt, payload);
                }
            }

            JsonTokens.RootObject objTokens = JsonConvert.DeserializeObject<JsonTokens.RootObject>(jsonString);

            return new Tokens
            {
                AccessToken = objTokens.access_token,
                RefreshToken = objTokens.refresh_token,
                ExpireTimestamp = objTokens.expiration_date
            };
        }

        /// <summary>
        ///     Sets the user location.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        public void SetUserLocation(string accessToken)
        {
            string payload =
                @"{""location"": {""City"": """ + _user.City + @""", ""loc_accuracy"": 0, ""loc_coordinates"": {" +
                @"""lat"": " + _user.Latitude + @", ""lng"": " + _user.Longitude + @"}, ""country"": """ + _user.CountryCode +
                @"""}, ""name"": """ + _user.City + @"""}";
            var client = new WebClient { Encoding = Encoding.UTF8 };
            client.Headers.Add("Content-Type", "application/json");
            client.UploadString(Constants.LinkUserLocation.Replace("{AT}", accessToken), "PUT", payload);
        }

        /// <summary>
        ///     Refreshes the access token.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns>Tokens.</returns>
        public Tokens RefreshAccessToken(Tokens token)
        {
            string plainJson;
            const string payload = @"{""refresh_token"": ""{RT}""}";
            using (var client = new MyWebClient())
            {
                client.Encoding = Encoding.UTF8;
                client.Headers.Add("Content-Type", "application/json");
                plainJson = client.UploadString(Constants.LinkRefreshToken.Replace("{AT}", token.AccessToken),
                    payload.Replace("{RT}", token.RefreshToken));
            }

            JsonRefreshTokens.RootObject objRefToken =
                JsonConvert.DeserializeObject<JsonRefreshTokens.RootObject>(plainJson);

            return new Tokens
            {
                AccessToken = objRefToken.access_token,
                ExpireTimestamp = objRefToken.expiration_date,
                RefreshToken = token.RefreshToken
            };
        }

        /// <summary>
        ///     Refreshes the access token.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <param name="refreshToken">The refresh token.</param>
        /// <returns>Tokens.</returns>
        public Tokens RefreshAccessToken(string accessToken, string refreshToken)
        {
            string plainJson;
            const string payload = @"{""refresh_token"": ""{RT}""}";
            using (var client = new MyWebClient())
            {
                client.Encoding = Encoding.UTF8;
                client.Headers.Add("Content-Type", "application/json");
                plainJson = client.UploadString(Constants.LinkRefreshToken.Replace("{AT}", accessToken),
                    payload.Replace("{RT}", refreshToken));
            }

            JsonRefreshTokens.RootObject objRefToken =
                JsonConvert.DeserializeObject<JsonRefreshTokens.RootObject>(plainJson);

            return new Tokens
            {
                AccessToken = objRefToken.access_token,
                ExpireTimestamp = objRefToken.expiration_date,
                RefreshToken = refreshToken
            };
        }
    }
}