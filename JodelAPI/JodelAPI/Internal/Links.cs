using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using JodelAPI.Shared;

namespace JodelAPI.Internal
{
    internal static class Links
    {
        #region ApiCalls

        // Base URL
        public const string ApiBaseUrl = "api.go-tellm.com";

        // User API Calls
        public static readonly ApiCall Register = new ApiCall(HttpMethod.Post, "/users/", "v2", authorize: false);
        public static readonly ApiCall Refresh = new ApiCall(HttpMethod.Post, "/users/refreshToken", "v2");
        public static readonly ApiCall SetPosition = new ApiCall(HttpMethod.Put, "/users/location", "v2");
        public static readonly ApiCall GetKarma = new ApiCall(HttpMethod.Get, "/users/karma", "v2");
        public static readonly ApiCall GetConfig = new ApiCall(HttpMethod.Get, "/user/config");
        public static readonly ApiCall GetMyPosts = new ApiCall(HttpMethod.Get, "/posts/mine/", "v2");
        public static readonly ApiCall GetMyReplies = new ApiCall(HttpMethod.Get, "/posts/mine/replies/", "v2");
        public static readonly ApiCall GetMyVotes = new ApiCall(HttpMethod.Get, "/posts/mine/votes/", "v2");
        public static readonly ApiCall GetMyCombo = new ApiCall(HttpMethod.Get, "/posts/mine/combo/", "v2");
        public static readonly ApiCall GetMyPopular = new ApiCall(HttpMethod.Get, "/posts/mine/popular/", "v2");
        public static readonly ApiCall GetMyDiscussed = new ApiCall(HttpMethod.Get, "/posts/mine/discussed/", "v2");
        public static readonly ApiCall GetMyPinned = new ApiCall(HttpMethod.Get, "/posts/mine/pinned/", "v2");
        // Post API Calls
        public static readonly ApiCall SetAction = new ApiCall(HttpMethod.Post, "/action");
        public static readonly ApiCall GetPosts = new ApiCall(HttpMethod.Get, "/posts/location/", "v2");
        public static readonly ApiCall GetCombo = new ApiCall(HttpMethod.Get, "/posts/location/combo");
        public static readonly ApiCall GetPopular = new ApiCall(HttpMethod.Get, "/posts/location/popular/", "v2");
        public static readonly ApiCall GetDiscussed = new ApiCall(HttpMethod.Get, "/posts/location/discussed/", "v2");
        public static readonly ApiCall GetPost = new ApiCall(HttpMethod.Get, "/posts/", "v2");
        public static readonly ApiCall Upvote = new ApiCall(HttpMethod.Put, "/posts/", version: "v2", postAction: "/upvote/");
        public static readonly ApiCall Downvote = new ApiCall(HttpMethod.Put, "/posts/", version: "v2", postAction: "/downvote/");
        public static readonly ApiCall Pin = new ApiCall(HttpMethod.Put, "/posts/", version: "v2", postAction: "/pin/");
        public static readonly ApiCall Unpin = new ApiCall(HttpMethod.Put, "/posts/", version: "v2", postAction: "/unpin/");
        public static readonly ApiCall NewPost = new ApiCall(HttpMethod.Post, "/posts/");
        public static readonly ApiCall DeletePost = new ApiCall(HttpMethod.Delete, "/posts/", "v2");
        // Channel API calls
        public static readonly ApiCall GetChannel = new ApiCall(HttpMethod.Get, "/posts/channel/combo/");
        public static readonly ApiCall FollowChannel = new ApiCall(HttpMethod.Put, "/user/followChannel/");
        public static readonly ApiCall UnfollowChannel = new ApiCall(HttpMethod.Put, "/user/unfollowChannel/");
        public static readonly ApiCall GetRecommendedChannels = new ApiCall(HttpMethod.Get, "/user/recommendedChannels");
        public static readonly ApiCall GetFollowedChannelsMeta = new ApiCall(HttpMethod.Post, "/user/followedChannelsMeta");

        // Links
        public const string LinkModeration = "https://api.go-tellm.com/api/v3/moderation/?access_token={AT}";
        public const string LinkReportJodel = "https://api.go-tellm.com/api/v2/posts/{PID}/flag?{AT}";
        
        public const string LinkPostImage = "https://api.go-tellm.com/api/v2/posts?access_token={AT}";

        #endregion
    }
}
