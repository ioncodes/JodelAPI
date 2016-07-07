using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Web;
using System.IO;
using System.Security.Cryptography;

namespace JodelAPI
{
    public static class API
    {
        public static string accessToken = "";
        public static string latitude = "";
        public static string longitude = "";
        public static string countryCode = "";
        public static string city = "";

        private static List<Tuple<string, string, string, bool>> jodelCache = new List<Tuple<string, string, string, bool>>(); // postid, message, hexcolor, isImage
        private static string lastPostID = "";


        public static List<Tuple<string, string, string, bool>> GetFirstJodels()
        {
            string plainJson = GetPageContent("https://api.go-tellm.com/api/v2/posts/location/combo?lat=" + latitude + "&lng=" + longitude + "&access_token=" + accessToken);
            JodelsFirstRound.RootObject jfr = JsonConvert.DeserializeObject<JodelsFirstRound.RootObject>(plainJson);
            List<Tuple<string, string, string, bool>> temp = new List<Tuple<string, string, string, bool>>(); // List<post_id,message>
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

                temp.Add(new Tuple<string, string, string, bool>(item.post_id, msg, item.color, isURL));

                i++;
            }

            lastPostID = FilterItem(temp, temp.IndexOf(temp.Last()), false); // Set the last post_id for next jodels

            return temp;
        }

        public static List<Tuple<string, string, string, bool>> GetNextJodels()
        {
            List<Tuple<string, string, string, bool>> temp = new List<Tuple<string, string, string, bool>>(); // List<counter,post_id,message>
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

                    temp.Add(new Tuple<string, string, string, bool>(item.post_id, msg, item.color, isURL));
                    i++;
                }

                lastPostID = FilterItem(temp, temp.IndexOf(temp.Last()), false); // Set the last post_id for next jodels
            }
            return temp;
        }

        public static List<Tuple<string, string, string, bool>> GetAllJodels()
        {
            List<Tuple<string, string, string, bool>> allJodels = new List<Tuple<string, string, string, bool>>();
            allJodels = GetFirstJodels();
            allJodels.AddRange(GetNextJodels());
            jodelCache = allJodels;
            return allJodels;
        }

        public static string FilterItem(List<Tuple<string, string, string, bool>> unfiltered, int index, bool filterMessage)
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
            using (var client = new WebClient())
            {
                client.UploadData("https://api.go-tellm.com/api/v2/posts/" + postID + "/upvote?access_token=" + accessToken, "PUT", new byte[] { });
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
            using (var client = new WebClient())
            {
                client.UploadData("https://api.go-tellm.com/api/v2/posts/" + postID + "/downvote?access_token=" + accessToken, "PUT", new byte[] { });
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
            return Convert.ToInt32(GetPageContent("https://api.go-tellm.com/api/v2/users/karma?access_token=" + accessToken));
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
                GetPageContentPOST("https://api.go-tellm.com/api/v2/posts/", json, true);
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

        public static string GenerateAccessToken() //Not working
        {
            string payload = @"{
                client_id: ""81e8a76e-1e02-4d17-9ba0-8a7020261b26"",
                   device_uid: """ + SHA256(RandomString(5, true)) + @""",
                   location:
                       {
                    loc_accuracy: ""19.0"",
                        city: """ + city + @""",
                        loc_coordinates:
                            {
                        lat: """ + latitude + @""",
                             lng: """ + longitude + @"""},
                        country: """+countryCode+@"""}}";


            string json_payload = JObject.Parse(payload).ToString();

            if(IsValidJson(json_payload))
            {
                return GetPageContentPOST("https://api.go-tellm.com/api/v2/users/", json_payload, false);
            }
            else
            {
                throw new Exception("Error generating access token: JSON Object invalid!");
            }
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

        private static string GetPageContentPOST(string link, string post, bool bearer)
        {
            var request = (HttpWebRequest)WebRequest.Create(link);

            var data = Encoding.ASCII.GetBytes(post);

            request.Method = "POST";
            request.ContentType = "application/json; charset=UTF-8";
            request.ContentLength = data.Length;
            request.UserAgent = "Jodel/65000 Dalvik/2.1.0 (Linux; U; Android 5.1.1; D6503 Build/23.4.A.1.232)";
            request.Accept = "gzip";
            request.KeepAlive = true;

            if(bearer)
            {
                request.Headers.Add("Authorization", "Bearer " + accessToken);
            }

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();

            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

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