using System.Net;
using System.Net.Http;

namespace JodelAPI
{
    internal class Constants
    {
        // Key Values
        public const string Key = "dIHNtHWOxFmoFouufSflpTKYjPmCIhWUCQHgbNzR";
        public const string ClientId = "81e8a76e-1e02-4d17-9ba0-8a7020261b26";
        public const string AppVersion = "4.29.0";

        // Base URL
        public const string ApiBaseUrl = "api.go-tellm.com";

        // User API Calls
        public static readonly ApiCall Register = new ApiCall(HttpMethod.Post, "/users/", "v2", authorize: false);
        public static readonly ApiCall SetPosition = new ApiCall(HttpMethod.Put, "/users/location/");
        public static readonly ApiCall GetKarma = new ApiCall(HttpMethod.Get, "/users/karma/", "v2");
        public static readonly ApiCall GetConfig = new ApiCall(HttpMethod.Get, "/user/config/");
        public static readonly ApiCall GetMyPosts = new ApiCall(HttpMethod.Get, "/posts/mine/", "v2");
        public static readonly ApiCall GetMyReplies = new ApiCall(HttpMethod.Get, "/posts/mine/replies/", "v2");
        public static readonly ApiCall GetMyVotes = new ApiCall(HttpMethod.Get, "/posts/mine/votes/", "v2");
        public static readonly ApiCall GetMyCombo = new ApiCall(HttpMethod.Get, "/posts/mine/combo/", "v2");
        public static readonly ApiCall GetMyPopular = new ApiCall(HttpMethod.Get, "/posts/mine/popular/", "v2");
        public static readonly ApiCall GetMyDiscussed = new ApiCall(HttpMethod.Get, "/posts/mine/discussed/", "v2");
        public static readonly ApiCall GetMyPinned = new ApiCall(HttpMethod.Get, "/posts/mine/pinned/", "v2");
        // Post API Calls
        public static readonly ApiCall GetPosts = new ApiCall(HttpMethod.Get, "/posts/", "v2");
        public static readonly ApiCall GetCombo = new ApiCall(HttpMethod.Get, "/posts/location/combo/");
        public static readonly ApiCall GetPopular = new ApiCall(HttpMethod.Get, "/posts/location/popular/", "v2");
        public static readonly ApiCall GetDiscussed = new ApiCall(HttpMethod.Get, "/posts/location/discussed/", "v2");
        public static readonly ApiCall GetPost = new ApiCall(HttpMethod.Get, "/posts/", "v2");
        public static readonly ApiCall Upvote = new ApiCall(HttpMethod.Put, "/posts/", version: "v2", postAction: "/upvote/");
        public static readonly ApiCall Downvote = new ApiCall(HttpMethod.Put, "/posts/", version: "v2", postAction: "/downvote/");
        public static readonly ApiCall Pin = new ApiCall(HttpMethod.Put, "/posts/", version: "v2", postAction: "/pin/");
        public static readonly ApiCall Unpin = new ApiCall(HttpMethod.Put, "/posts/", version: "v2", postAction: "/unpin/");
        public static readonly ApiCall NewPost = new ApiCall(HttpMethod.Post, "/posts/", "v2");
        public static readonly ApiCall DeletePost = new ApiCall(HttpMethod.Delete, "/posts/", "v2");
        // Channel API calls
        public static readonly ApiCall GetChannel = new ApiCall(HttpMethod.Get, "/posts/channel/combo/");
        public static readonly ApiCall FollowChannel = new ApiCall(HttpMethod.Put, "/user/followChannel/");
        public static readonly ApiCall UnfollowChannel = new ApiCall(HttpMethod.Put, "/user/unfollowChannel/");
        public static readonly ApiCall GetRecommendedChannels = new ApiCall(HttpMethod.Get, "/user/recommendedChannels/");

        // Links
        public const string LinkFirstJodels =
            "https://api.go-tellm.com/api/v3/posts/location/combo?lat={LAT}&lng={LNG}";

        public const string LinkSecondJodels =
            "https://api.go-tellm.com/api/v3/posts/location?lng={LNG}&lat={LAT}&after={PID}&limit=1000";

        public const string LinkUpvoteJodel = "https://api.go-tellm.com/api/v2/posts/{PID}/upvote/";
        public const string LinkDownvoteJodel = "https://api.go-tellm.com/api/v2/posts/{PID}/downvote/";
        public const string LinkGetKarma = "https://api.go-tellm.com/api/v2/users/karma?access_token={AT}";
        public const string LinkPostJodel = "https://api.go-tellm.com/api/v2/posts/";
        public const string LinkGetComments = "https://api.go-tellm.com/api/v2/posts/{PID}?access_token={AT}";
        public const string LinkModeration = "https://api.go-tellm.com/api/v3/moderation/?access_token={AT}";
        public const string LinkGenAt = "https://api.go-tellm.com/api/v2/users/";
        public const string LinkUserLocation = "https://api.go-tellm.com/api/v2/users/location?access_token={AT}";
        public const string LinkReportJodel = "https://api.go-tellm.com/api/v2/posts/{PID}/flag?{AT}";
        public const string LinkLoadFollowedChannels = "https://api.go-tellm.com/api/v3/user/followedChannelsMeta?access_token={AT}";

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

        public const string LinkFollowChannel = "https://api.go-tellm.com/api/v3/user/followChannel?channel={CH}";

        public const string LinkUnfollowChannel = "https://api.go-tellm.com/api/v3/user/unfollowChannel?channel={CH}";

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
