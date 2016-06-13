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

        public static List<Tuple<int, string, string>> GetFirstJodels()
        {
            string plainJson = GetPageContent("https://api.go-tellm.com/api/v2/posts/location/combo?lat=" + latitude + "&lng=" + longitude + "&access_token=" + accessToken);
            JodelsFirstRound.RootObject jfr = JsonConvert.DeserializeObject<JodelsFirstRound.RootObject>(plainJson);
            List<Tuple<int, string, string>> temp = new List<Tuple<int, string, string>>(); // List<counter,post_id,message>
            int i = 0;
            foreach(var item in jfr.recent)
            {
                temp.Add(new Tuple<int, string, string>(i, item.post_id, item.message));
                i++;
            }

            lastPostID = FilterItem(temp, temp.IndexOf(temp.Last()), false); // Set the last post_id for next jodels

            return temp;
        }

        public static List<Tuple<int, string, string>> GetNextJodels()
        {
            string plainJson = GetPageContent("https://api.go-tellm.com/api/v2/posts/location?lng="+longitude+"&lat="+latitude+"&after="+lastPostID+"&access_token="+accessToken+"&limit=1000000");
            JodelsLastRound.RootObject jlr = JsonConvert.DeserializeObject<JodelsLastRound.RootObject>(plainJson);
            List<Tuple<int, string, string>> temp = new List<Tuple<int, string, string>>(); // List<counter,post_id,message>
            int i = 0;
            foreach (var item in jlr.posts)
            {
                temp.Add(new Tuple<int, string, string>(i, item.post_id, item.message));
                i++;
            }

            return temp;
        }

        public static List<Tuple<int, string, string>> GetAllJodels()
        {
            List<Tuple<int, string, string>> allJodels = new List<Tuple<int, string, string>>();
            allJodels = GetFirstJodels();
            allJodels.AddRange(GetNextJodels());
            return allJodels;
        }

        public static string FilterItem(List<Tuple<int, string, string>> unfiltered, int index, bool filterMessage)
        {
            int i = unfiltered.FindIndex(t => t.Item1 == index);
            if (!filterMessage)
            {
                return unfiltered[i].Item2;
            }
            else
            {
                return unfiltered[i].Item3;
            }
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
