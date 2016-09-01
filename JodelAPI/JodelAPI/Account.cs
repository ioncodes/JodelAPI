using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JodelAPI
{
    public static class Account
    {
        public static string AccessToken = "";
        public static string Latitude = "";
        public static string Longitude = "";
        public static string CountryCode = "";
        public static string City = "";
        public static string GoogleApiToken = "";

        /// <summary>
        /// Gets the karma.
        /// </summary>
        /// <returns>System.Int32.</returns>
        public static int GetKarma()
        {
            string resp;
            using (var client = new MyWebClient())
            {
                resp = client.DownloadString(Constants.LinkGetKarma.ToLink());
            }
            string result = resp.Substring(resp.LastIndexOf(':') + 1);
            return Convert.ToInt32(result.Replace("}", "").Replace("\"", ""));
        }
    }
}
