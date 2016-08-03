using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace JodelAPI
{
    public static class API
    {
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

        public enum Decision
        {
            Allow = 0,
            Block = 2,
            DontKnow = 1
        }


        private const string Key = "pNsUaphGEfqqZJJIKHjfxAReDqdCTIIuaIVGaowG";
        private const string ClientId = "81e8a76e-1e02-4d17-9ba0-8a7020261b26";

        public static string AccessToken = "";
        public static string Latitude = "";
        public static string Longitude = "";
        public static string CountryCode = "";
        public static string City = "";
        private static string _lastPostId = "";

        public static List<Jodels> GetFirstJodels()
        {
            string plainJson = GetPageContent("https://api.go-tellm.com/api/v2/posts/location/combo?lat=" + Latitude + "&lng=" + Longitude + "&access_token=" + AccessToken);
            JsonJodelsFirstRound.RootObject jfr = JsonConvert.DeserializeObject<JsonJodelsFirstRound.RootObject>(plainJson);
            List<Jodels> temp = new List<Jodels>(); // List<post_id,message>

            int i = 0;
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
                    Latitude = item.location.loc_coordinates.lat.ToString(),
                    Longitude = item.location.loc_coordinates.lng.ToString(),
                    LocationName = item.location.name
                };

                temp.Add(objJodels);

                i++;
            }

            _lastPostId = temp.Last().PostId; // Set the last post_id for next jodels

            return temp;
        }

        public static List<Jodels> GetNextJodels()
        {
            List<Jodels> temp = new List<Jodels>();
            for (int e = 0; e < 3; e++)
            {
                string plainJson = GetPageContent("https://api.go-tellm.com/api/v2/posts/location?lng=" + Longitude + "&lat=" + Latitude + "&after=" + _lastPostId + "&access_token=" + AccessToken + "&limit=1000000");
                JsonJodelsLastRound.RootObject jlr = JsonConvert.DeserializeObject<JsonJodelsLastRound.RootObject>(plainJson);
                int i = 0;
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
                        Latitude = item.location.loc_coordinates.lat.ToString(),
                        Longitude = item.location.loc_coordinates.lng.ToString(),
                        LocationName = item.location.name
                    };

                    temp.Add(objJodels);
                    i++;
                }

                _lastPostId = temp.Last().PostId; // Set the last post_id for next jodels
            }
            return temp;
        }

        public static List<Jodels> GetAllJodels()
        {
            var allJodels = GetFirstJodels();
            allJodels.AddRange(GetNextJodels());
            return allJodels;
        }

        public static void Upvote(string postId)
        {
            DateTime dt = DateTime.UtcNow;

            string stringifiedPayload =
                @"PUT%api.go-tellm.com%443%/api/v2/posts/" + postId + "/" + "upvote/%" + AccessToken + "%" + $"{dt:s}Z" + "%%";

            var keyByte = Encoding.UTF8.GetBytes(Key);
            var hmacsha1 = new HMACSHA1(keyByte);
            hmacsha1.ComputeHash(Encoding.UTF8.GetBytes(stringifiedPayload));

            using (var client = new WebClient())
            {
                client.Headers.Add("Content-Type", "application/json; charset=UTF-8");
                client.Headers.Add("User-Agent", "Jodel/4.12.5 Dalvik/2.1.0 (Linux; U; Android 6.0.1; Nexus 5 Build/MMB29V)"); //TODO: Randomize
                client.Headers.Add("Accept-Encoding", "gzip");
                client.Headers.Add("X-Client-Type", "android_4.12.5");
                client.Headers.Add("X-Api-Version", "0.2");
                client.Headers.Add("X-Timestamp", $"{dt:s}Z");
                client.Headers.Add("X-Authorization", "HMAC " + ByteToString(hmacsha1.Hash));
                client.Headers.Add("Authorization", "Bearer " + AccessToken);
                client.Encoding = Encoding.UTF8;
                client.UploadData(
                    "https://api.go-tellm.com/api/v2/posts/" + postId + "/" + "upvote/", "PUT", new byte[] { });
            }
        }

        public static void Downvote(string postId)
        {
            DateTime dt = DateTime.UtcNow;

            string stringifiedPayload =
                @"PUT%api.go-tellm.com%443%/api/v2/posts/" + postId + "/" + "downvote/%" + AccessToken + "%" + $"{dt:s}Z" + "%%";

            var keyByte = Encoding.UTF8.GetBytes(Key);
            var hmacsha1 = new HMACSHA1(keyByte);
            hmacsha1.ComputeHash(Encoding.UTF8.GetBytes(stringifiedPayload));

            using (var client = new WebClient())
            {
                client.Headers.Add("Content-Type", "application/json; charset=UTF-8");
                client.Headers.Add("User-Agent", "Jodel/4.12.5 Dalvik/2.1.0 (Linux; U; Android 6.0.1; Nexus 5 Build/MMB29V)"); //TODO: Randomize
                client.Headers.Add("Accept-Encoding", "gzip, deflate");
                client.Headers.Add("X-Client-Type", "android_4.12.5");
                client.Headers.Add("X-Api-Version", "0.2");
                client.Headers.Add("X-Timestamp", $"{dt:s}Z");
                client.Headers.Add("X-Authorization", "HMAC " + ByteToString(hmacsha1.Hash));
                client.Headers.Add("Authorization", "Bearer " + AccessToken);
                client.Encoding = Encoding.UTF8;
                client.UploadData(
                    "https://api.go-tellm.com/api/v2/posts/" + postId + "/" + "downvote/", "PUT", new byte[] { });
            }
        }

        public static int GetKarma()
        {
            string resp = GetPageContent("https://api.go-tellm.com/api/v2/users/karma?access_token=" + AccessToken);
            string result = resp.Substring(resp.LastIndexOf(':') + 1);
            return Convert.ToInt32(result.Replace("}", "").Replace("\"", ""));
        }

        public static void PostJodel(string message, PostColor colorParam = PostColor.Random, string postID = null)
        {
            DateTime dt = DateTime.UtcNow;

            var color = GetColor(colorParam);

            string jsonCommentFragment = string.Empty;
            if (postID != null)
            {
                jsonCommentFragment = @"""ancestor"": """ + postID + @""", ";
            }

            string stringifiedPayload = @"POST%api.go-tellm.com%443%/api/v2/posts/%" + AccessToken + "%" + $"{dt:s}Z" +
                                        @"%%{""color"": """ + color + @""", " + jsonCommentFragment +
                                        @"""message"": """ + message + @""", ""location"": {""loc_accuracy"": 1, ""city"": """ + City +
                                        @""", ""loc_coordinates"": {""lat"": " + Latitude + @", ""lng"": " + Longitude +
                                        @"}, ""country"": """ + CountryCode + @""", ""name"": """ + City + @"""}}";

            string payload = @"{""color"": """ + color + @""", " + jsonCommentFragment +
                             @"""message"": """ + message + @""", ""location"": {""loc_accuracy"": 1, ""city"": """ + City +
                             @""", ""loc_coordinates"": " + @"{""lat"": " + Latitude + @", ""lng"": " + Longitude +
                             @"}, ""country"": """ + CountryCode + @""", ""name"": """ + City + @"""}}";

            var keyByte = Encoding.UTF8.GetBytes(Key);
            using (var hmacsha1 = new HMACSHA1(keyByte))
            {
                hmacsha1.ComputeHash(Encoding.UTF8.GetBytes(stringifiedPayload));

                GetPageContentPost("https://api.go-tellm.com/api/v2/posts/", payload, true, ByteToString(hmacsha1.Hash), $"{dt:s}Z");
            }
        }

        public static List<Comments> GetComments(string postId)
        {
            string plainJson = GetPageContent("https://api.go-tellm.com/api/v2/posts/" + postId + "?access_token=" + AccessToken);
            JsonComments.RootObject com = JsonConvert.DeserializeObject<JsonComments.RootObject>(plainJson);

            return com.children.Select(c => new Comments()
            {
                PostId = c.post_id,
                Message = c.message,
                UserHandle = c.user_handle,
                VoteCount = c.vote_count
            }).ToList();
        }

        public static List<ModerationQueue> GetModerationQueue()
        {
            string plainJson = GetPageContent("https://api.go-tellm.com/api/v3/moderation/?access_token=" + AccessToken);
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

        public static string GenerateAccessToken()
        {
            DateTime dt = DateTime.UtcNow;

            string deviceUid = Sha256(RandomString(5, true));

            string stringifiedPayload = @"POST%api.go-tellm.com%443%/api/v2/users/%%" + $"{dt:s}Z" +
                                        @"%%{""device_uid"": """ + deviceUid + @""", ""location"": {""city"": """ + City +
                                        @""", ""loc_accuracy"": 100, ""loc_coordinates"": {""lat"": " + Latitude +
                                        @", ""lng"": " + Longitude + @"}, ""country"": """ + CountryCode + @"""}, " +
                                        @"""client_id"": """ + ClientId + @"""}";

            string payload = @"{""device_uid"": """ + deviceUid + @""", ""location"": {""city"": """ + City +
                             @""", ""loc_accuracy"": 100, ""loc_coordinates"": " + @"{""lat"": " + Latitude +
                             @", ""lng"": " + Longitude + @"}, ""country"": """ + CountryCode +
                             @"""}, ""client_id"": """ + ClientId + @"""}";

            var keyByte = Encoding.UTF8.GetBytes(Key);
            using (var hmacsha1 = new HMACSHA1(keyByte))
            {
                hmacsha1.ComputeHash(Encoding.UTF8.GetBytes(stringifiedPayload));

                return GetPageContentPost("https://api.go-tellm.com/api/v2/users/", payload, false,
                    ByteToString(hmacsha1.Hash), $"{dt:s}Z");
            }
        }

        public static void FlagJodel(int taskId, Decision decision)
        {
            DateTime dt = DateTime.UtcNow;

            string dec = Convert.ChangeType(decision, decision.GetTypeCode())?.ToString(); // get int from enum.
            string stringifiedPayload = @"{	""task_id"": """ + taskId + 
                                        @""", ""location"": { ""country"": """ + CountryCode + 
                                        @""", ""name"": ""unknown"", ""loc_accuracy"": ""1"", ""loc_coordinates"": { ""lat"": """ + Latitude + 
                                        @""", ""lng"": """ + Longitude + 
                                        @""" }, ""city"": """ + City + 
                                        @""" }, ""decision"": """ + dec + 
                                        @""" }";


            var keyByte = Encoding.UTF8.GetBytes(Key);
            var hmacsha1 = new HMACSHA1(keyByte);
            hmacsha1.ComputeHash(Encoding.UTF8.GetBytes(stringifiedPayload));

            using (var client = new WebClient())
            {
                client.Headers.Add("Content-Type", "application/json; charset=UTF-8");
                client.Headers.Add("User-Agent", "Jodel/4.12.5 Dalvik/2.1.0 (Linux; U; Android 6.0.1; Nexus 5 Build/MMB29V)"); //TODO: Randomize
                client.Headers.Add("Accept-Encoding", "gzip");
                client.Headers.Add("X-Client-Type", "android_4.12.5");
                client.Headers.Add("X-Api-Version", "0.2");
                client.Headers.Add("X-Timestamp", $"{dt:s}Z");
                client.Headers.Add("X-Authorization", "HMAC " + ByteToString(hmacsha1.Hash));
                client.Encoding = Encoding.UTF8;
                client.UploadString(
                    "https://api.go-tellm.com/api/v3/moderation?access_token=" + AccessToken, stringifiedPayload);
            }
        }

        public static List<Jodels> FilterByChannel(List<Jodels> jodels, string channel) // Get's all jodels containing the word
        {
            if (channel[0] == '#')
            {
                channel = channel.Remove(0, 1);
            }

            List<Jodels> temp = (
                from jodel in jodels
                where jodel.Message.Contains(channel)
                select new Jodels()
                {
                    PostId = jodel.PostId, HexColor = jodel.HexColor, IsImage = jodel.IsImage, Latitude = jodel.Latitude, Longitude = jodel.Longitude, LocationName = jodel.LocationName, Message = jodel.Message, VoteCount = jodel.VoteCount
                }).ToList();

            return temp;
        }

        public static Coordinates GetLocation()
        {
            return GetCoords();
        }

        public static void SetCurrentLocation()
        {
            var coord = GetCoords();

            Latitude = coord.Latitude;
            Longitude = coord.Longitude;
        }

        private static string ByteToString(byte[] buff)
        {
            return buff.Aggregate("", (current, t) => current + t.ToString("X2"));
        }

        private static string GetPageContent(string link)
        {
            string html;
            WebRequest request = WebRequest.Create(link);
            WebResponse response = request.GetResponse();
            Stream data = response.GetResponseStream();
            using (StreamReader sr = new StreamReader(data))
            {
                html = sr.ReadToEnd();
            }
            return html;
        }

        private static string GetPageContentPost(string link, string post, bool bearer, string hmac, string timestamp)
        {
            var request = (HttpWebRequest)WebRequest.Create(link);

            var data = Encoding.UTF8.GetBytes(post);

            request.Method = "POST";
            request.ContentType = "application/json; charset=UTF-8";
            request.ContentLength = data.LongLength;
            request.UserAgent = "Jodel/4.12.5 Dalvik/2.1.0 (Linux; U; Android 6.0.1; Nexus 5 Build/MMB29V)"; //TODO: Randomize
            request.KeepAlive = true;
            request.Headers.Add("Accept-Encoding", "gzip");
            request.Headers.Add("X-Client-Type", "android_4.12.5");
            request.Headers.Add("X-Api-Version", "0.2");
            if (timestamp != null)
                request.Headers.Add("X-Timestamp", timestamp);
            if (hmac != null)
                request.Headers.Add("X-Authorization", "HMAC " + hmac);

            if (bearer)
            {
                request.Headers.Add("Authorization", "Bearer " + AccessToken);
            }
            request.ServicePoint.Expect100Continue = false;
            request.AuthenticationLevel = AuthenticationLevel.None;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();

            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            if (hmac != null)
            {
                var responseJson = JsonConvert.DeserializeObject<dynamic>(responseString); // ugly solution, may throw exception if no access token is responded
                responseString = responseJson.access_token;
            }
            return responseString;
        }

        private static string Sha256(string value)
        {
            StringBuilder sb = new StringBuilder();

            using (SHA256 hash = SHA256.Create())
            {
                Encoding enc = Encoding.UTF8;
                byte[] result = hash.ComputeHash(enc.GetBytes(value));

                foreach (byte b in result)
                    sb.Append(b.ToString("x2"));
            }

            return sb.ToString();
        }

        private static string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            for (int i = 1; i < size + 1; i++)
            {
                var ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            return lowerCase ? builder.ToString().ToLower() : builder.ToString();
        }

        private static string GetColor(PostColor c)
        {
            switch (c)
            {
                case PostColor.Red:
                    return "DD5F5F";
                case PostColor.Orange:
                    return "FF9908";
                case PostColor.Yellow:
                    return "FFBA00";
                case PostColor.Blue:
                    return "DD5F5F";
                case PostColor.Bluegreyish:
                    return "8ABDB0";
                case PostColor.Green:
                    return "9EC41C";
                case PostColor.Random:
                    return "FFFFFF";
                default:
                    throw new ArgumentOutOfRangeException(nameof(c), c, null);
            }
        }

        private static Coordinates GetCoords()
        {
            var watcher = new GeoCoordinateWatcher();
            Coordinates coord = new Coordinates();

            watcher.PositionChanged += (sender, e) =>
            {
                var coordinate = e.Position.Location;
                coord.Latitude = coordinate.Latitude.ToString(CultureInfo.InvariantCulture);
                coord.Longitude = coordinate.Longitude.ToString(CultureInfo.InvariantCulture);
                watcher.Stop();
            };

            watcher.Start();

            return coord;
        }
    }
}