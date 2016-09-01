using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using JodelAPI.Objects;

namespace JodelAPI
{
    static class Helpers
    {
        public static string ByteToString(byte[] buff)
        {
            return buff.Aggregate("", (current, t) => current + t.ToString("X2"));
        }

        public static string Sha256(string value)
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

        public static string RandomString(int size, bool lowerCase)
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

        public static string GetColor(Jodel.PostColor c)
        {
            switch (c)
            {
                case Jodel.PostColor.Red:
                    return "DD5F5F";
                case Jodel.PostColor.Orange:
                    return "FF9908";
                case Jodel.PostColor.Yellow:
                    return "FFBA00";
                case Jodel.PostColor.Blue:
                    return "DD5F5F";
                case Jodel.PostColor.Bluegreyish:
                    return "8ABDB0";
                case Jodel.PostColor.Green:
                    return "9EC41C";
                case Jodel.PostColor.Random:
                    return "FFFFFF";
                default:
                    throw new ArgumentOutOfRangeException(nameof(c), c, null);
            }
        }

        public static Coordinates GetCoords(string address)
        {
            string[] coords = address.ToCoordinates();
            Coordinates coord = new Coordinates
            {
                Latitude = coords[0],
                Longitude = coords[1]
            };

            return coord;
        }

         public static string ToLink(this string link)
        {
            if (link.Contains("{AT}"))
            {
                link = link.Replace("{AT}", Account.AccessToken);
            }

            if (link.Contains("{LAT}"))
            {
                link = link.Replace("{LAT}", Account.Latitude);
            }

            if (link.Contains("{LNG}"))
            {
                link = link.Replace("{LNG}", Account.Longitude);
            }

            return link;
        }

        public static string ToLink(this string link, string postId)
        {
            if (link.Contains("{AT}"))
            {
                link = link.Replace("{AT}", Account.AccessToken);
            }

            if (link.Contains("{LAT}"))
            {
                link = link.Replace("{LAT}", Account.Latitude);
            }

            if (link.Contains("{LNG}"))
            {
                link = link.Replace("{LNG}", Account.Longitude);
            }

            if (link.Contains("{PID}"))
            {
                link = link.Replace("{PID}", postId);
            }



            return link;
        }

        public static WebHeaderCollection ToHeader(this WebHeaderCollection header, string stringifiedPayload, bool addBearer = false)
        {
            header.Remove("X-Authorization");
            header.Remove("X-Timestamp");
            header.Remove("Authorization");

            DateTime dt = DateTime.UtcNow;
            var keyByte = Encoding.UTF8.GetBytes(Constants.Key);
            var hmacsha1 = new HMACSHA1(keyByte);
            hmacsha1.ComputeHash(Encoding.UTF8.GetBytes(stringifiedPayload));

            header.Add("X-Timestamp", $"{dt:s}Z");
            header.Add("X-Authorization", "HMAC " + ByteToString(hmacsha1.Hash));

            if (addBearer)
            {
                header.Add("Authorization", "Bearer " + Account.AccessToken);
            }

            return header;
        }
    }

    class MyWebClient : WebClient
    {
        protected override WebRequest GetWebRequest(Uri address)
        {
            HttpWebRequest request = base.GetWebRequest(address) as HttpWebRequest;
            request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
            return request;
        }
    }
}
