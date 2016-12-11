using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace JodelAPI.Internal
{
    internal class JodelWebClient : WebClient
    {
        protected override WebRequest GetWebRequest(Uri address)
        {
            HttpWebRequest request = base.GetWebRequest(address) as HttpWebRequest;
            request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
            request.ServicePoint.Expect100Continue = false;
            return request;
        }

        internal static JodelWebClient GetJodelWebClientWithHeaders(DateTime time, string stringifiedPayload, string accesstoken = "", bool addBearer = false, HttpMethod method = null)
        {
            JodelWebClient client = new JodelWebClient();

            var headers = Constants.Header;

            headers.Remove("X-Authorization");
            headers.Remove("X-Timestamp");
            headers.Remove("Authorization");

            var keyByte = Encoding.UTF8.GetBytes(Constants.Key);
            var hmacsha1 = new HMACSHA1(keyByte);
            hmacsha1.ComputeHash(Encoding.UTF8.GetBytes(stringifiedPayload));

            headers.Add("X-Timestamp", $"{time:s}Z");
            headers.Add("X-Authorization", "HMAC " + hmacsha1.Hash.Aggregate("", (current, t) => current + t.ToString("X2")));

            if (addBearer)
            {
                headers.Add("Authorization", "Bearer " + accesstoken);
            }

            client.Headers.Add(headers);

            if (method != HttpMethod.Get)
                client.Headers.Add(Constants.JsonHeader);

            client.Encoding = Encoding.UTF8;

            return client;
        }

    }
}
