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
    }
}
