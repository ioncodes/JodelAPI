using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using JodelAPI.Internal;
using JodelAPI.Json;
using Newtonsoft.Json;

namespace JodelAPI.Shared
{
    public class AccessToken
    {
        #region Fields and Properties

        public string DeviceUid { get; set; }

        public string Token { get; set; }

        public string RefreshToken { get; set; }

        public int ExpirationDate { get; set; }

        /// <summary>
        /// UTC-ExpirationDate
        /// </summary>
        public DateTime ExpirationDateTime
        {
            get
            {
                DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                dateTime = dateTime.AddSeconds(ExpirationDate);
                return dateTime;
            }
        }

        public string DistinctId { get; set; }

        public string TokenType { get; set; }

        public User UserConfig { get; set; }

        #endregion

        #region Constructor

        public AccessToken(User user)
        {
            this.DeviceUid = String.Empty;
            this.UserConfig = user;
        }

        public AccessToken(User user, string deviceUid, string token)
            : this(user)
        {
            this.DeviceUid = deviceUid;
            this.Token = token;
        }

        public AccessToken(User user, string deviceUid, string token, int expirationDate, string distinctId, string tokenType, string refreshToken)
            : this(user, deviceUid, token)
        {
            this.ExpirationDate = expirationDate;
            this.DistinctId = distinctId;
            this.TokenType = tokenType;
            this.RefreshToken = refreshToken;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Generates a new Accesstoken
        /// </summary>
        /// <returns>Successful</returns>
        public bool GenerateNewAccessToken()
        {
            try
            {
                if (DeviceUid == String.Empty)
                    DeviceUid = Helpers.GetRandomDeviceUid();

                string jsonString;
                string payload = @"{""device_uid"": """ +
                    DeviceUid +
                    @""", ""location"": {""City"": """ + UserConfig.CityName +
                    @""", ""loc_accuracy"": 0, ""loc_coordinates"": " + @"{""lat"": " + UserConfig.Place.Latitude.ToString(CultureInfo.InvariantCulture) +
                    @", ""lng"": " + UserConfig.Place.Longitude.ToString(CultureInfo.InvariantCulture) + @"}, ""country"": """ + UserConfig.CountryCode +
                    @"""}, ""client_id"": """ + Constants.ClientId + @"""}";

                
                using (JodelWebClient client = JodelWebClient.GetJodelWebClientWithHeaders(payload, "POST", Links.LinkGenAtPayload))
                {
                    jsonString = client.UploadString(Links.LinkGenAt, payload);
                }

                JsonTokens.RootObject objTokens = JsonConvert.DeserializeObject<JsonTokens.RootObject>(jsonString);

                this.Token = objTokens.access_token; 
                this.DistinctId = objTokens.distinct_id;
                this.ExpirationDate = objTokens.expiration_date;
                this.TokenType = objTokens.token_type;

            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Refreshes the access token
        /// </summary>
        /// <returns>Successful</returns>
        public bool RefreshAccessToken()
        {
            try
            {
                string plainJson;
                string payload = @"{""refresh_token"": """ + RefreshToken + @"""}";
                using (var client = new JodelWebClient())
                {
                    client.Encoding = Encoding.UTF8;
                    client.Headers.Add("Content-Type", "application/json");
                    plainJson = client.UploadString(Links.LinkRefreshToken.ToLink(UserConfig), payload);
                }

                JsonRefreshTokens.RootObject obj =
                    JsonConvert.DeserializeObject<JsonRefreshTokens.RootObject>(plainJson);

                this.Token = obj.access_token;
                this.ExpirationDate = obj.expiration_date;
                this.TokenType = obj.token_type;
            }
            catch
            {
                return false;
            }
            return true;
        }

        #endregion
    }
}
