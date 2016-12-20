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
        
        #region User

        public static readonly ApiCall GetRequestToken = new ApiCall(HttpMethod.Post, "/users/", "v2", authorize: false);
        public static readonly ApiCall GetNewAccessToken = new ApiCall(HttpMethod.Post, "/users/refreshToken", "v2");
        public static readonly ApiCall SendPushToken = new ApiCall(HttpMethod.Post, "/users/pushToken", "v2");
        public static readonly ApiCall SendUserLocation = new ApiCall(HttpMethod.Put, "/users/location", "v2");
        public static readonly ApiCall SendHomeTown = new ApiCall(HttpMethod.Put, "/user/home");
        public static readonly ApiCall SetProfile = new ApiCall(HttpMethod.Put, "/user/profile");
        public static readonly ApiCall VerifyPush = new ApiCall(HttpMethod.Put, "/user/verification/push");
        public static readonly ApiCall GetKarma = new ApiCall(HttpMethod.Get, "/users/karma", "v2");
        public static readonly ApiCall GetUserConfig = new ApiCall(HttpMethod.Get, "/user/config");
        public static readonly ApiCall GetMyPosts = new ApiCall(HttpMethod.Get, "/posts/mine/", "v2");
        public static readonly ApiCall GetMyRepliedPosts = new ApiCall(HttpMethod.Get, "/posts/mine/replies/", "v2");
        public static readonly ApiCall GetMyVotedPosts = new ApiCall(HttpMethod.Get, "/posts/mine/votes/", "v2");
        public static readonly ApiCall GetMyPostsCombo = new ApiCall(HttpMethod.Get, "/posts/mine/combo/", "v2");
        public static readonly ApiCall GetMyPopularPosts = new ApiCall(HttpMethod.Get, "/posts/mine/popular/", "v2");
        public static readonly ApiCall GetMyMostDiscussedPosts = new ApiCall(HttpMethod.Get, "/posts/mine/discussed/", "v2");
        public static readonly ApiCall GetMyPinnedPosts = new ApiCall(HttpMethod.Get, "/posts/mine/pinned/", "v2");

        #endregion

        #region Posts

        public static readonly ApiCall TrackAction = new ApiCall(HttpMethod.Post, "/action");
        public static readonly ApiCall GetMostRecentPosts = new ApiCall(HttpMethod.Get, "/posts/location/", "v2");
        public static readonly ApiCall GetPostsCombo = new ApiCall(HttpMethod.Get, "/posts/location/combo");
        public static readonly ApiCall GetMostPopularPosts = new ApiCall(HttpMethod.Get, "/posts/location/popular/", "v2");
        public static readonly ApiCall GetMostDiscussedPosts = new ApiCall(HttpMethod.Get, "/posts/location/discussed/", "v2");
        public static readonly ApiCall GetPost = new ApiCall(HttpMethod.Get, "/posts/", "v2");
        public static readonly ApiCall GetPostDetails = new ApiCall(HttpMethod.Get, "/posts/", postAction: "/details");
        public static readonly ApiCall UpvotePost = new ApiCall(HttpMethod.Put, "/posts/", version: "v2", postAction: "/upvote/");
        public static readonly ApiCall DownvotePost = new ApiCall(HttpMethod.Put, "/posts/", version: "v2", postAction: "/downvote/");
        public static readonly ApiCall PinPost = new ApiCall(HttpMethod.Put, "/posts/", version: "v2", postAction: "/pin/");
        public static readonly ApiCall UnpinPost = new ApiCall(HttpMethod.Put, "/posts/", version: "v2", postAction: "/unpin/");
        public static readonly ApiCall SendPost = new ApiCall(HttpMethod.Post, "/posts/");
        public static readonly ApiCall DeletePost = new ApiCall(HttpMethod.Delete, "/posts/", "v2");
        public static readonly ApiCall EnablePostNotification = new ApiCall(HttpMethod.Put, "/posts/", "v2", "/notifications/enable");
        public static readonly ApiCall DisablePostNotification = new ApiCall(HttpMethod.Put, "/posts/", "v2", "/notifications/disable");
        public static readonly ApiCall DismissStickyPost = new ApiCall(HttpMethod.Put, "/stickyposts/", postAction: "/dismiss");
        public static readonly ApiCall UpvoteStickyPost = new ApiCall(HttpMethod.Put, "/stickyposts/", postAction: "/up");
        public static readonly ApiCall DownvoteStickyPost = new ApiCall(HttpMethod.Put, "/stickyposts/", postAction: "/down");
        public static readonly ApiCall FlagPost = new ApiCall(HttpMethod.Put, "/posts/", "v2", "/flag");
        public static readonly ApiCall GiveThanks = new ApiCall(HttpMethod.Post, "/posts/", postAction: "/giveThanks");
        public static readonly ApiCall HidePost = new ApiCall(HttpMethod.Put, "/posts/", postAction: "/hide");

        #endregion

        #region Channels

        public static readonly ApiCall GetChannelCombo = new ApiCall(HttpMethod.Get, "/posts/channel/combo/");
        public static readonly ApiCall GetPopularChannelPosts = new ApiCall(HttpMethod.Get, "/posts/channel/popular");
        public static readonly ApiCall GetDiscussedChannelPosts = new ApiCall(HttpMethod.Get, "/posts/channel/discussed");
        public static readonly ApiCall GetRecentChannelPosts = new ApiCall(HttpMethod.Get, "/posts/channel");
        public static readonly ApiCall FollowChannel = new ApiCall(HttpMethod.Put, "/user/followChannel/");
        public static readonly ApiCall UnfollowChannel = new ApiCall(HttpMethod.Put, "/user/unfollowChannel/");
        public static readonly ApiCall GetRecommendedChannels = new ApiCall(HttpMethod.Get, "/user/recommendedChannels");
        public static readonly ApiCall GetChannelMeta = new ApiCall(HttpMethod.Get, "/user/channelMeta");
        public static readonly ApiCall GetFollowedChannelsMeta = new ApiCall(HttpMethod.Post, "/user/followedChannelsMeta");

        #endregion

        #region Hashtags

        public static readonly ApiCall GetDiscussedHashtagPosts = new ApiCall(HttpMethod.Get, "/posts/hashtag/discussed");
        public static readonly ApiCall GetPopularHashtagPosts = new ApiCall(HttpMethod.Get, "/posts/hashtag/popular");
        public static readonly ApiCall GetHashtagCombo = new ApiCall(HttpMethod.Get, "/posts/hashtag/combo");
        public static readonly ApiCall GetRecentHashtagPosts = new ApiCall(HttpMethod.Get, "/posts/hashtag");

        #endregion

        #region Moderation

        public static readonly ApiCall GetModerationFeed = new ApiCall(HttpMethod.Get, "/moderation");
        public static readonly ApiCall SendModerationResult = new ApiCall(HttpMethod.Post, "/moderation");

        #endregion

        #region Misc

        public static readonly ApiCall SendLogs = new ApiCall(HttpMethod.Put, "/investigate");

        #endregion

        #endregion
    }
}
