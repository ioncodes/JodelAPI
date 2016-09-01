using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using JodelAPI.Json;
using JodelAPI.Objects;
using Newtonsoft.Json;

namespace JodelAPI
{
    public static class Moderation
    {
        /// <summary>
        /// Reason for reporting Jodels
        /// </summary>
        public enum Reason
        {
            PersonalData = 1,
            Mobbing = 2,
            Spamming = 3,
            Spoiler = 4,
            Other = 5
        }

        /// <summary>
        /// Decisions for flaging an Jodel
        /// </summary>
        public enum Decision
        {
            Allow = 0,
            Block = 2,
            DontKnow = 1
        }

        /// <summary>
        /// Gets the reported Jodels
        /// </summary>
        /// <returns>List&lt;ModerationQueue&gt;.</returns>
        public static List<ModerationQueue> GetModerationQueue()
        {
            string plainJson;
            using (var client = new MyWebClient())
            {
                client.Encoding = Encoding.UTF8;
                plainJson = client.DownloadString(Constants.LinkModeration.ToLink());
            }
            JsonModeration.RootObject queue = JsonConvert.DeserializeObject<JsonModeration.RootObject>(plainJson);
            return queue.posts.Select(item => new ModerationQueue()
            {
                PostId = item.post_id,
                FlagCount = item.flag_count,
                FlagReason = item.flag_reason,
                HexColor = item.color,
                Message = item.message,
                ParentId = item.parent_id,
                TaskId = item.task_id,
                UserHandle = item.user_handle,
                VoteCount = item.vote_count

            }).ToList();
        }

        /// <summary>
        /// Generates an access token.
        /// </summary>
        /// <returns>System.String.</returns>
        public static Tokens GenerateAccessToken()
        {
            DateTime dt = DateTime.UtcNow;
            string jsonString;
            string deviceUid = Helpers.Sha256(Helpers.RandomString(5, true));

            string stringifiedPayload = @"POST%api.go-tellm.com%443%/api/v2/users/%%" + $"{dt:s}Z" +
                                        @"%%{""device_uid"": """ + deviceUid + @""", ""location"": {""city"": """ + Account.City +
                                        @""", ""loc_accuracy"": 100, ""loc_coordinates"": {""lat"": " + Account.Latitude +
                                        @", ""lng"": " + Account.Longitude + @"}, ""country"": """ + Account.CountryCode + @"""}, " +
                                        @"""client_id"": """ + Constants.ClientId + @"""}";

            string payload = @"{""device_uid"": """ + deviceUid + @""", ""location"": {""city"": """ + Account.City +
                             @""", ""loc_accuracy"": 100, ""loc_coordinates"": " + @"{""lat"": " + Account.Latitude +
                             @", ""lng"": " + Account.Longitude + @"}, ""country"": """ + Account.CountryCode +
                             @"""}, ""client_id"": """ + Constants.ClientId + @"""}";

            var keyByte = Encoding.UTF8.GetBytes(Constants.Key);
            using (var hmacsha1 = new HMACSHA1(keyByte))
            {
                hmacsha1.ComputeHash(Encoding.UTF8.GetBytes(stringifiedPayload));

                using (var client = new MyWebClient())
                {
                    client.Headers.Add(Constants.Header.ToHeader(stringifiedPayload));
                    client.Encoding = Encoding.UTF8;
                    jsonString = client.UploadString(Constants.LinkGenAt, payload);
                }
            }

            JsonTokens.RootObject objTokens = JsonConvert.DeserializeObject<JsonTokens.RootObject>(jsonString);

            return new Tokens { AccessToken = objTokens.access_token, RefreshToken = objTokens.refresh_token, ExpireTimestamp = objTokens.expiration_date };
        }

        /// <summary>
        /// Flags the jodel.
        /// </summary>
        /// <param name="taskId">The task identifier.</param>
        /// <param name="decision">The decision.</param>
        public static void FlagJodel(int taskId, Decision decision)
        {
            DateTime dt = DateTime.UtcNow;

            string dec = Convert.ChangeType((object) decision, (TypeCode) decision.GetTypeCode())?.ToString(); // get int from enum.
            string stringifiedPayload = @"POST%api.go-tellm.com%443%/api/v3/moderation/%%" + $"{dt:s}Z" +
                            @"%%{""decision"": " + dec +
                            @", ""task_id"": """ + taskId +
                            @"""}";

            string payload = @"{""decision"": " + dec +
                                        @", ""task_id"": """ + taskId +
                                        @"""}";

            using (var client = new MyWebClient())
            {
                client.Headers.Add(Constants.Header.ToHeader(stringifiedPayload));
                client.Encoding = Encoding.UTF8;
                client.UploadString(Constants.LinkModeration.ToLink(), payload);
            }
        }

        /// <summary>
        /// Determines whether the specified token is from a moderator.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns><c>true</c> if the specified token is moderator; otherwise, <c>false</c>.</returns>
        public static bool IsModerator(string token)
        {
            string plainJson;
            using (var client = new MyWebClient())
            {
                client.Encoding = Encoding.UTF8;
                plainJson = client.DownloadString(Constants.LinkConfig.ToLink());
            }

            JsonConfig.RootObject config = JsonConvert.DeserializeObject<JsonConfig.RootObject>(plainJson);

            if (config.moderator)
            {
                return true;
            }
            return false;
        }


        /// <summary>
        /// Refreshes the access token.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns>Tokens.</returns>
        public static Tokens RefreshAccessToken(Tokens token)
        {
            string plainJson;
            const string payload = @"{""refresh_token"": ""{RT}""}";
            using (var client = new MyWebClient())
            {
                client.Encoding = Encoding.UTF8;
                client.Headers.Add("Content-Type", "application/json");
                plainJson = client.UploadString(Constants.LinkRefreshToken.Replace("{AT}", token.AccessToken), payload.Replace("{RT}", token.RefreshToken));
            }

            JsonRefreshTokens.RootObject objRefToken = JsonConvert.DeserializeObject<JsonRefreshTokens.RootObject>(plainJson);

            return new Tokens { AccessToken = objRefToken.access_token, ExpireTimestamp = objRefToken.expiration_date, RefreshToken = token.RefreshToken };
        }

        /// <summary>
        /// Refreshes the access token.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <param name="refreshToken">The refresh token.</param>
        /// <returns>Tokens.</returns>
        public static Tokens RefreshAccessToken(string accessToken, string refreshToken)
        {
            string plainJson;
            const string payload = @"{""refresh_token"": ""{RT}""}";
            using (var client = new MyWebClient())
            {
                client.Encoding = Encoding.UTF8;
                client.Headers.Add("Content-Type", "application/json");
                plainJson = client.UploadString(Constants.LinkRefreshToken.Replace("{AT}", accessToken), payload.Replace("{RT}", refreshToken));
            }

            JsonRefreshTokens.RootObject objRefToken = JsonConvert.DeserializeObject<JsonRefreshTokens.RootObject>(plainJson);

            return new Tokens { AccessToken = objRefToken.access_token, ExpireTimestamp = objRefToken.expiration_date, RefreshToken = refreshToken };
        }
    }
}
