using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JodelAPI
{
    static class Constants
    {
        public const string Key = "iyWpGGuOOCdKIMRsfxoJMIPsmCFdrscSxGyCfmBb";
        public const string ClientId = "81e8a76e-1e02-4d17-9ba0-8a7020261b26";

        // Links
        public const string LinkFirstJodels = "https://api.go-tellm.com/api/v2/posts/location/combo?lat={LAT}&lng={LNG}&access_token={AT}";
        public const string LinkSecondJodels = "https://api.go-tellm.com/api/v2/posts/location?lng={LNG}&lat={LAT}&after={PID}&access_token={AT}&limit=1000";
        public const string LinkUpvoteJodel = "https://api.go-tellm.com/api/v2/posts/{PID}/upvote/";
        public const string LinkDownvoteJodel = "https://api.go-tellm.com/api/v2/posts/{PID}/downvote/";
        public const string LinkGetKarma = "https://api.go-tellm.com/api/v2/users/karma?access_token={AT}";
        public const string LinkPostJodel = "https://api.go-tellm.com/api/v2/posts/";
        public const string LinkGetComments = "https://api.go-tellm.com/api/v2/posts/{PID}?access_token={AT}";
        public const string LinkModeration = "https://api.go-tellm.com/api/v3/moderation/?access_token={AT}";
        public const string LinkGenAT = "https://api.go-tellm.com/api/v2/users/";
        public const string LinkReportJodel = "https://api.go-tellm.com/api/v2/posts/{PID}/flag?{AT}";
        public const string LinkGetMyJodels = "https://api.go-tellm.com/api/v2/posts/mine?limit=150&access_token={AT}&skip=0";
        public const string LinkGetMyComments = "https://api.go-tellm.com/api/v2/posts/mine/replies?skip=0&access_token={AT}&limit=150";
        public const string LinkGetMyVotes = "https://api.go-tellm.com/api/v2/posts/mine/votes?limit=150&access_token={AT}&skip=0";
        public const string LinkConfig = "https://api.go-tellm.com/api/v3/user/config?access_token={AT}";
    }
}
