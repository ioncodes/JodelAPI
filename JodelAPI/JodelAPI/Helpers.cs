using System;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using JodelAPI.Objects;

namespace JodelAPI
{
    internal static class Helpers
    {
        public static User _user;

        public static string ByteToString(byte[] buff)
        {
            return buff.Aggregate("", (current, t) => current + t.ToString("X2"));
        }

        /// <summary>
        /// Generates a random device id
        /// </summary>
        /// <returns>Device ID Hash</returns>
        public static string GetRandomDeviceId()
        {
            byte[] tokenData = new byte[32];
            using (RandomNumberGenerator rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(tokenData);
            }

            using (SHA256 hash = SHA256.Create())
            {
                byte[] result = hash.ComputeHash(tokenData);
                return BitConverter.ToString(result).Replace("-", "").ToLower();
            }
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

        public static string ToLink(this string link, string accessToken = null)
        {
            if (link.Contains("{AT}"))
            {
                link = link.Replace("{AT}", accessToken ?? _user.AccessToken);
            }

            if (link.Contains("{LAT}"))
            {
                link = link.Replace("{LAT}", _user.Latitude);
            }

            if (link.Contains("{LNG}"))
            {
                link = link.Replace("{LNG}", _user.Longitude);
            }

            return link;
        }

        public static string ToLink(this string link, string accessToken, string pid)
        {
            if (link.Contains("{AT}"))
            {
                link = link.Replace("{AT}", accessToken ?? _user.AccessToken);
            }


            if (link.Contains("{PID}"))
            {
                link = link.Replace("{PID}", pid);
            }

            return link;
        }

        public static string ToLinkSecond(this string link, string postIdOrChannel, string accessToken = null)
        {
            if (link.Contains("{AT}"))
            {
                link = link.Replace("{AT}", accessToken ?? _user.AccessToken);
            }

            if (link.Contains("{LAT}"))
            {
                link = link.Replace("{LAT}", _user.Latitude);
            }

            if (link.Contains("{LNG}"))
            {
                link = link.Replace("{LNG}", _user.Longitude);
            }

            if (link.Contains("{PID}"))
            {
                link = link.Replace("{PID}", postIdOrChannel);
            }

            if (link.Contains("{CH}"))
            {
                link = link.Replace("{CH}", postIdOrChannel);
            }

            return link;
        }

        public static WebHeaderCollection ToHeader(this WebHeaderCollection header, string stringifiedPayload,
            DateTime dt, bool addBearer = false)
        {
            header.Remove("X-Authorization");
            header.Remove("X-Timestamp");
            header.Remove("Authorization");

            var keyByte = Encoding.UTF8.GetBytes(Constants.Key);
            var hmacsha1 = new HMACSHA1(keyByte);
            hmacsha1.ComputeHash(Encoding.UTF8.GetBytes(stringifiedPayload));

            header.Add("X-Timestamp", $"{dt:s}Z");
            header.Add("X-Authorization", "HMAC " + ByteToString(hmacsha1.Hash));

            if (addBearer)
            {
                header.Add("Authorization", "Bearer " + _user.AccessToken);
            }

            return header;
        }
    }

    internal class MyWebClient : WebClient
    {
        protected override WebRequest GetWebRequest(Uri address)
        {
            HttpWebRequest request = base.GetWebRequest(address) as HttpWebRequest;
            if (request == null) return null;
            request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
            request.ServicePoint.Expect100Continue = false;
            return request;
        }
    }
}