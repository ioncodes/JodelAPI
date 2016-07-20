using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.IO;
using System.Security.Cryptography;
using System.Net.Security;

namespace JodelAPI
{
    public static class API
    {
        public static string accessToken = "";
        public static string latitude = "";
        public static string longitude = "";
        public static string countryCode = "";
        public static string city = "";
        private const string key = "XpOTPTszrtNioQQAnrREKwjtWESeUMlPQcsxmbkC";

        private static List<Tuple<string, string, string, bool, int, string, string, Tuple<string>>> jodelCache = new List<Tuple<string, string, string, bool, int, string, string, Tuple<string>>>(); // postid, message, hexcolor, isImage, votecount, lat, lng, name
        private static string lastPostID = "";


        public static List<Tuple<string, string, string, bool, int, string, string, Tuple<string>>> GetFirstJodels()
        {
            string plainJson = GetPageContent("https://api.go-tellm.com/api/v2/posts/location/combo?lat=" + latitude + "&lng=" + longitude + "&access_token=" + accessToken);
            JodelsFirstRound.RootObject jfr = JsonConvert.DeserializeObject<JodelsFirstRound.RootObject>(plainJson);
            List<Tuple<string, string, string, bool, int, string, string, Tuple<string>>> temp = new List<Tuple<string, string, string, bool, int, string, string, Tuple<string>>>(); // List<post_id,message>
            int i = 0;
            foreach (var item in jfr.recent)
            {
                string msg = item.message;
                bool isURL = false;
                if (msg == "Jodel")
                {
                    msg = "http:"+item.image_url;
                    isURL = true;
                }

                temp.Add(new Tuple<string, string, string, bool, int, string, string, Tuple<string>>(item.post_id, msg, item.color, isURL, item.vote_count, item.location.loc_coordinates.lat.ToString(), item.location.loc_coordinates.lng.ToString(), new Tuple<string>(item.location.name)));

                i++;
            }

            lastPostID = FilterItem(temp, temp.IndexOf(temp.Last()), false); // Set the last post_id for next jodels

            return temp;
        }

        public static List<Tuple<string, string, string, bool, int, string, string, Tuple<string>>> GetNextJodels()
        {
            List<Tuple<string, string, string, bool, int, string, string, Tuple<string>>> temp = new List<Tuple<string, string, string, bool, int, string, string, Tuple<string>>>(); // List<counter,post_id,message>
            for (int e = 0; e < 3; e++)
            {
                string plainJson = GetPageContent("https://api.go-tellm.com/api/v2/posts/location?lng=" + longitude + "&lat=" + latitude + "&after=" + lastPostID + "&access_token=" + accessToken + "&limit=1000000");
                JodelsLastRound.RootObject jlr = JsonConvert.DeserializeObject<JodelsLastRound.RootObject>(plainJson);
                int i = 0;
                foreach (var item in jlr.posts)
                {
                    string msg = item.message;
                    bool isURL = false;
                    if (msg == "Jodel")
                    {
                        msg = "http:" + item.image_url; // WELL THERE IS NO IMAGE_URL!!!!???
                        isURL = true;
                    }

                    temp.Add(new Tuple<string, string, string, bool, int, string, string, Tuple<string>>(item.post_id, msg, item.color, isURL, item.vote_count, item.location.loc_coordinates.lat.ToString(), item.location.loc_coordinates.lng.ToString(), new Tuple<string>(item.location.name)));
                    i++;
                }

                lastPostID = FilterItem(temp, temp.IndexOf(temp.Last()), false); // Set the last post_id for next jodels
            }
            return temp;
        }

        public static List<Tuple<string, string, string, bool, int, string, string, Tuple<string>>> GetAllJodels()
        {
            List<Tuple<string, string, string, bool, int, string, string, Tuple<string>>> allJodels = new List<Tuple<string, string, string, bool, int, string, string, Tuple<string>>>();
            allJodels = GetFirstJodels();
            allJodels.AddRange(GetNextJodels());
            jodelCache = allJodels;
            return allJodels;
        }

        public static string FilterItem(List<Tuple<string, string, string, bool, int, string, string, Tuple<string>>> unfiltered, int index, bool filterMessage)
        {
            if (!filterMessage)
            {
                return unfiltered[index].Item1;
            }
            else
            {
                return unfiltered[index].Item2;
            }
        }

        public static void Upvote(string postID)
        {
            DateTime dt = DateTime.UtcNow;

            string stringified_payload =
                @"PUT%api.go-tellm.com%443%/api/v2/posts/" + postID + "/" + "upvote/%" + accessToken + "%" + $"{dt:s}Z" + "%%";

            var keyByte = Encoding.UTF8.GetBytes(key);
            var hmacsha1 = new HMACSHA1(keyByte);
            hmacsha1.ComputeHash(Encoding.UTF8.GetBytes(stringified_payload));

            using (var client = new WebClient())
            {
                client.Headers.Add("Content-Type", "application/json");
                client.Headers.Add("User-Agent", "Jodel/4.11.2 Dalvik/2.1.0 (Linux; U; Android 6.0.1; Nexus 5 Build/MMB29V)"); //TODO: Randomize
                client.Headers.Add("Accept", "*/*");
                client.Headers.Add("Accept-Encoding", "gzip, deflate");
                client.Headers.Add("X-Client-Type", "android_4.11.2");
                client.Headers.Add("X-Api-Version", "0.2");
                client.Headers.Add("X-Timestamp", $"{dt:s}Z");
                client.Headers.Add("X-Authorization", "HMAC " + ByteToString(hmacsha1.Hash));
                client.Headers.Add("Authorization", "Bearer " + accessToken);
                client.Encoding = Encoding.UTF8;
                client.UploadData(
                    "https://api.go-tellm.com/api/v2/posts/" + postID + "/" + "upvote/", "PUT", new byte[] { });
            }
        }

        public static void Upvote(int indexOfItem)
        {
            string postID = FilterItem(jodelCache, indexOfItem, false);

            using (var client = new WebClient())
            {
                client.UploadData("https://api.go-tellm.com/api/v2/posts/" + postID + "/upvote?access_token=" + accessToken, "PUT", new byte[] { });
            }
        } // cached List<> only

        public static void Downvote(string postID)
        {
            DateTime dt = DateTime.UtcNow;

            string stringified_payload =
                @"PUT%api.go-tellm.com%443%/api/v2/posts/" + postID + "/" + "downvote/%" + accessToken + "%" + $"{dt:s}Z" + "%%";

            var keyByte = Encoding.UTF8.GetBytes(key);
            var hmacsha1 = new HMACSHA1(keyByte);
            hmacsha1.ComputeHash(Encoding.UTF8.GetBytes(stringified_payload));

            using (var client = new WebClient())
            {
                client.Headers.Add("Content-Type", "application/json");
                client.Headers.Add("User-Agent", "Jodel/4.11.2 Dalvik/2.1.0 (Linux; U; Android 6.0.1; Nexus 5 Build/MMB29V)"); //TODO: Randomize
                client.Headers.Add("Accept", "*/*");
                client.Headers.Add("Accept-Encoding", "gzip, deflate");
                client.Headers.Add("X-Client-Type", "android_4.11.2");
                client.Headers.Add("X-Api-Version", "0.2");
                client.Headers.Add("X-Timestamp", $"{dt:s}Z");
                client.Headers.Add("X-Authorization", "HMAC " + ByteToString(hmacsha1.Hash));
                client.Headers.Add("Authorization", "Bearer " + accessToken);
                client.Encoding = Encoding.UTF8;
                client.UploadData(
                    "https://api.go-tellm.com/api/v2/posts/" + postID + "/" + "downvote/", "PUT", new byte[] { });
            }
        }

        public static void Downvote(int indexOfItem)
        {
            string postID = FilterItem(jodelCache, indexOfItem, false);

            using (var client = new WebClient())
            {
                client.UploadData("https://api.go-tellm.com/api/v2/posts/" + postID + "/downvote?access_token=" + accessToken, "PUT", new byte[] { });
            }
        } // cached List<> only

        public static int GetKarma()
        {
            string resp = GetPageContent("https://api.go-tellm.com/api/v2/users/karma?access_token=" + accessToken);
            string result = resp.Substring(resp.LastIndexOf(':') + 1);
            return Convert.ToInt32(result.Replace("}","").Replace("\"",""));
        }

        public static void PostJodel(string message, string color = "FFFFFF")
        {
            if(message.Length > 230)
            {
                throw new Exception("Error posting Jodel: Message is more than 230 characters!");
            }

            string jsonRaw = @"{ color: """ + color + @""",location:{ loc_accuracy:""0.0"",city:"""+city+@""",loc_coordinates:{ lat:""" + latitude + @""",lng: """ + longitude + @"""},country:""" + countryCode + @""",name:"""+city+@"""},message:"""+message+@"""}";
            string json = JObject.Parse(jsonRaw).ToString();

            if (IsValidJson(json))
            {
                GetPageContentPOST("https://api.go-tellm.com/api/v2/posts/", json, true, null, null);
            }
            else
            {
                throw new Exception("Error posting jodel: JSON Object invalid!");
            }
        }

        public static List<Tuple<string, string, string, int>> GetComments(string postID)
        {
            string plainJson = GetPageContent("https://api.go-tellm.com/api/v2/posts/"+postID+"?access_token="+accessToken);
            Comments.RootObject com = JsonConvert.DeserializeObject<Comments.RootObject>(plainJson);
            List<Tuple<string, string, string, int>> comments = new List<Tuple<string, string, string, int>>(); // postID, message, user_handle, votecount

            foreach(var c in com.children)
            {
                comments.Add(new Tuple<string, string, string, int>(c.post_id, c.message, c.user_handle, c.vote_count));
            }

            return comments;
        }

        public static string GenerateAccessToken()
        {
            DateTime dt = DateTime.UtcNow;

            string device_uid = SHA256(RandomString(5, true));

            string stringified_payload
                = @"POST%api.go-tellm.com%443%/api/v2/users/%%" + $"{dt:s}Z" + @"%%{""device_uid"": """ + device_uid +
            @""", ""location"": {""city"": ""Zurich"", ""loc_accuracy"": 100, ""loc_coordinates"": {""lat"": 47.3667, ""lng"": 8.55}, ""country"": ""CH""}, " +
            @"""client_id"": ""81e8a76e-1e02-4d17-9ba0-8a7020261b26""}";

            string payload = @"{""device_uid"": """ + device_uid + @""", ""location"": {""city"": ""Zurich"", ""loc_accuracy"": 100, ""loc_coordinates"": " +
            @"{""lat"": 47.3667, ""lng"": 8.55}, ""country"": ""CH""}, ""client_id"": ""81e8a76e-1e02-4d17-9ba0-8a7020261b26""}";

            var keyByte = Encoding.UTF8.GetBytes(key);
            using (var hmacsha1 = new HMACSHA1(keyByte))
            {
                hmacsha1.ComputeHash(Encoding.UTF8.GetBytes(stringified_payload));

                return GetPageContentPOST("https://api.go-tellm.com/api/v2/users/", payload, false, ByteToString(hmacsha1.Hash), $"{dt:s}Z");
            }
        }

        static string ByteToString(byte[] buff)
        {
            return buff.Aggregate("", (current, t) => current + t.ToString("X2"));
        }

        private static string GetPageContent(string link)
        {
            string html = string.Empty;
            WebRequest request = WebRequest.Create(link);
            WebResponse response = request.GetResponse();
            Stream data = response.GetResponseStream();
            using (StreamReader sr = new StreamReader(data))
            {
                html = sr.ReadToEnd();
            }
            return html;
        }

        private static string GetPageContentPOST(string link, string post, bool bearer, string hmac, string timestamp)
        {
            var request = (HttpWebRequest)WebRequest.Create(link);

            var data = Encoding.UTF8.GetBytes(post);

            request.Method = "POST";
            request.ContentType = "application/json";
            request.ContentLength = data.LongLength;
            request.UserAgent = "Jodel/4.11.2 Dalvik/2.1.0 (Linux; U; Android 6.0.1; Nexus 5 Build/MMB29V)"; //TODO: Randomize
            request.KeepAlive = true;
            request.Accept = "*/*";
            request.Headers.Add("Accept-Encoding", "gzip, deflate");
            request.Headers.Add("X-Client-Type", "android_4.11.2");
            request.Headers.Add("X-Api-Version", "0.2");
            if (timestamp != null)
                request.Headers.Add("X-Timestamp", timestamp);
            if (hmac != null)
                request.Headers.Add("X-Authorization", "HMAC " + hmac);

            if (bearer)
            {
                request.Headers.Add("Authorization", "Bearer " + accessToken);
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

        private static string SHA256(string value)
        {
            StringBuilder Sb = new StringBuilder();

            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                Byte[] result = hash.ComputeHash(enc.GetBytes(value));

                foreach (Byte b in result)
                    Sb.Append(b.ToString("x2"));
            }

            return Sb.ToString();
        }

        private static string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 1; i < size + 1; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            else
                return builder.ToString();
        }

        private static bool IsValidJson(string strInput)
        {
            strInput = strInput.Trim();
            if ((strInput.StartsWith("{") && strInput.EndsWith("}")) || //For object
                (strInput.StartsWith("[") && strInput.EndsWith("]"))) //For array
            {
                try
                {
                    var obj = JToken.Parse(strInput);
                    return true;
                }
                catch (JsonReaderException jex)
                {
                    //Exception in parsing json
                    return false;
                }
                catch (Exception ex) //some other exception
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}