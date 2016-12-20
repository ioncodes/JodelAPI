using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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

        internal string ExecuteRequest(string authToken = null, Dictionary<string, string> parameters = null, string payload = null, string postId = null)
        {
            string plainJson = null;
            DateTime dt = DateTime.Now;
            string urlParam = Url;
            if (!string.IsNullOrWhiteSpace(postId)) urlParam += postId;
            if (!string.IsNullOrWhiteSpace(Action)) urlParam += Action;
            urlParam += ParamsToString(parameters);
            string stringifiedUrl = "https://" + Constants.ApiBaseUrl + "/api/" + Version + urlParam;

            string stringifiedPayload = Method.Method + "%" + Constants.ApiBaseUrl + "%443%/api/" + Version + urlParam;
            stringifiedPayload += "%" + authToken;
            stringifiedPayload += "%" + $"{dt:s}Z" + "%%" + payload;

            using (var client = new MyWebClient())
            {
                client.Headers.Add(Constants.Header.ToHeader(stringifiedPayload, dt, Authorize));
                client.Encoding = Encoding.UTF8;
                if (Method == HttpMethod.Get)
                {
                    plainJson = client.DownloadString(stringifiedUrl);
                }
                else
                {
                    plainJson = client.UploadString(stringifiedUrl, Method.Method, payload ?? string.Empty);
                }
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
