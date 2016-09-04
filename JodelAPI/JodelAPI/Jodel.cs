using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using JodelAPI.Objects;
using JodelAPI.Json;
using Newtonsoft.Json;

namespace JodelAPI
{
    public static class Jodel
    {
        /// <summary>
        /// Colors for Jodels
        /// </summary>
        public enum PostColor
        {
            Orange,
            Yellow,
            Red,
            Blue,
            Bluegreyish,
            Green,
            Random
        }

        /// <summary>
        /// Methods to sort List<Jodels>
        /// </summary>
        public enum SortMethod
        {
            MostCommented,
            Top
        }

        private static string _lastPostId = "";

        /// <summary>
        /// Gets the first amount of Jodels (internal usage)
        /// </summary>
        /// <returns>List&lt;Jodels&gt;.</returns>
        public static List<Jodels> GetFirstJodels()
        {
            string plainJson;
            using (var client = new MyWebClient())
            {
                client.Encoding = Encoding.UTF8;
                plainJson = client.DownloadString(Constants.LinkFirstJodels.ToLink());
            }
            JsonJodelsFirstRound.RootObject jfr = JsonConvert.DeserializeObject<JsonJodelsFirstRound.RootObject>(plainJson);
            List<Jodels> temp = new List<Jodels>(); // List<post_id,message>

            foreach (var item in jfr.recent)
            {
                string msg = item.message;
                bool isUrl = false;
                if (msg == "Jodel")
                {
                    msg = "http:" + item.image_url;
                    isUrl = true;
                }

                Jodels objJodels = new Jodels
                {
                    PostId = item.post_id,
                    Message = msg,
                    HexColor = item.color,
                    IsImage = isUrl,
                    VoteCount = item.vote_count,
                    LocationName = item.location.name,
                    CommentsCount = item.child_count ?? 0
                };

                temp.Add(objJodels);
            }

            _lastPostId = temp.Last().PostId; // Set the last post_id for next jodels

            return temp;
        }

        /// <summary>
        /// Gets the second amount of Jodels (internal usage)
        /// </summary>
        /// <returns>List&lt;Jodels&gt;.</returns>
        public static List<Jodels> GetNextJodels()
        {
            List<Jodels> temp = new List<Jodels>();
            for (int e = 0; e < 3; e++)
            {
                string plainJson;
                using (var client = new MyWebClient())
                {
                    client.Encoding = Encoding.UTF8;
                    plainJson = client.DownloadString(Constants.LinkSecondJodels.ToLink(_lastPostId));
                }
                JsonJodelsLastRound.RootObject jlr = JsonConvert.DeserializeObject<JsonJodelsLastRound.RootObject>(plainJson);
                foreach (var item in jlr.posts)
                {
                    string msg = item.message;
                    bool isUrl = false;
                    if (msg == "Jodel")
                    {
                        msg = "http:" + item.image_url; // WELL THERE IS NO IMAGE_URL!!!!???
                        isUrl = true;
                    }

                    Jodels objJodels = new Jodels
                    {
                        PostId = item.post_id,
                        Message = msg,
                        HexColor = item.color,
                        IsImage = isUrl,
                        VoteCount = item.vote_count,
                        LocationName = item.location.name,
                        CommentsCount = item.child_count ?? 0
                    };

                    temp.Add(objJodels);
                }
                if (temp.Count == 0)
                    return temp; // not enough Jodels anymore.
                _lastPostId = temp.Last().PostId; // Set the last post_id for next jodels
            }
            return temp;
        }

        /// <summary>
        /// Gets all jodels.
        /// </summary>
        /// <returns>List&lt;Jodels&gt;.</returns>
        public static List<Jodels> GetAllJodels()
        {
            var allJodels = GetFirstJodels();
            allJodels.AddRange(GetNextJodels());
            return allJodels;
        }

        /// <summary>
        /// Upvotes the specified post identifier (Jodel).
        /// </summary>
        /// <param name="postId">The post identifier.</param>
        public static void Upvote(string postId)
        {
            DateTime dt = DateTime.UtcNow;

            string stringifiedPayload =
                @"PUT%api.go-tellm.com%443%/api/v2/posts/" + postId + "/" + "upvote/%" + Account.AccessToken + "%" + $"{dt:s}Z" + "%%";

            using (var client = new MyWebClient())
            {
                client.Headers.Add(Constants.Header.ToHeader(stringifiedPayload, true));
                client.Encoding = Encoding.UTF8;
                client.UploadData(Constants.LinkUpvoteJodel.ToLink(postId), "PUT", new byte[] { });
            }
        }

        /// <summary>
        /// Downvotes the specified post identifier (Jodel).
        /// </summary>
        /// <param name="postId">The post identifier.</param>
        public static void Downvote(string postId)
        {
            DateTime dt = DateTime.UtcNow;

            string stringifiedPayload =
                @"PUT%api.go-tellm.com%443%/api/v2/posts/" + postId + "/" + "downvote/%" + Account.AccessToken + "%" + $"{dt:s}Z" + "%%";

            using (var client = new MyWebClient())
            {
                client.Headers.Add(Constants.Header.ToHeader(stringifiedPayload, true));
                client.Encoding = Encoding.UTF8;
                client.UploadData(Constants.LinkDownvoteJodel.ToLink(postId), "PUT", new byte[] { });
            }
        }

        /// <summary>
        /// Posts an jodel.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="colorParam">The color parameter.</param>
        /// <param name="postId">The post identifier.</param>
        public static string PostJodel(string message, PostColor colorParam = PostColor.Random, string postId = null)
        {
            DateTime dt = DateTime.UtcNow;

            var color = Helpers.GetColor(colorParam);

            string jsonCommentFragment = String.Empty;
            if (postId != null)
            {
                jsonCommentFragment = @"""ancestor"": """ + postId + @""", ";
            }

            string stringifiedPayload = @"POST%api.go-tellm.com%443%/api/v2/posts/%" + Account.AccessToken + "%" + $"{dt:s}Z" +
                                        @"%%{""color"": """ + color + @""", " + jsonCommentFragment +
                                        @"""message"": """ + message + @""", ""location"": {""loc_accuracy"": 1, ""city"": """ + Account.City +
                                        @""", ""loc_coordinates"": {""lat"": " + Account.Latitude + @", ""lng"": " + Account.Longitude +
                                        @"}, ""country"": """ + Account.CountryCode + @""", ""name"": """ + Account.City + @"""}}";

            string payload = @"{""color"": """ + color + @""", " + jsonCommentFragment +
                             @"""message"": """ + message + @""", ""location"": {""loc_accuracy"": 1, ""city"": """ + Account.City +
                             @""", ""loc_coordinates"": " + @"{""lat"": " + Account.Latitude + @", ""lng"": " + Account.Longitude +
                             @"}, ""country"": """ + Account.CountryCode + @""", ""name"": """ + Account.City + @"""}}";

            var keyByte = Encoding.UTF8.GetBytes(Constants.Key);
            using (var hmacsha1 = new HMACSHA1(keyByte))
            {
                hmacsha1.ComputeHash(Encoding.UTF8.GetBytes(stringifiedPayload));

                using (var client = new MyWebClient())
                {
                    client.Headers.Add(Constants.Header.ToHeader(stringifiedPayload, true));
                    client.Encoding = Encoding.UTF8;
                    string newJodels = client.UploadString(Constants.LinkPostJodel, payload);
                    JsonPostJodels.RootObject temp = JsonConvert.DeserializeObject<JsonPostJodels.RootObject>(newJodels);
                    return temp.posts[0].post_id;
                }
            }
        }

        /// <summary>
        /// Gets the comments.
        /// </summary>
        /// <param name="postId">The post identifier.</param>
        /// <returns>List&lt;Comments&gt;.</returns>
        public static List<Comments> GetComments(string postId)
        {
            string plainJson;
            using (var client = new MyWebClient())
            {
                client.Encoding = Encoding.UTF8;
                plainJson = client.DownloadString(Constants.LinkGetComments.ToLink(postId));
            }
            JsonComments.RootObject com = JsonConvert.DeserializeObject<JsonComments.RootObject>(plainJson);

            return com.children.Select(c => new Comments()
            {
                PostId = c.post_id,
                Message = c.message,
                UserHandle = c.user_handle,
                VoteCount = c.vote_count
            }).ToList();
        }

        /// <summary>
        /// Gets my jodels.
        /// </summary>
        /// <returns>List&lt;MyJodels&gt;.</returns>
        public static List<MyJodels> GetMyJodels()
        {
            string plainJson;
            using (var client = new MyWebClient())
            {
                client.Encoding = Encoding.UTF8;
                plainJson = client.DownloadString(Constants.LinkGetMyJodels.ToLink());
            }

            JsonMyJodels.RootObject myJodels = JsonConvert.DeserializeObject<JsonMyJodels.RootObject>(plainJson);
            return myJodels.posts.Select(item => new MyJodels()
            {
                PostId = item.post_id,
                Message = item.message,
                HexColor = item.color,
                VoteCount = item.vote_count,
                Latitude = item.location.loc_coordinates.lat.ToString(),
                Longitude = item.location.loc_coordinates.lng.ToString(),
                LocationName = item.location.name
            }).ToList();
        }

        /// <summary>
        /// Gets my comments.
        /// </summary>
        /// <returns>List&lt;MyComments&gt;.</returns>
        public static List<MyComments> GetMyComments()
        {
            string plainJson;
            using (var client = new MyWebClient())
            {
                client.Encoding = Encoding.UTF8;
                plainJson = client.DownloadString(Constants.LinkGetMyComments.ToLink());
            }

            JsonMyComments.RootObject myComments = JsonConvert.DeserializeObject<JsonMyComments.RootObject>(plainJson);
            return myComments.posts.Select(item => new MyComments()
            {
                PostId = item.post_id,
                Message = item.message,
                HexColor = item.color,
                VoteCount = item.vote_count,
                IsOwn = item.post_own.Equals("own"),
                Latitude = item.location.loc_coordinates.lat.ToString(),
                Longitude = item.location.loc_coordinates.lng.ToString(),
                LocationName = item.location.name
            }).ToList();
        }

        /// <summary>
        /// Gets my votes.
        /// </summary>
        /// <returns>List&lt;MyVotes&gt;.</returns>
        public static List<MyVotes> GetMyVotes()
        {
            string plainJson;
            using (var client = new MyWebClient())
            {
                client.Encoding = Encoding.UTF8;
                plainJson = client.DownloadString(Constants.LinkGetMyVotes.ToLink());
            }

            JsonMyVotes.RootObject myVotes = JsonConvert.DeserializeObject<JsonMyVotes.RootObject>(plainJson);
            return myVotes.posts.Select(item => new MyVotes()
            {
                PostId = item.post_id,
                Message = item.message,
                HexColor = item.color,
                VoteCount = item.vote_count,
                IsOwn = item.post_own.Equals("own"),
                LocationName = item.location.name
            }).ToList();
        }

        /// <summary>
        /// Sorts the jodels.
        /// </summary>
        /// <param name="jodels">The jodels.</param>
        /// <param name="method">The method.</param>
        /// <returns>List&lt;Jodels&gt;.</returns>
        public static List<Jodels> Sort(List<Jodels> jodels, SortMethod method)
        {
            return method == SortMethod.MostCommented
                          ? jodels.OrderByDescending(o => o.CommentsCount).ToList()
                          : jodels.OrderByDescending(o => o.VoteCount).ToList();
        }

        /// <summary>
        /// Reports the jodel.
        /// </summary>
        /// <param name="postId"></param>
        /// <param name="reason"></param>
        public static void ReportJodel(string postId, Moderation.Reason reason)
        {
            string rea = Convert.ChangeType(reason, reason.GetTypeCode())?.ToString(); // get int from enum.
            string stringifiedPayload = @"{""reason_id"":" + rea + "}";

            using (var client = new MyWebClient())
            {
                client.Headers.Add(Constants.Header.ToHeader(stringifiedPayload, true));
                client.Encoding = Encoding.UTF8;
                client.UploadData(Constants.LinkReportJodel.ToLink(postId), "PUT", new byte[] { });
            }
        }

        /// <summary>
        /// Pins a Jodel.
        /// </summary>
        /// <param name="postId"></param>
        public static void PinJodel(string postId)
        {
            DateTime dt = DateTime.UtcNow;

            string stringifiedPayload =
                @"PUT%api.go-tellm.com%443%/api/v2/posts/" + postId + "/" + "pin?access_token=/%" + Account.AccessToken + "%" + $"{dt:s}Z" + "%%";

            using (var client = new MyWebClient())
            {
                client.Headers.Add(Constants.Header.ToHeader(stringifiedPayload));
                client.Encoding = Encoding.UTF8;
                client.UploadData(Constants.LinkPinJodel.ToLink(postId), "PUT", new byte[] { });
            }
        }

        /// <summary>
        /// Get's all pinned Jodels.
        /// </summary>
        /// <returns>List&lt;MyPins&gt;.</returns>
        public static List<MyPins> GetMyPins()
        {
            string plainJson;
            using (var client = new MyWebClient())
            {
                client.Encoding = Encoding.UTF8;
                plainJson = client.DownloadString(Constants.LinkMyPins.ToLink());
            }

            JsonMyPins.RootObject myPins = JsonConvert.DeserializeObject<JsonMyPins.RootObject>(plainJson);
            return myPins.posts.Select(item => new MyPins()
            {
                PostId = item.post_id,
                Message = item.message,
                VoteCount = item.vote_count,
                PinCount = item.pin_count,
                IsOwn = item.post_own.Equals("own")
            }).ToList();
        }

        public static void DeleteJodel(string postId)
        {
            DateTime dt = DateTime.UtcNow;

            string stringifiedPayload =
                @"PUT%api.go-tellm.com%443%/api/v2/posts/" + postId + "%" + Account.AccessToken + "%" + $"{dt:s}Z" + "%%";

            using (var client = new MyWebClient())
            {
                client.Headers.Add(Constants.Header.ToHeader(stringifiedPayload, true));
                client.Encoding = Encoding.UTF8;
                client.UploadData(Constants.LinkDeleteJodel.ToLink(postId), "DELETE", new byte[] { });
            }
        }

        public static class Channels
        {
            /// <summary>
            /// Follows a channel.
            /// </summary>
            /// <param name="channel"></param>
            public static void FollowChannel(string channel)
            {
                if (channel[0] == '#')
                    channel = channel.Remove(0, 1);

                DateTime dt = DateTime.UtcNow;

                string stringifiedPayload =
                    @"PUT%api.go-tellm.com%443%/api/v3/user/followChannel?access_token=" + Account.AccessToken + "%" + "&channel=" + channel + $"{dt:s}Z" + "%%";

                using (var client = new MyWebClient())
                {
                    client.Headers.Add(Constants.Header.ToHeader(stringifiedPayload));
                    client.Encoding = Encoding.UTF8;
                    client.UploadData(Constants.LinkFollowChannel.ToLink(channel), "PUT", new byte[] { });
                }
            }

            /// <summary>
            /// Unfollows a channel.
            /// </summary>
            /// <param name="channel"></param>
            public static void UnfollowChannel(string channel)
            {
                if (channel[0] == '#')
                    channel = channel.Remove(0, 1);

                DateTime dt = DateTime.UtcNow;

                string stringifiedPayload =
                    @"PUT%api.go-tellm.com%443%/api/v3/user/unfollowChannel?access_token=" + Account.AccessToken + "%" + "&channel=" + channel + $"{dt:s}Z" + "%%";

                using (var client = new MyWebClient())
                {
                    client.Headers.Add(Constants.Header.ToHeader(stringifiedPayload));
                    client.Encoding = Encoding.UTF8;
                    client.UploadData(Constants.LinkUnfollowChannel.ToLink(channel), "PUT", new byte[] { });
                }
            }

            /// <summary>
            /// Get's all Jodels from this channel.
            /// </summary>
            /// <param name="channel">The channel.</param>
            /// <returns>List&lt;ChannelJodel&gt;.</returns>
            public static List<ChannelJodel> GetJodels(string channel)
            {
                string plainJson;
                using (var client = new MyWebClient())
                {
                    client.Encoding = Encoding.UTF8;
                    plainJson = client.DownloadString(Constants.LinkGetJodelsFromChannel.ToLink(channel));
                }

                JsonGetJodelsFromChannel.RootObject myJodelsFromChannel = JsonConvert.DeserializeObject<JsonGetJodelsFromChannel.RootObject>(plainJson);
                return myJodelsFromChannel.recent.Select(item => new ChannelJodel
                {
                    PostId = item.post_id,
                    Message = item.message,
                    VoteCount = item.vote_count,
                    PinCount = item.pin_count,
                    IsOwn = item.post_own.Equals("own")
                }).ToList();
            }

            /// <summary>
            /// Get's the recommended channels.
            /// </summary>
            /// <returns>List&lt;RecommendedChannel&gt;.</returns>
            public static List<RecommendedChannel> GetRecommendedChannels()
            {
                string plainJson;
                using (var client = new MyWebClient())
                {
                    client.Encoding = Encoding.UTF8;
                    plainJson = client.DownloadString(Constants.LinkGetRecommendedChannels.ToLink());
                }

                JsonRecommendedChannels.RootObject recommendedChannels = JsonConvert.DeserializeObject<JsonRecommendedChannels.RootObject>(plainJson);
                return recommendedChannels.recommended.Select(item => new RecommendedChannel
                {
                    Name = item.channel,
                    Followers = item.followers
                }).ToList();
            }
        }
    }
}
