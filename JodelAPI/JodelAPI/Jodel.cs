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
    public class Jodel
    {
        private static User _user;
        public Moderation Moderation = new Moderation();
        public Account Account;
        public Location Location;

        public Jodel(string accessToken, string longitude, string latitude, string city, string countryCode, string googleApiToken = "")
        {
            _user = new User(accessToken, latitude, longitude, countryCode, city, googleApiToken);
            Helpers._user = _user;
            Account = new Account(_user);
            Location = new Location(_user);
        }

        public Jodel(User user)
        {
            _user = user;
            Helpers._user = user;
            Account = new Account(user);
            Location = new Location(user);
        }

        /// <summary>
        ///     Colors for Jodels
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
        ///     Methods to sort List<Jodels>
        /// </summary>
        public enum SortMethod
        {
            MostCommented,
            Top
        }

        private string _lastPostId = "";

        /// <summary>
        ///     Gets the first amount of Jodels (internal usage)
        /// </summary>
        /// <returns>List&lt;Jodels&gt;.</returns>
        public List<Jodels> GetFirstJodels(string accessToken = null)
        {
            string plainJson;
            using (var client = new MyWebClient())
            {
                client.Encoding = Encoding.UTF8;
                plainJson = client.DownloadString(Constants.LinkFirstJodels.ToLink(accessToken));
            }
            JsonJodelsFirstRound.RootObject jfr =
                JsonConvert.DeserializeObject<JsonJodelsFirstRound.RootObject>(plainJson);
            List<Jodels> temp = new List<Jodels>(); // List<post_id,message>

            foreach (var item in jfr.recent)
            {
                string image_url = "";
                bool isUrl = false;

                if (item.image_url != null)
                {
                    image_url = "http:" + item.image_url;
                    isUrl = true;
                }

                Jodels objJodels = new Jodels
                {
                    PostId = item.post_id,
                    Message = item.message,
                    HexColor = item.color,
                    IsImage = isUrl,
                    ImageUrl = image_url,
                    VoteCount = item.vote_count,
                    LocationName = item.location.name,
                    CommentsCount = item.child_count ?? 0,
                    ChildCount = item.child_count ?? 0,
                    CreatedAt = DateTime.ParseExact(item.created_at.Replace("Z", "").Replace("T", " "), "yyyy-MM-dd HH:mm:ss.fff", null),
                    UpdatedAt = DateTime.ParseExact(item.updated_at.Replace("Z", "").Replace("T", " "), "yyyy-MM-dd HH:mm:ss.fff", null),
                    Distance = item.distance,
                    IsNotificationEnabled = item.notifications_enabled,
                    PinCount = item.pin_count,
                    PostOwn = item.post_own,
                    UserHandle = item.user_handle
                };

                temp.Add(objJodels);
            }

            _lastPostId = temp.Last().PostId; // Set the last post_id for next jodels

            return temp;
        }

        /// <summary>
        ///     Gets the second amount of Jodels (internal usage)
        /// </summary>
        /// <returns>List&lt;Jodels&gt;.</returns>
        public List<Jodels> GetNextJodels(string accessToken = null)
        {
            List<Jodels> temp = new List<Jodels>();
            for (int e = 0; e < 3; e++)
            {
                string plainJson;
                using (var client = new MyWebClient())
                {
                    client.Encoding = Encoding.UTF8;
                    plainJson = client.DownloadString(Constants.LinkSecondJodels.ToLinkSecond(_lastPostId, accessToken));
                }
                JsonJodelsLastRound.RootObject jlr = JsonConvert.DeserializeObject<JsonJodelsLastRound.RootObject>(plainJson);
                foreach (var item in jlr.posts)
                {
                    string image_url = "";
                    bool isUrl = false;

                    if (item.image_url != null)
                    {
                        image_url = "http:" + item.image_url;
                        isUrl = true;
                    }

                    Jodels objJodels = new Jodels
                    {
                        PostId = item.post_id,
                        Message = item.message,
                        HexColor = item.color,
                        IsImage = isUrl,
                        ImageUrl = image_url,
                        VoteCount = item.vote_count,
                        LocationName = item.location.name,
                        CommentsCount = item.child_count ?? 0,
                        ChildCount = item.child_count ?? 0,
                        CreatedAt = DateTime.ParseExact(item.created_at.Replace("Z", "").Replace("T", " "), "yyyy-MM-dd HH:mm:ss.fff", null),
                        UpdatedAt = DateTime.ParseExact(item.updated_at.Replace("Z", "").Replace("T", " "), "yyyy-MM-dd HH:mm:ss.fff", null),
                        Distance = item.distance,
                        IsNotificationEnabled = item.notifications_enabled,
                        PinCount = item.pin_count,
                        PostOwn = item.post_own,
                        UserHandle = item.user_handle
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
        ///     Gets all jodels.
        /// </summary>
        /// <returns>List&lt;Jodels&gt;.</returns>
        public List<Jodels> GetAllJodels()
        {
            var allJodels = GetFirstJodels();
            allJodels.AddRange(GetNextJodels());
            return allJodels;
        }

        /// <summary>
        ///     Gets all jodels.
        /// </summary>
        /// <returns>List&lt;Jodels&gt;.</returns>
        public List<Jodels> GetAllJodels(string accessToken)
        {
            var allJodels = GetFirstJodels(accessToken);
            allJodels.AddRange(GetNextJodels(accessToken));
            return allJodels;
        }

        /// <summary>
        ///     Upvotes the specified post identifier (Jodel).
        /// </summary>
        /// <param name="postId">The post identifier.</param>
        public void Upvote(string postId)
        {
            DateTime dt = DateTime.UtcNow;

            string stringifiedPayload =
                @"PUT%api.go-tellm.com%443%/api/v2/posts/" + postId + "/" + "upvote/%" + _user.AccessToken + "%" +
                $"{dt:s}Z" + "%%";

            using (var client = new MyWebClient())
            {
                client.Headers.Add(Constants.Header.ToHeader(stringifiedPayload, dt, true));
                client.Encoding = Encoding.UTF8;
                client.UploadData(Constants.LinkUpvoteJodel.ToLink(_user.AccessToken, postId), "PUT", new byte[] { });
            }
        }

        /// <summary>
        ///     Downvotes the specified post identifier (Jodel).
        /// </summary>
        /// <param name="postId">The post identifier.</param>
        public void Downvote(string postId)
        {
            DateTime dt = DateTime.UtcNow;

            string stringifiedPayload =
                @"PUT%api.go-tellm.com%443%/api/v2/posts/" + postId + "/" + "downvote/%" + _user.AccessToken + "%" +
                $"{dt:s}Z" + "%%";

            using (var client = new MyWebClient())
            {
                client.Headers.Add(Constants.Header.ToHeader(stringifiedPayload, dt, true));
                client.Encoding = Encoding.UTF8;
                client.UploadData(Constants.LinkDownvoteJodel.ToLink(_user.AccessToken, postId), "PUT", new byte[] { });
            }
        }

        /// <summary>
        ///     Posts a jodel.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="colorParam">The color parameter.</param>
        /// <param name="postId">The post identifier.</param>
        public string PostJodel(string message, PostColor colorParam = PostColor.Random, string postId = null)
        {
            DateTime dt = DateTime.UtcNow;

            var color = Helpers.GetColor(colorParam);

            string jsonCommentFragment = string.Empty;
            if (postId != null)
            {
                jsonCommentFragment = @"""ancestor"": """ + postId + @""", ";
            }

            string stringifiedPayload = @"POST%api.go-tellm.com%443%/api/v2/posts/%" + _user.AccessToken + "%" +
                                        $"{dt:s}Z" +
                                        @"%%{""color"": """ + color + @""", " + jsonCommentFragment +
                                        @"""message"": """ + message +
                                        @""", ""location"": {""loc_accuracy"": 1, ""city"": """ + _user.City +
                                        @""", ""loc_coordinates"": {""lat"": " + _user.Latitude + @", ""lng"": " +
                                        _user.Longitude +
                                        @"}, ""country"": """ + _user.CountryCode + @""", ""name"": """ + _user.City +
                                        @"""}}";

            string payload = @"{""color"": """ + color + @""", " + jsonCommentFragment +
                             @"""message"": """ + message + @""", ""location"": {""loc_accuracy"": 1, ""city"": """ +
                             _user.City +
                             @""", ""loc_coordinates"": " + @"{""lat"": " + _user.Latitude + @", ""lng"": " +
                             _user.Longitude +
                             @"}, ""country"": """ + _user.CountryCode + @""", ""name"": """ + _user.City + @"""}}";

            var keyByte = Encoding.UTF8.GetBytes(Constants.Key);
            using (var hmacsha1 = new HMACSHA1(keyByte))
            {
                hmacsha1.ComputeHash(Encoding.UTF8.GetBytes(stringifiedPayload));

                using (var client = new MyWebClient())
                {
                    client.Headers.Add(Constants.Header.ToHeader(stringifiedPayload, dt, true));
                    client.Encoding = Encoding.UTF8;
                    string newJodels = client.UploadString(Constants.LinkPostJodel, payload);
                    JsonPostJodels.RootObject temp = JsonConvert.DeserializeObject<JsonPostJodels.RootObject>(newJodels);
                    return temp.posts[0].post_id;
                }
            }
        }

        //TODO: FIXME!
        //public static string PostJodel(Image image, PostColor colorParam = PostColor.Random, string postId = null)
        //{
        //    DateTime dt = DateTime.UtcNow;

        //    var color = Helpers.GetColor(colorParam);

        //    ImageConverter ic = new ImageConverter();
        //    byte[] buffer = (byte[]) ic.ConvertTo(image, typeof(byte[]));
        //    string base64 = Convert.ToBase64String(buffer, Base64FormattingOptions.InsertLineBreaks);

        //    string stringifiedPayload = @"POST%api.go-tellm.com%443%/api/v2/posts/%" + _user.AccessToken + "%" +
        //                                $"{dt:s}Z" +
        //                                "%%{\"color\": \"" + color +
        //                                "\", \"message\": \"\", \"location\": {\"loc_accuracy\": 1, \"city\": \"" +
        //                                _user.City +
        //                                "\", \"loc_coordinates\": " + "{\"lat\": " + _user.Latitude + ", \"lng\": " +
        //                                _user.Longitude + "}, \"country\": \"" + _user.CountryCode +
        //                                "\", \"name\": \"" + _user.City + "\"}, \"image\": \"" + base64 + "\"}";

        //    string payload = "{\"color\": \"" + color +
        //                     "\", \"message\": \"\", \"location\": {\"loc_accuracy\": 1, \"city\": \"" + _user.City +
        //                     "\", \"loc_coordinates\": " + "{\"lat\": " + _user.Latitude + ", \"lng\": " +
        //                     _user.Longitude + "}, \"country\": \"" + _user.CountryCode +
        //                     "\", \"name\": \"" + _user.City + "\"}, \"image\": \"" + base64 + "\"}";

        //    var keyByte = Encoding.UTF8.GetBytes(Constants.Key);
        //    using (var hmacsha1 = new HMACSHA1(keyByte))
        //    {
        //        hmacsha1.ComputeHash(Encoding.UTF8.GetBytes(stringifiedPayload));

        //        using (var client = new MyWebClient())
        //        {
        //            client.Headers.Add(Constants.Header.ToHeader(stringifiedPayload, true));
        //            client.Encoding = Encoding.UTF8;
        //            string newJodels = client.UploadString(Constants.LinkPostImage, payload);
        //            JsonPostJodels.RootObject temp = JsonConvert.DeserializeObject<JsonPostJodels.RootObject>(newJodels);
        //            return temp.posts[0].post_id;
        //        }
        //    }
        //}

        /// <summary>
        ///     Gets the comments.
        /// </summary>
        /// <param name="postId">The post identifier.</param>
        /// <returns>List&lt;Comments&gt;.</returns>
        public List<Comments> GetComments(string postId)
        {
            string plainJson;
            using (var client = new MyWebClient())
            {
                client.Encoding = Encoding.UTF8;
                plainJson = client.DownloadString(Constants.LinkGetComments.ToLink(_user.AccessToken, postId));
            }
            JsonComments.RootObject com = JsonConvert.DeserializeObject<JsonComments.RootObject>(plainJson);

            var result = new List<Comments>();

            if (com.children != null)
            {
                foreach (var child in com.children)
                {
                    string image_url = "";
                    bool isUrl = false;

                    if (child.image_url != null)
                    {
                        image_url = "http:" + child.image_url;
                        isUrl = true;
                    }

                    result.Add(new Comments
                    {
                        Message = child.message,
                        PostId = child.post_id,
                        CreatedAt = DateTime.ParseExact(child.created_at.Replace("Z", "").Replace("T", " "), "yyyy-MM-dd HH:mm:ss.fff", null),
                        UpdatedAt = DateTime.ParseExact(child.updated_at.Replace("Z", "").Replace("T", " "), "yyyy-MM-dd HH:mm:ss.fff", null),
                        IsImage = isUrl,
                        ImageUrl = image_url,
                        UserHandle = child.user_handle,
                        VoteCount = child.vote_count
                    });
                }
            }

            return result;
        }

        /// <summary>
        ///     Gets my jodels.
        /// </summary>
        /// <returns>List&lt;MyJodels&gt;.</returns>
        public List<MyJodels> GetMyJodels()
        {
            string plainJson;
            using (var client = new MyWebClient())
            {
                client.Encoding = Encoding.UTF8;
                plainJson = client.DownloadString(Constants.LinkGetMyJodels.ToLink());
            }

            JsonMyJodels.RootObject myJodels = JsonConvert.DeserializeObject<JsonMyJodels.RootObject>(plainJson);
            return myJodels.posts.Select(item => new MyJodels
            {
                PostId = item.post_id,
                Message = item.message,
                HexColor = item.color,
                VoteCount = item.vote_count,
                Latitude = item.location.loc_coordinates.lat,
                Longitude = item.location.loc_coordinates.lng,
                LocationName = item.location.name,
                ChildCount = item.child_count,
                CreatedAt = DateTime.ParseExact(item.created_at.Replace("Z", "").Replace("T", " "), "yyyy-MM-dd HH:mm:ss.fff", null),
                UpdatedAt = DateTime.ParseExact(item.updated_at.Replace("Z", "").Replace("T", " "), "yyyy-MM-dd HH:mm:ss.fff", null),
                Distance = item.distance,
                IsNotificationEnabled = item.notifications_enabled,
                PinCount = item.pin_count,
                PostOwn = item.post_own,
                UserHandle = item.user_handle
            }).ToList();
        }

        /// <summary>
        ///     Gets my comments.
        /// </summary>
        /// <returns>List&lt;MyComments&gt;.</returns>
        public List<MyComments> GetMyComments()
        {
            string plainJson;
            using (var client = new MyWebClient())
            {
                client.Encoding = Encoding.UTF8;
                plainJson = client.DownloadString(Constants.LinkGetMyComments.ToLink());
            }

            JsonMyComments.RootObject myComments = JsonConvert.DeserializeObject<JsonMyComments.RootObject>(plainJson);
            return myComments.posts.Select(item => new MyComments
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
        ///     Gets my votes.
        /// </summary>
        /// <returns>List&lt;MyVotes&gt;.</returns>
        public List<MyVotes> GetMyVotes()
        {
            string plainJson;
            using (var client = new MyWebClient())
            {
                client.Encoding = Encoding.UTF8;
                plainJson = client.DownloadString(Constants.LinkGetMyVotes.ToLink());
            }

            JsonMyVotes.RootObject myVotes = JsonConvert.DeserializeObject<JsonMyVotes.RootObject>(plainJson);
            return myVotes.posts.Select(item => new MyVotes
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
        ///     Sorts the jodels.
        /// </summary>
        /// <param name="jodels">The jodels.</param>
        /// <param name="method">The method.</param>
        /// <returns>List&lt;Jodels&gt;.</returns>
        public List<Jodels> Sort(List<Jodels> jodels, SortMethod method)
        {
            return method == SortMethod.MostCommented
                ? jodels.OrderByDescending(o => o.CommentsCount).ToList()
                : jodels.OrderByDescending(o => o.VoteCount).ToList();
        }

        /// <summary>
        ///     Reports the jodel.
        /// </summary>
        /// <param name="postId"></param>
        /// <param name="reason"></param>
        public void ReportJodel(string postId, Moderation.Reason reason)
        {
            string rea = Convert.ChangeType(reason, reason.GetTypeCode())?.ToString(); // get int from enum.
            string stringifiedPayload = @"{""reason_id"":" + rea + "}";

            using (var client = new MyWebClient())
            {
                client.Headers.Add(Constants.Header.ToHeader(stringifiedPayload, DateTime.UtcNow, true));
                client.Encoding = Encoding.UTF8;
                client.UploadData(Constants.LinkReportJodel.ToLink(_user.AccessToken, postId), "PUT", new byte[] { });
            }
        }

        /// <summary>
        ///     Pins a Jodel.
        /// </summary>
        /// <param name="postId"></param>
        public void PinJodel(string postId)
        {
            DateTime dt = DateTime.UtcNow;

            string stringifiedPayload =
                @"PUT%api.go-tellm.com%443%/api/v2/posts/" + postId + "/" + "pin?access_token=/%" + _user.AccessToken +
                "%" + $"{dt:s}Z" + "%%";

            using (var client = new MyWebClient())
            {
                client.Headers.Add(Constants.Header.ToHeader(stringifiedPayload, DateTime.UtcNow));
                client.Encoding = Encoding.UTF8;
                client.UploadData(Constants.LinkPinJodel.ToLink(_user.AccessToken, postId), "PUT", new byte[] { });
            }
        }

        /// <summary>
        ///     Get's all pinned Jodels.
        /// </summary>
        /// <returns>List&lt;MyPins&gt;.</returns>
        public List<MyPins> GetMyPins()
        {
            string plainJson;
            using (var client = new MyWebClient())
            {
                client.Encoding = Encoding.UTF8;
                plainJson = client.DownloadString(Constants.LinkMyPins.ToLink());
            }

            JsonMyPins.RootObject myPins = JsonConvert.DeserializeObject<JsonMyPins.RootObject>(plainJson);
            return myPins.posts.Select(item => new MyPins
            {
                PostId = item.post_id,
                Message = item.message,
                VoteCount = item.vote_count,
                PinCount = item.pin_count,
                IsOwn = item.post_own.Equals("own")
            }).ToList();
        }

        public void DeleteJodel(string postId)
        {
            DateTime dt = DateTime.UtcNow;

            string stringifiedPayload =
                @"PUT%api.go-tellm.com%443%/api/v2/posts/" + postId + "%" + _user.AccessToken + "%" + $"{dt:s}Z" +
                "%%";

            using (var client = new MyWebClient())
            {
                client.Headers.Add(Constants.Header.ToHeader(stringifiedPayload, DateTime.UtcNow, true));
                client.Encoding = Encoding.UTF8;
                client.UploadData(Constants.LinkDeleteJodel.ToLink(_user.AccessToken, postId), "DELETE", new byte[] { });
            }
        }

        public class Channels
        {
            /// <summary>
            ///     Follows a channel.
            /// </summary>
            /// <param name="channel"></param>
            public void FollowChannel(string channel)
            {
                if (channel[0] == '#')
                    channel = channel.Remove(0, 1);

                DateTime dt = DateTime.UtcNow;

                string stringifiedPayload =
                    @"PUT%api.go-tellm.com%443%/api/v3/user/followChannel?access_token=" + _user.AccessToken + "%" +
                    "&channel=" + channel + $"{dt:s}Z" + "%%";

                using (var client = new MyWebClient())
                {
                    client.Headers.Add(Constants.Header.ToHeader(stringifiedPayload, DateTime.UtcNow));
                    client.Encoding = Encoding.UTF8;
                    client.UploadData(Constants.LinkFollowChannel.ToLink(channel), "PUT", new byte[] { });
                }
            }

            /// <summary>
            ///     Unfollows a channel.
            /// </summary>
            /// <param name="channel"></param>
            public void UnfollowChannel(string channel)
            {
                if (channel[0] == '#')
                    channel = channel.Remove(0, 1);

                DateTime dt = DateTime.UtcNow;

                string stringifiedPayload =
                    @"PUT%api.go-tellm.com%443%/api/v3/user/unfollowChannel?access_token=" + _user.AccessToken + "%" +
                    "&channel=" + channel + $"{dt:s}Z" + "%%";

                using (var client = new MyWebClient())
                {
                    client.Headers.Add(Constants.Header.ToHeader(stringifiedPayload, DateTime.UtcNow));
                    client.Encoding = Encoding.UTF8;
                    client.UploadData(Constants.LinkUnfollowChannel.ToLink(channel), "PUT", new byte[] { });
                }
            }

            /// <summary>
            ///     Get's all Jodels from this channel.
            /// </summary>
            /// <param name="channel">The channel.</param>
            /// <returns>List&lt;ChannelJodel&gt;.</returns>
            public List<ChannelJodel> GetJodels(string channel)
            {
                string plainJson;
                using (var client = new MyWebClient())
                {
                    client.Encoding = Encoding.UTF8;
                    plainJson = client.DownloadString(Constants.LinkGetJodelsFromChannel.ToLink(channel));
                }

                JsonGetJodelsFromChannel.RootObject myJodelsFromChannel =
                    JsonConvert.DeserializeObject<JsonGetJodelsFromChannel.RootObject>(plainJson);
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
            ///     Get's all Jodels from this channel.
            /// </summary>
            /// <param name="channel">The channel.</param>
            /// <returns>List&lt;ChannelJodel&gt;.</returns>
            public async Task<List<ChannelJodel>> GetJodelsAsync(string channel)
            {
                string plainJson;
                using (var client = new MyWebClient())
                {
                    client.Encoding = Encoding.UTF8;
                    var taskResult = await Task.FromResult(
                        client.DownloadStringTaskAsync(new Uri(Constants.LinkGetJodelsFromChannel.ToLink(channel))));
                    plainJson = taskResult.Result;
                }

                JsonGetJodelsFromChannel.RootObject myJodelsFromChannel =
                    JsonConvert.DeserializeObject<JsonGetJodelsFromChannel.RootObject>(plainJson);
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
            ///     Get's the recommended channels.
            /// </summary>
            /// <returns>List&lt;RecommendedChannel&gt;.</returns>
            public List<RecommendedChannel> GetRecommendedChannels()
            {
                string plainJson;
                using (var client = new MyWebClient())
                {
                    client.Encoding = Encoding.UTF8;
                    plainJson = client.DownloadString(Constants.LinkGetRecommendedChannels.ToLink());
                }

                JsonRecommendedChannels.RootObject recommendedChannels =
                    JsonConvert.DeserializeObject<JsonRecommendedChannels.RootObject>(plainJson);
                return recommendedChannels.recommended.Select(item => new RecommendedChannel
                {
                    Name = item.channel,
                    Followers = item.followers
                }).ToList();
            }
        }
    }
}