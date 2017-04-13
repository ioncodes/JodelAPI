using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using JodelAPI.Internal;
using JodelAPI.Json;
using JodelAPI.Json.Request;
using JodelAPI.Json.Response;
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
                DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
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
            DeviceUid = string.Empty;
            UserConfig = user;
        }

        public AccessToken(User user, string deviceUid, string token)
            : this(user)
        {
            DeviceUid = deviceUid;
            Token = token;
        }

        public AccessToken(User user, string deviceUid, string token, int expirationDate, string distinctId, string tokenType, string refreshToken)
            : this(user, deviceUid, token)
        {
            ExpirationDate = expirationDate;
            DistinctId = distinctId;
            TokenType = tokenType;
            RefreshToken = refreshToken;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Generates a new Accesstoken
        /// </summary>
        /// <returns>Successful</returns>
        public bool GenerateNewAccessToken(string proxy = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(DeviceUid))
                {
                    DeviceUid = Helpers.GetRandomDeviceUid();
                }

                JsonRequestGenerateAccessToken payload = new JsonRequestGenerateAccessToken
                {
                    device_uid = DeviceUid,
                    location = new JsonRequestGenerateAccessToken.Location
                    {
                        City = UserConfig.CityName,
                        country = UserConfig.CountryCode,
                        loc_accuracy = 0,
                        loc_coordinates = new JsonRequestGenerateAccessToken.Location.Coordinates
                        {
                            lat = UserConfig.Place.Latitude,
                            lng = UserConfig.Place.Longitude
                        }
                    },
                    client_id = Constants.ClientId
                };

                string jsonString = Links.GetRequestToken.ExecuteRequest(UserConfig, payload: payload, proxy: proxy);

                JsonTokens.RootObject objTokens = JsonConvert.DeserializeObject<JsonTokens.RootObject>(jsonString);

                Token = objTokens.access_token;
                DistinctId = objTokens.distinct_id;
                ExpirationDate = objTokens.expiration_date;
                TokenType = objTokens.token_type;
                RefreshToken = objTokens.refresh_token;
            }
            catch (Exception)
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
                JsonRequestRefreshAccessToken payload = new JsonRequestRefreshAccessToken {refresh_token = RefreshToken};

                string plainJson = Links.GetNewAccessToken.ExecuteRequest(UserConfig, payload: payload);

                JsonRefreshTokens.RootObject obj =
                    JsonConvert.DeserializeObject<JsonRefreshTokens.RootObject>(plainJson);

                Token = obj.access_token;
                ExpirationDate = obj.expiration_date;
                TokenType = obj.token_type;
            }
            catch(Exception)
            {
                return false;
            }
            return true;
        }

        #endregion
    }
}
