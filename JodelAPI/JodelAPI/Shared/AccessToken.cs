using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
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

        #endregion
    }
}
