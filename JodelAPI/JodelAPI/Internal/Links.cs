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

        public static readonly ApiCall GetMostRecentPosts = new ApiCall(HttpMethod.Get, "/posts/location/", "v2");
        public static readonly ApiCall GetPostsCombo = new ApiCall(HttpMethod.Get, "/posts/location/combo");
        public static readonly ApiCall GetMostPopularPosts = new ApiCall(HttpMethod.Get, "/posts/location/popular/", "v2");
        public static readonly ApiCall GetMostDiscussedPosts = new ApiCall(HttpMethod.Get, "/posts/location/discussed/", "v2");
        public static readonly ApiCall GetPost = new ApiCall(HttpMethod.Get, "/posts/", "v2");
        public static readonly ApiCall GetPostDetails = new ApiCall(HttpMethod.Get, "/posts/", postAction: "/details");
        
        #endregion

        #region Channels

        public static readonly ApiCall GetChannelCombo = new ApiCall(HttpMethod.Get, "/posts/channel/combo/");
        public static readonly ApiCall GetPopularChannelPosts = new ApiCall(HttpMethod.Get, "/posts/channel/popular");
        public static readonly ApiCall GetDiscussedChannelPosts = new ApiCall(HttpMethod.Get, "/posts/channel/discussed");
        public static readonly ApiCall GetRecentChannelPosts = new ApiCall(HttpMethod.Get, "/posts/channel");
        public static readonly ApiCall GetRecommendedChannels = new ApiCall(HttpMethod.Get, "/user/recommendedChannels");
        public static readonly ApiCall GetChannelMeta = new ApiCall(HttpMethod.Get, "/user/channelMeta");

        #endregion

        #region Hashtags

        public static readonly ApiCall GetDiscussedHashtagPosts = new ApiCall(HttpMethod.Get, "/posts/hashtag/discussed");
        public static readonly ApiCall GetPopularHashtagPosts = new ApiCall(HttpMethod.Get, "/posts/hashtag/popular");
        public static readonly ApiCall GetHashtagCombo = new ApiCall(HttpMethod.Get, "/posts/hashtag/combo");
        public static readonly ApiCall GetRecentHashtagPosts = new ApiCall(HttpMethod.Get, "/posts/hashtag");

        #endregion

        #region Moderation

        public static readonly ApiCall GetModerationFeed = new ApiCall(HttpMethod.Get, "/moderation");

        #endregion

        #region Misc

        #endregion

        #endregion
    }
}
