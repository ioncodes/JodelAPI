using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net;
using System.Web;
using System.IO;

namespace JodelAPI
{
    public static class Main
    {
        public static string accessToken = "";
        public static string latitude = "";
        public static string longitude = "";

        private static string lastPostID = "";

        public static List<Tuple<string, string>> GetFirstJodels()
        {
            string plainJson = GetPageContent("https://api.go-tellm.com/api/v2/posts/location/combo?lat=" + latitude + "&lng=" + longitude + "&access_token=" + accessToken);
            JodelsFirstRound.RootObject jfr = JsonConvert.DeserializeObject<JodelsFirstRound.RootObject>(plainJson);
            List<Tuple<string, string>> temp = new List<Tuple<string, string>>(); // List<counter,post_id,message>
            int i = 0;
            foreach(var item in jfr.recent)
            {
                temp.Add(new Tuple<string, string>(item.post_id, item.message));
                i++;
            }

            lastPostID = FilterItem(temp, temp.IndexOf(temp.Last()), false); // Set the last post_id for next jodels

            return temp;
        }

        public static List<Tuple<string, string>> GetNextJodels()
        {
            List<Tuple<string, string>> temp = new List<Tuple<string, string>>(); // List<counter,post_id,message>
            for (int e = 0; e < 150; e++) // NOT SURE ABOUT 150. IT WORKS THO!!!!
            {
                string plainJson = GetPageContent("https://api.go-tellm.com/api/v2/posts/location?lng=" + longitude + "&lat=" + latitude + "&after=" + lastPostID + "&access_token=" + accessToken + "&limit=1000000");
                JodelsLastRound.RootObject jlr = JsonConvert.DeserializeObject<JodelsLastRound.RootObject>(plainJson);
                int i = 0;
                foreach (var item in jlr.posts)
                {
                    temp.Add(new Tuple<string, string>(item.post_id, item.message));
                    i++;
                }

                lastPostID = FilterItem(temp, temp.IndexOf(temp.Last()), false); // Set the last post_id for next jodels
            }
            return temp;
        }

        public static List<Tuple<string, string>> GetAllJodels()
        {
            List<Tuple<string, string>> allJodels = new List<Tuple<string, string>>();
            allJodels = GetFirstJodels();
            allJodels.AddRange(GetNextJodels());
            return allJodels;
        }

        public static string FilterItem(List<Tuple<string, string>> unfiltered, int index, bool filterMessage)
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

        public static void Upvote(List<Tuple<string, string>> list, int indexOfItem)
        {
            string postID = FilterItem(list, indexOfItem, false);

            using (var client = new WebClient())
            {
                client.UploadData("https://api.go-tellm.com/api/v2/posts/" + postID + "/upvote?access_token=" + accessToken, "PUT", new byte[] { });
            }
        }

        public static int GetKarma()
        {
            return Convert.ToInt32(GetPageContent("https://api.go-tellm.com/api/v2/users/karma?access_token=" + accessToken));
        }

        private static string GetPageContent(string link)
        {
            WebRequest request = WebRequest.Create(link);
            WebResponse response = request.GetResponse();
            Stream data = response.GetResponseStream();
            string html = String.Empty;
            using (StreamReader sr = new StreamReader(data))
            {
                html = sr.ReadToEnd();
            }

            return html;
        }
    }
}