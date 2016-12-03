using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JodelAPI
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

        internal string ExecuteRequest(Dictionary<string, string> parameters = null)
        {
            string plainJson;
            DateTime dt = DateTime.Now;
            string payload = "{}";
            string urlParam = Url + ParamsToString(parameters);
            string stringifiedPayload = Method.Method + "%" + Constants.ApiBaseUrl + "%443%api/" + Version + urlParam + "%" + $"{dt:s}Z" + "%%" + payload;
            string stringifiedUrl = "https://" + Constants.ApiBaseUrl + "/api/" + Version + urlParam;
            
            using (var client = new MyWebClient())
            {
                client.Headers.Add(Constants.Header.ToHeader(stringifiedPayload, DateTime.UtcNow, Authorize));
                client.Encoding = Encoding.UTF8;
                plainJson = client.DownloadString(stringifiedUrl);
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
