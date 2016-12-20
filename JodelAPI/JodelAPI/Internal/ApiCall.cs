using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using JodelAPI.Json.Request;
using JodelAPI.Shared;

namespace JodelAPI.Internal
{
    internal class ApiCall
    {
        public System.Net.Http.HttpMethod Method { get; }

        public string Url { get; }

        public string Version { get; }

        public string Action { get; }

        public bool Authorize { get; }

        /// <summary>
        /// API Call
        /// </summary>
        /// <param name="method">HTTP Method</param>
        /// <param name="url">URL to be appended to base url</param>
        /// <param name="version">API version to be used</param>
        /// <param name="postAction">Action to be applied to post</param>
        /// <param name="authorize"><c>true</c> if authorization bearer needs to be added to request header, otherwise <c>false</c></param>
        internal ApiCall(System.Net.Http.HttpMethod method, string url, string version = "v3", string postAction = "", bool authorize = true)
        {
            Method = method;
            Url = url;
            Version = version;
            Action = postAction;
            Authorize = authorize;
        }

        internal string ExecuteRequest(User user, Dictionary<string, string> parameters = null, JsonRequest payload = null, string postId = null)
        {
            string plainJson = null;
            string payloadString = payload != null
                ? Newtonsoft.Json.JsonConvert.SerializeObject(payload, Newtonsoft.Json.Formatting.None,
                    new Newtonsoft.Json.JsonSerializerSettings
                    {
                        NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore
                    })
                : null;
            DateTime dt = DateTime.Now;
            string urlParam = Url;
            if (!string.IsNullOrWhiteSpace(postId)) urlParam += postId;
            if (!string.IsNullOrWhiteSpace(Action)) urlParam += Action;
            urlParam += ParamsToString(parameters);
            string stringifiedUrl = "https://" + Links.ApiBaseUrl + "/api/" + Version + urlParam;

            string stringifiedPayload = Method.Method + "%" + Links.ApiBaseUrl + "%443%/api/" + Version + urlParam;
            stringifiedPayload += "%" + user.Token.Token;
            stringifiedPayload += "%" + $"{dt:s}Z" + "%%" + payloadString;

            using (var client = JodelWebClient.GetJodelWebClientWithHeaders(dt, stringifiedPayload, user.Token.Token, Authorize, Method))
            {
#if DEBUG
                Console.WriteLine("****************************************************************");
                Console.WriteLine("{0} {1}", Method, stringifiedUrl);
                Console.WriteLine("----------------------------------------------------------------");
                Console.WriteLine("----------------------------------------------------------------");
                for (int i = 0; i < client.Headers.Count; i++)
                {
                    Console.WriteLine("{0,-30}{1}", client.Headers.AllKeys[i], client.Headers.GetValues(i).Aggregate((a, b) => a + ", " + b));
                }
                Console.WriteLine("----------------------------------------------------------------");
                Console.WriteLine("----------------------------------------------------------------");
                Console.WriteLine(stringifiedPayload);
                Console.WriteLine("----------------------------------------------------------------");
                Console.WriteLine(payloadString);
#endif
                if (Method == HttpMethod.Get)
                {
                    plainJson = client.DownloadString(stringifiedUrl);
                }
                else
                {
                    plainJson = client.UploadString(stringifiedUrl, Method.Method, payloadString ?? string.Empty);
                }
#if DEBUG
                //Console.WriteLine("----------------------------------------------------------------");
                //Console.WriteLine(plainJson);
                Console.WriteLine("****************************************************************");
#endif
            }

            return plainJson;
        }

        private string ParamsToString(Dictionary<string, string> parameters)
        {
            if (parameters == null) return string.Empty;
            return "?" + string.Join("&", parameters.Select(p => p.Key + "=" + p.Value));
        }
    }
}
