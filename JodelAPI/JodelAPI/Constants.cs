using System.Net;

namespace JodelAPI
{
    internal class Constants
    {
        // Key Values
        public const string Key = "VwJHzYUbPjGiXWauoVNaHoCWsaacTmnkGwNtHhjy";
        public const string ClientId = "81e8a76e-1e02-4d17-9ba0-8a7020261b26";
        public const string AppVersion = "4.27.0";

        // Links
        public const string LinkFirstJodels =
            "https://api.go-tellm.com/api/v2/posts/location/combo?lat={LAT}&lng={LNG}&access_token={AT}";

        public const string LinkSecondJodels =
            "https://api.go-tellm.com/api/v2/posts/location?lng={LNG}&lat={LAT}&after={PID}&access_token={AT}&limit=1000";

        public const string LinkUpvoteJodel = "https://api.go-tellm.com/api/v2/posts/{PID}/upvote/";
        public const string LinkDownvoteJodel = "https://api.go-tellm.com/api/v2/posts/{PID}/downvote/";
        public const string LinkGetKarma = "https://api.go-tellm.com/api/v2/users/karma?access_token={AT}";
        public const string LinkPostJodel = "https://api.go-tellm.com/api/v2/posts/";
        public const string LinkGetComments = "https://api.go-tellm.com/api/v2/posts/{PID}?access_token={AT}";
        public const string LinkModeration = "https://api.go-tellm.com/api/v3/moderation/?access_token={AT}";
        public const string LinkGenAt = "https://api.go-tellm.com/api/v2/users/";
        public const string LinkUserLocation = "https://api.go-tellm.com/api/v2/users/location?access_token={AT}";
        public const string LinkReportJodel = "https://api.go-tellm.com/api/v2/posts/{PID}/flag?{AT}";

        public const string LinkGetMyJodels =
            "https://api.go-tellm.com/api/v2/posts/mine?limit=150&access_token={AT}&skip=0";

        public const string LinkGetMyComments =
            "https://api.go-tellm.com/api/v2/posts/mine/replies?skip=0&access_token={AT}&limit=150";

        public const string LinkGetMyVotes =
            "https://api.go-tellm.com/api/v2/posts/mine/votes?limit=150&access_token={AT}&skip=0";

        public const string LinkConfig = "https://api.go-tellm.com/api/v3/user/config?access_token={AT}";
        public const string LinkRefreshToken = "https://api.go-tellm.com/api/v2/users/refreshToken?access_token={AT}";
        public const string LinkPinJodel = "https://api.go-tellm.com/api/v2/posts/{PID}/pin?access_token={AT}";

        public const string LinkMyPins =
            "https://api.go-tellm.com/api/v2/posts/mine/pinned?limit=1000&access_token={AT}&skip=0";

        public const string LinkFollowChannel =
            "https://api.go-tellm.com/api/v3/user/followChannel?access_token={AT}&channel={CH}";

        public const string LinkUnfollowChannel =
            "https://api.go-tellm.com/api/v3/user/unfollowChannel?access_token={AT}&channel={CH}";

        public const string LinkGetJodelsFromChannel =
            "https://api.go-tellm.com/api/v3/posts/channel/combo?access_token={AT}&channel={CH}";

        public const string LinkGetRecommendedChannels =
            "https://api.go-tellm.com/api/v3/user/recommendedChannels?access_token={AT}";

        public const string LinkDeleteJodel = "https://api.go-tellm.com/api/v2/posts/{PID}";
        public const string LinkPostImage = "https://api.go-tellm.com/api/v2/posts?access_token={AT}";

        // Headers
        public static WebHeaderCollection Header = new WebHeaderCollection
        {
            {"Accept-Encoding", "gzip, deflate"},
            {"User-Agent", "Jodel/" + AppVersion + " Dalvik/2.1.0 (Linux; U; Android 6.0.1; Nexus 5 Build/MMB29V)"},
            {"Content-Type", "application/json; charset=UTF-8"},
            {"X-Client-Type", "android_" + AppVersion},
            {"X-Api-Version", "0.2"}
        };
    }
}
