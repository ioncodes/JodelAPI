using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace JodelAPI.Internal
{
    internal class Constants
    {
        // Key Values
        public const string Key = "zpwKnTvubiKritHEnjOTcTeHxLJJNTEVumuNZqcE";
        public const string ClientId = "81e8a76e-1e02-4d17-9ba0-8a7020261b26";
        public const string AppVersion = "4.30.2";


        // Headers
        public static WebHeaderCollection Header = new WebHeaderCollection
        {
            {"Accept-Encoding", "gzip"},
            {"User-Agent", "Jodel/" + AppVersion + " Dalvik/2.1.0 (Linux; U; Android 6.0.1; E6653 Build/32.2.A.0.305)"},
            {"X-Client-Type", "android_" + AppVersion},
            {"X-Api-Version", "0.2"}
        };
        public static WebHeaderCollection JsonHeader = new WebHeaderCollection
        {
            {"Content-Type", "application/json; charset=UTF-8"}
        };
    }
}
